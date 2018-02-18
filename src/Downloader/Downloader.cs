// <copyright file="Downloader.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Downloader
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using DataAccess;
    using Extensibility;
    using Extensions;
    using Microsoft.Extensions.Logging;

    /// <summary>
    ///     <see cref="Downloader" /> Implementation.
    /// </summary>
    /// <seealso cref="Extensibility.IDownloader" />
    [Export(typeof(IDownloader))]
    public sealed class Downloader : IDownloader
    {
        private readonly IProtocolHandlerFactory handlerFactory;

        private readonly ILogger logger = AppLogger.CreateLogger<Downloader>();
        private readonly IDownloaderOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="Downloader"/> class.
        /// </summary>
        public Downloader()
        {
            // Intentional left in blank, Mef requires this Constructor.
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Downloader" /> class.
        /// </summary>
        /// <param name="handlerFactory">The handler factory.</param>
        /// <param name="options">The options.</param>
        [ImportingConstructor]
        public Downloader(IProtocolHandlerFactory handlerFactory, IDownloaderOptions options)
        {
            this.options = options;
            this.handlerFactory = handlerFactory;
            Configure(this.options);
        }

        /// <summary>
        ///     Downloads asynchronous.
        /// </summary>
        /// <param name="uris">The Uri collection to process.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>Task Completion.</returns>
        public async Task DownloadAsync(IEnumerable<Uri> uris, CancellationToken cancellationToken)
        {
            var getUriTasks = uris.Select(uri => this.DownloadUriAsync(uri, cancellationToken));
            await Task.WhenAll(getUriTasks.ToList());
        }

        private static void CleanUpPreviousData(string tempFileName)
        {
            if (System.IO.File.Exists(tempFileName))
            {
                System.IO.File.Delete(tempFileName);
            }
        }

        private static void Configure(IDownloaderOptions options)
        {
            Directory.CreateDirectory(options.OutputPath);
        }

        private static async Task CopyAsync(string filename, string destFile, CancellationToken cancellationToken)
        {
            using (var sourceStream = System.IO.File.Open(filename, FileMode.Open))
            {
                using (var destinationStream = System.IO.File.Create(destFile))
                {
                    await sourceStream.CopyToAsync(destinationStream, 1024 * 1024, cancellationToken);
                }
            }

            if (cancellationToken.IsCancellationRequested)
            {
                CleanUpPreviousData(filename);
            }
        }

        private static void RegisterOnDataBase(string server, string name, string filePath, long length, string mime)
        {
            var db = new DataBase();
            var fileInfo = new FileInfo(filePath);
            var size = length > 0 ? length : fileInfo.Length;
            var extension = fileInfo.Extension?.Replace('.', '\0');
            db.StoreFile(server, name, filePath, size, mime, extension, fileInfo.CreationTime);
            Console.WriteLine(Resources.Registered_on_DB, server, name);
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

        private async Task DownloadUriAsync(Uri uri, CancellationToken cancellationToken)
        {
            Console.WriteLine(Resources.Start_Downloading, uri);
           var cleanUri = uri.CleanUri();
            var credentials = uri.GetCredentials();
            var handler = this.handlerFactory.GetHandler(uri);
            var metadata = handler.FetchMetadata(cleanUri, credentials, cancellationToken);
            var fileName = Path.Combine(Path.GetTempPath(), cleanUri.GetMd5());
            var destFile = Path.Combine(this.options.OutputPath, cleanUri.GetFileName());
            var result = this.ProcessUri(cleanUri, fileName, credentials, cancellationToken);
            Console.WriteLine(Resources.Downloaded_to_tmp, cleanUri);
            if (result == CompletedState.Succeeded)
            {
                await CopyAsync(fileName, destFile, cancellationToken).ContinueWith(
                    (obj) => RegisterOnDataBase(cleanUri.Host, cleanUri.PathAndQuery, destFile, metadata.Size, metadata.Mime), cancellationToken);
            }
        }

        private CompletedState ProcessUri(Uri uri, string fileName, ICredentials credentials, CancellationToken tk)
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
                            result = this.ContinueDownloadAsync(uri, fileName, credentials, tk).GetAwaiter().GetResult();
                            break;
                        case CompletedState.Failed:
                            CleanUpPreviousData(fileName);
                            result = this.BeginDownloadAsync(uri, fileName, credentials, tk).GetAwaiter().GetResult();
                            break;
                        default:
                            result = this.BeginDownloadAsync(uri, fileName, credentials, tk).GetAwaiter().GetResult();
                            break;
                    }

                    attempts--;
                }
                catch (Exception exception)
                {
                    this.logger.LogInformation(
                        string.Format(Resources.EWI002, this.options.MaxAttempts - attempts, attempts, exception));
                    Console.Error.WriteLine(Resources.EWI002, this.options.MaxAttempts - attempts, attempts, exception);
                    attempts--;
                }

                // This is required in case it was the last attempt and the task is canceled.
                if (result != CompletedState.Succeeded)
                {
                    CleanUpPreviousData(fileName);
                }

                if (result == CompletedState.Succeeded)
                {
                    break;
                }
            }

            return result;
        }
    }
}