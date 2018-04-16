using System;
using System.Composition;

namespace Downloader
{
    using System.IO;

    [Export(typeof(IDownloaderOptions))]
    public class DownloaderOptions : IDownloaderOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DownloaderOptions"/> class.
        /// </summary>
        public DownloaderOptions()
        {
            this.OutputPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), Resources.DefaultOutput);
            this.MaxAttempts = 3;
            this.DelayBetweenAttempts = TimeSpan.FromSeconds(10);
        }

        public int MaxAttempts { get; }

        public TimeSpan DelayBetweenAttempts { get; }

        public string OutputPath { get; }
    }
}
