// <copyright file="ProtocolHandlerFactory.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Downloader
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using Extensibility;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Protocol Handler Factory.
    /// </summary>
    /// <seealso cref="Extensibility.IProtocolHandlerFactory" />
    [Export(typeof(IProtocolHandlerFactory))]
    public class ProtocolHandlerFactory : IProtocolHandlerFactory
    {
        private readonly Dictionary<string, Type> handlers = new Dictionary<string, Type>();

        private readonly ILogger Logger = AppLogger.CreateLogger<ProtocolHandlerFactory>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolHandlerFactory"/> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        [ImportingConstructor]
        public ProtocolHandlerFactory([ImportMany] IEnumerable<IProtocolHandler> handlers)
        {
            foreach (var handler in handlers)
            {
                this.RegisterHandler(handler);
            }
        }

        /// <summary>
        /// Gets the handler.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>An Instance of the handler for Uri.</returns>
        public IProtocolHandler GetHandler(Uri uri)
        {
            if (this.handlers.ContainsKey(uri.Scheme))
            {
                var type = this.handlers[uri.Scheme.ToLower()];
                return (IProtocolHandler) Activator.CreateInstance(type);
            }

            this.Logger.LogError(Resources.EWI001, uri.Scheme);
            return default(IProtocolHandler);
        }

        private void RegisterHandler(IProtocolHandler handler)
        {
            var type = handler.GetType();
            foreach (var scheme in handler.Scheme)
            {
                if (!this.handlers.ContainsKey(scheme))
                {
                    this.handlers.Add(scheme, type);
                }
            }
        }
    }
}