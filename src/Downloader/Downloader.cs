namespace Downloader
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensibility;
    using Extensions;
    using Microsoft.Extensions.Logging;

    [Export(typeof(IDownloader))]
    public sealed class Downloader : IDownloader
    {
        private readonly IProtocolHandlerFactory handlerFactory;
        private readonly IDownloaderOptions options;

        private readonly ILogger Logger = AppLogger.CreateLogger<Downloader>();

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

        public async Task DownloadAsync(IEnumerable<Uri> uris, CancellationToken tk)
        {
            foreach (var uri in uris)
            {
                var cleanUri = uri.CleanUri();
                var credentials = uri.GetCredentials();
                var fileName = Path.Combine(Path.GetTempPath(), cleanUri.GetMd5());
                var destFile = Path.Combine(this.options.OutputPath, cleanUri.GetFileName());
                var result = await this.ProcessUri(cleanUri,  fileName, credentials, tk);
                if (result == CompletedState.Succeeded)
                {
                    await CopyAsync(fileName, destFile, tk);
                }
            }
        }

        private static void CleanUpPreviousData(string tempFileName)
        {
            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }
        }

        private static void Configure(IDownloaderOptions options)
        {
            Directory.CreateDirectory(options.OutputPath);
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

        private async Task<CompletedState> BeginDownloadAsync(Uri uri, string fileName, ICredentials credentials, CancellationToken tk)
        {
            var handler = this.handlerFactory.GetHandler(uri);
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                   return await handler.DownloadAsync(uri, fileStream, credentials, tk);
            }
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
    }
}