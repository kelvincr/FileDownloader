// <copyright file="Options.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Console
{
    using System.Collections.Generic;
    using CommandLine;

    /// <summary>
    /// Console Options.
    /// </summary>
    internal class Options
    {
        /// <summary>
        /// Gets or sets the input files.
        /// </summary>
        /// <value>
        /// The input files.
        /// </value>
        [Option('i', "input", Required = true, HelpText = "Input Uris to be processed.")]
        public IEnumerable<string> InputFiles { get; set; }

        /// <summary>
        /// Gets or sets the output path.
        /// </summary>
        /// <value>
        /// The output path.
        /// </value>
        [Option('o', "output", Default = "DownloaderFiles", HelpText = "OutputFile for Downloads.")]
        public string OutputPath { get; set; }
    }
}
