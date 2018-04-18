// <copyright file="HttpHandler.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WebHandlers
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
    /// Http Handler.
    /// </summary>
    /// <seealso cref="WebHandlers.BaseHandler" />
    /// <seealso cref="Extensibility.IProtocolHandler" />
    /// <inheritdoc cref="Extensibility.IProtocolHandler" />
    [Export(typeof(IProtocolHandler))]
    public class HttpHandler : BaseHandler, IProtocolHandler
    {
        /// <inheritdoc cref="IProtocolHandler"/>
        public IEnumerable<string> Scheme => new[] { "http", "https" };

        /// <inheritdoc cref="IProtocolHandler"/>
        public(long Size, string Mime) FetchMetadata(Uri uri, ICredentials credentials, CancellationToken cancellationToken)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Credentials = credentials;
                request.Method = WebRequestMethods.Http.Head;
                var response = request.GetResponse();
                return (response.ContentLength, response.ContentType);
            }
            catch (Exception)
            {
                return (-1, string.Empty);
            }
        }

        /// <inheritdoc cref="IProtocolHandler"/>
        public async Task<CompletedState> DownloadAsync(
            Uri uri, Stream writeStream, ICredentials credentials, CancellationToken cancellationToken)
        {
            return await this.ContinueDownloadAsync(uri, writeStream, 0, credentials, cancellationToken);
        }

        /// <inheritdoc cref="IProtocolHandler"/>
        public async Task<CompletedState> ContinueDownloadAsync(
            Uri uri, Stream writeStream, long responseOffset, ICredentials credentials, CancellationToken cancellationToken)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Credentials = credentials;
            request.Method = WebRequestMethods.File.DownloadFile;
            return await this.GetResponseAsync(writeStream, request, responseOffset, cancellationToken);
        }
    }
}