using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace Console
{
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
        [Option('o', "output", Default = "DownloaderFiles" , HelpText = "OutputFile for Downloads.")]
        public string OutputPath { get; set; }
    }
}
