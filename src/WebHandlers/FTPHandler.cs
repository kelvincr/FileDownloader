﻿// <copyright file="FTPHandler.cs" company="Corp">
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
    /// FTP handler.
    /// </summary>
    /// <seealso cref="WebHandlers.BaseHandler" />
    /// <seealso cref="Extensibility.IProtocolHandler" />
    /// <inheritdoc cref="Extensibility.IProtocolHandler" />
    [Export(typeof(IProtocolHandler))]
    public class FtpHandler : BaseHandler, IProtocolHandler
    {
        /// <inheritdoc cref="IProtocolHandler"/>
        public IEnumerable<string> Scheme => new[] { "ftp", "sftp" };

        /// <inheritdoc cref="IProtocolHandler"/>
        public(long Size, string Mime) FetchMetadata(Uri uri, ICredentials credentials, CancellationToken cancellationToken)
        {
            try
            {
                var request = (FtpWebRequest)WebRequest.Create(uri);
                request.Credentials = credentials;
                request.Method = WebRequestMethods.Ftp.GetFileSize;
                var response = request.GetResponse();
                return (response.ContentLength, string.Empty);
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
            var request = (FtpWebRequest)WebRequest.Create(uri);
            request.Credentials = credentials ?? new NetworkCredential("anonymous", string.Empty);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            return await this.GetResponseAsync(writeStream, request, responseOffset, cancellationToken);
        }
    }
}