namespace Downloader.Test
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensibility;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DownloaderTest
    {
        /// <summary>
        /// Downloaders the is resolve using mef.
        /// </summary>
        [TestMethod]
        public void DownloaderIsResolveUsingMef()
        {
            var downloader = Loader.Load<IDownloader>();
            downloader.Should().NotBeNull().And.BeOfType<Downloader>();
        }

        /// <summary>
        /// Downloader should download in parallel.
        /// </summary>
        /// <returns>Task Completed.</returns>
        [TestMethod]
        public async Task DownloaderShouldDownloadInParallel()
        {
            var downloader = Loader.Load<IDownloader>();
            downloader.Should().NotBeNull().And.BeOfType<Downloader>();
            var testUris = new[] { new Uri("test://server/6"), new Uri("test://server/8"), new Uri("test://server/9") };
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await downloader.DownloadAsync(testUris, new CancellationTokenSource().Token);
            Task.WaitAll();
            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed.Seconds;
            elapsedTime.Should().BeLessThan(10);

        }

        //[TestMethod]
        //public void DownloaderShould

    }
}
