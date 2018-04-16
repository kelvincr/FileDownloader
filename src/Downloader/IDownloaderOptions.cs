using System;

namespace Downloader
{
    public interface IDownloaderOptions
    {
        int MaxAttempts { get; }

        TimeSpan DelayBetweenAttempts { get; }

        string OutputPath { get; }
    }
}