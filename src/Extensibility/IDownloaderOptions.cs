namespace Extensibility
{
    public interface IDownloaderOptions
    {
        int MaxRetries { get; }

        string OutputPath { get; }
    }
}