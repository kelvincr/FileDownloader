// <copyright file="IDownloader.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Extensibility
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Download Interface.
    /// </summary>
    public interface IDownloader
    {
        /// <summary>
        /// Downloads asynchronous.
        /// </summary>
        /// <param name="uris">The Uris.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Complementation of the task.</returns>
        Task DownloadAsync(IEnumerable<Uri> uris, CancellationToken cancellationToken);
    }
}