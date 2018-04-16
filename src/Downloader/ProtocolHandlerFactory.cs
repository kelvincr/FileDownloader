namespace Extensibility
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using Downloader;
    using Microsoft.Extensions.Logging;

    [Export(typeof(IProtocolHandlerFactory))]
    public class ProtocolHandlerFactory : IProtocolHandlerFactory
    {
        private readonly Dictionary<string, Type> handlers = new Dictionary<string, Type>();

        private readonly ILogger Logger = AppLogger.CreateLogger<ProtocolHandlerFactory>();

        [ImportingConstructor]
        public ProtocolHandlerFactory([ImportMany] IEnumerable<IProtocolHandler> handlers)
        {
            foreach (var handler in handlers)
            {
                this.RegisterHandler(handler);
            }
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
