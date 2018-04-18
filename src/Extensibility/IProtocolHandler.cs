// <copyright file="IProtocolHandler.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Extensibility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// protocol Handler Interface.
    /// </summary>
    public interface IProtocolHandler
    {
        /// <summary>
        /// Gets the scheme.
        /// </summary>
        /// <value>
        /// The scheme.
        /// </value>
        IEnumerable<string> Scheme { get; }

        /// <summary>
        /// Fetches the size asynchronous.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="credentials">The credentials.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The size of the Uri.</returns>
        Task<long> FetchSizeAsync(Uri uri, ICredentials credentials, CancellationToken cancellationToken);

        /// <summary>
        /// Downloads the asynchronous.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="writeStream">The write stream.</param>
        /// <param name="credentials">The credentials.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>State of the Download.</returns>
        Task<CompletedState> DownloadAsync(
            Uri uri, Stream writeStream, ICredentials credentials, CancellationToken cancellationToken);

        /// <summary>
        /// Continues the download asynchronous.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="writeStream">The write stream.</param>
        /// <param name="responseOffset">The response offset.</param>
        /// <param name="credentials">The credentials.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The state of the download.</returns>
        Task<CompletedState> ContinueDownloadAsync(
            Uri uri, Stream writeStream, long responseOffset, ICredentials credentials, CancellationToken cancellationToken);
    }
}