namespace Downloader
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    ///     Application logger.
    /// </summary>
    public static class AppLogger
    {
        static AppLogger()
        {
            LoggerFactory = new LoggerFactory();
        }

        public static ILoggerFactory LoggerFactory { get; }

        public static ILogger CreateLogger<T>()
        {
            return LoggerFactory.CreateLogger<T>();
        }
    }
}