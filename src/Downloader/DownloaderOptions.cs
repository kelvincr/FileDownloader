namespace Downloader
{
    using System.IO;

    public class DownloaderOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DownloaderOptions"/> class.
        /// </summary>
        public DownloaderOptions()
        {
            this.Output = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), Resources.DefaultOutput);
            this.MaxRetries = 5;
        }

        public int MaxRetries { get; }

        public string Output { get; }
    }
}
