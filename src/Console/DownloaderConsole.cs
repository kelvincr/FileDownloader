// <copyright file="DownloaderConsole.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using WebHandlers;

namespace Console
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CommandLine;
    using Downloader;
    using Extensibility;
    using Figgle;
    using Microsoft.Extensions.Logging;
    using NetEscapades.Extensions.Logging.RollingFile;

    /// <summary>
    /// Entry Point program.
    /// </summary>
    internal class DownloaderConsole
    {
        private static ILogger logger;
        private static CancellationTokenSource cancellationTokenSource;

        private static void Main(string[] args)
        {
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;
            cancellationTokenSource = new CancellationTokenSource();
            InitLogger();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(async opts => await RunOptionsAndReturnExitCode(opts))
                .WithNotParsed(HandleParseError);
            Task.WaitAll();
            Console.WriteLine(Resources.Press_Any_Key_to_Exit);
            Console.ReadKey();
        }

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            foreach (var err in errs)
            {
                logger.LogError(err.ToString());
            }
        }

        private static async Task RunOptionsAndReturnExitCode(Options opts)
        {
            PrintBanner();
            var uris = opts.InputFiles.Where(IsValidUri).Select(x => new Uri(x));
            var options = Loader.Load<IDownloaderOptions>();
            options.OutputPath = Path.GetFullPath(opts.OutputPath);
            var handlersFactory = Loader.Load<IProtocolHandlerFactory>();

            //// TODO Investigate File Not found, using ImportingConstructor, MEF container.
            //// var downloader = Loader.Load<IDownloader>();
            var downloader = new Downloader(handlersFactory, options);
            Console.WriteLine(Resources.Download_Async_Started);
            await downloader.DownloadAsync(uris, cancellationTokenSource.Token);
            Task.WaitAll();
            Console.WriteLine(Resources.Download_Async_Completed);
        }

        private static void PrintBanner()
        {
            var defaultBackgroundColor = Console.BackgroundColor;
            var defaultForegroundColor = Console.ForegroundColor;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(
                FiggleFonts.Standard.Render("Welcome !"));
            Console.WriteLine(
                FiggleFonts.Standard.Render("File Downloader"));
            Console.WriteLine($@"Version: {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}");
            Console.WriteLine($@"Author: Kelvin Jimenez, 2018.");
            Console.BackgroundColor = defaultBackgroundColor;
            Console.ForegroundColor = defaultForegroundColor;
        }

        private static bool IsValidUri(string uri)
        {
            if (!Uri.TryCreate(uri, UriKind.Absolute, out var outUri))
            {
                Console.Error.WriteLine(Resources.InvalidUri, uri);
                return false;
            }

            return true;
        }

        private static void InitLogger()
        {
            var options = new FileLoggerOptions
            {
                FileName = "DownloaderConsole-",
                LogDirectory = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().Location),  "LogFiles"),
                FileSizeLimit = 5 * 1024 * 1024
            };

            AppLogger.LoggerFactory.AddProvider(new FileLoggerProvider(new MyLoggerOptions(options)));
            logger = AppLogger.CreateLogger<DownloaderConsole>();
        }
    }
}