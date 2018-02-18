// <copyright file="MyLoggerOptions.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Console
{
    using Microsoft.Extensions.Options;
    using NetEscapades.Extensions.Logging.RollingFile;

    /// <inheritdoc />
    public class MyLoggerOptions : IOptions<FileLoggerOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyLoggerOptions"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MyLoggerOptions(FileLoggerOptions options)
        {
            this.Value = options;
        }

        /// <inheritdoc />
        public FileLoggerOptions Value { get; }
    }
}