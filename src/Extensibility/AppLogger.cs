// <copyright file="AppLogger.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Extensibility
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

        /// <summary>
        /// Gets the logger factory.
        /// </summary>
        /// <value>
        /// The logger factory.
        /// </value>
        public static ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <typeparam name="T">The Type of the Object requesting a logger.</typeparam>
        /// <returns>A new instance of the logger.</returns>
        public static ILogger CreateLogger<T>()
        {
            return LoggerFactory.CreateLogger<T>();
        }
    }
}