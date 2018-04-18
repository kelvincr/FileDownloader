// <copyright file="TestHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Downloader.Test.Stubs
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensibility;

    /// <summary>
    ///     Test Handler
    /// </summary>
    /// <seealso cref="Extensibility.IProtocolHandler" />
    [Export(typeof(IProtocolHandler))]
    public class TestHandler : IProtocolHandler
    {
        /// <inheritdoc />
        public IEnumerable<string> Scheme => new[] { "Test" };

        /// <inheritdoc />
        public Task<CompletedState> ContinueDownloadAsync(Uri uri, Stream writeStream, long responseOffset, ICredentials credentials, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<CompletedState> DownloadAsync(Uri uri, Stream writeStream, ICredentials credentials, CancellationToken cancellationToken)
        {
            var time = int.Parse(uri.AbsolutePath.Substring(1));
            var result = 0;
            result = await LoopMethod(time, result);
            Console.WriteLine(result);
            return CompletedState.Succeeded;
        }

        /// <inheritdoc />
        public (long Size, string Mime) FetchMetadata(Uri uri, ICredentials credentials, CancellationToken cancellationToken)
        {
            const long size = -1;
            return (size, string.Empty);
        }

        private static async Task<int> LoopMethod(int time, int result)
        {
            for (var i = 0; i < time * 10000; i++)
            {
                result++;
            }

            return result;
        }
    }
}