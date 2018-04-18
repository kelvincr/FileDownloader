// <copyright file="IDownloaderOptions.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Extensibility
{
    /// <summary>
    ///     DownloaderOptions Interface.
    /// </summary>
    public interface IDownloaderOptions
    {

        /// <summary>
        /// Gets or sets the maximum attempts.
        /// </summary>
        /// <value>
        /// The maximum attempts.
        /// </value>
        int MaxAttempts { get; set; }

        /// <summary>
        /// Gets or sets the output path.
        /// </summary>
        /// <value>
        /// The output path.
        /// </value>
        string OutputPath { get; set; }
    }
}