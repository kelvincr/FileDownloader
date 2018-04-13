namespace Downloader
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using Extensibility;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class Downloader : IDownloader
    {
        private ILogger Logger => AppLogger.CreateLogger<Downloader>();

        /// <summary>
        /// Gets or sets the protocol handlers.
        /// </summary>
        /// <value>
        /// The protocol handlers.
        /// </value>
        [ImportMany]
        public IEnumerable<IProtocolHandler> Handlers { get; set; }

        public void Download(IEnumerable<Uri> sources, IOptions<DownloaderOptions> options)
        {

        }
    }
}