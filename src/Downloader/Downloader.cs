using System.Text.RegularExpressions;

namespace Downloader
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensibility;
    using Microsoft.Extensions.Logging;

    [Export(typeof(IDownloader))]
    public sealed class Downloader : IDownloader
    {
        private readonly IDownloaderOptions options;
        private readonly IProtocolHandlerFactory handlerFactory;

        public Downloader()
        {
        }

        [ImportingConstructor]
        public Downloader(IProtocolHandlerFactory handlerFactory, IDownloaderOptions options)
        {
            this.options = options;
            this.handlerFactory = handlerFactory;
            Configure(this.options);
        }

        private ILogger Logger => AppLogger.CreateLogger<Downloader>();

        public async Task DownloadAsync(IEnumerable<Uri> uris, CancellationToken tk)
        {
            foreach (var uri in uris)
            {
                var data = GetCredentials(uri);
                var fileName = Path.Combine(Path.GetTempPath(), GetMd5(data.uri.ToString()));
                var destFile = Path.Combine(this.options.OutputPath, this.GetNewFileName(data.uri.ToString()));
                var result = await this.ProcessUri(data.uri,  fileName, data.credentials, tk);
                if (result == CompletedState.Succeeded)
                {
                    await CopyAsync(fileName, destFile, tk);
                }
            }
        }

        private static async Task CopyAsync(string filename, string destFile, CancellationToken cancellationToken)
        {
            using (var sourceStream = File.Open(filename, FileMode.Open))
            {
                using (var destinationStream = File.Create(destFile))
                {
                    await sourceStream.CopyToAsync(destinationStream, 1024 * 1024, cancellationToken);
                }
            }

            if (cancellationToken.IsCancellationRequested)
            {
                CleanUpPreviousData(filename);
            }
        }

        private string GetNewFileName(string url)
        {
            var urlParts = new List<string>();
            var stringBuilder = new StringBuilder();
            var r = new Regex(@"[a-z]+", RegexOptions.IgnoreCase);
            foreach (Match m in r.Matches(url))
            {
                urlParts.Add(m.Value);
            }

            foreach (var t in urlParts)
            {
                stringBuilder.Append(t);
                stringBuilder.Append("_");
            }

            return stringBuilder.ToString();
        }

        private static void CleanUpPreviousData(string tempFileName)
        {
            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }
        }

        private static string GetMd5(string input)
        {
            var md5 = new MD5CryptoServiceProvider();

            var originalBytes = Encoding.Default.GetBytes(input);
            var encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes).Replace("-", string.Empty);
        }

        private async Task<CompletedState> BeginDownloadAsync(Uri uri, string fileName, ICredentials credentials, CancellationToken tk)
        {
            var handler = this.handlerFactory.GetHandler(uri);
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                   return await handler.DownloadAsync(uri, fileStream, credentials, tk);
            }
        }

        private static void Configure(IDownloaderOptions options)
        {
            Directory.CreateDirectory(options.OutputPath);
        }

        private static (Uri uri, NetworkCredential credentials) GetCredentials(Uri uri)
        {
            var userInfo = uri.UserInfo.Split(':');
            var networkCredentials = userInfo.Length == 2 ? new NetworkCredential(userInfo[0], userInfo[1]) : null;
            return (new Uri(uri.OriginalString), networkCredentials);
        }

        private async Task<CompletedState> ProcessUri(Uri uri, string fileName, ICredentials credentials, CancellationToken tk)
        {
            var attempts = this.options.MaxAttempts;
            var result = CompletedState.NonStarted;
            while (attempts > 0)
            {
                try
                {
                    switch (result)
                    {
                        case CompletedState.Succeeded:
                        case CompletedState.Canceled:
                            attempts = 0;
                            break;
                        case CompletedState.Partial:
                            result = await this.ContinueDownloadAsync(uri, fileName, credentials, tk);
                            break;
                        case CompletedState.Failed:
                            CleanUpPreviousData(fileName);
                            result = await this.BeginDownloadAsync(uri, fileName, credentials, tk);
                            break;
                        default:
                            result = await this.BeginDownloadAsync(uri, fileName, credentials, tk);
                            break;
                    }

                    attempts--;
                }
                catch (Exception exception)
                {
                    this.Logger.LogInformation(
                        string.Format(Resources.EWI002, this.options.MaxAttempts - attempts, attempts, exception));
                    attempts--;
                }

                // This is required in case it was the last attempt and the task is cancelled.
                if (result != CompletedState.Succeeded)
                {
                    CleanUpPreviousData(fileName);
                }
            }

            return result;
        }

        private async Task<CompletedState> ContinueDownloadAsync(Uri uri, string fileName, ICredentials credentials, CancellationToken tk)
        {
            var handler = this.handlerFactory.GetHandler(uri);
            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                fileStream.Seek(0, SeekOrigin.End);
                return await handler.ContinueDownloadAsync(uri, fileStream, fileStream.Position, credentials, tk);
            }
        }
    }
}