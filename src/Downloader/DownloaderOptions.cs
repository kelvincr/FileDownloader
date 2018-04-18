// <copyright file="DownloaderOptions.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using Extensibility;

namespace Downloader
{
    using System;
    using System.Composition;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Default Downloader Options.
    /// </summary>
    /// <seealso cref="IDownloaderOptions" />
    [Export(typeof(IDownloaderOptions))]
    public class DownloaderOptions : IDownloaderOptions
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DownloaderOptions" /> class.
        /// </summary>
        public DownloaderOptions()
        {
            this.OutputPath = Path.Combine(
                GetCurrentDirectory(),
                Resources.DefaultOutput);
            this.MaxAttempts = 3;
        }

        /// <inheritdoc />
        public int MaxAttempts { get; }

        /// <inheritdoc />
        public string OutputPath { get; }

        private static string GetCurrentDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException();
        }
    }
}