namespace Extensibility
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using System.Linq;
    using Downloader;
    using Microsoft.Extensions.Logging;

    [Export(typeof(IProtocolHandlerFactory))]
    public class ProtocolHandlerFactory : IProtocolHandlerFactory
    {
        private readonly Dictionary<string, Type> handlers;

        private readonly ILogger Logger = AppLogger.CreateLogger<ProtocolHandlerFactory>();

        [ImportingConstructor]
        public ProtocolHandlerFactory([ImportMany] IEnumerable<IProtocolHandler> handlers)
        {
            this.handlers = handlers.ToDictionary(handler => handler.Scheme.ToLower(), handler => handler.GetType());
        }

        public IProtocolHandler GetHandler(Uri uri)
        {
            if (this.handlers.ContainsKey(uri.Scheme))
            {
                var type = this.handlers[uri.Scheme.ToLower()];
                return (IProtocolHandler)Activator.CreateInstance(type);
            }

            this.Logger.LogError(Resources.EWI001, uri.Scheme);
            return default(IProtocolHandler);
        }
    }
}
