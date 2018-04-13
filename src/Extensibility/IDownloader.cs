namespace Extensibility
{
    using System.Collections.Generic;

    public interface IDownloader
    {
        IEnumerable<IProtocolHandler> Handlers { get; set; }
    }
}