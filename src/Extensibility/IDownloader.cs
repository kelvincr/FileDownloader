using System;
using System.Threading;
using System.Threading.Tasks;

namespace Extensibility
{
    using System.Collections.Generic;

    public interface IDownloader
    {
        Task DownloadAsync(IEnumerable<Uri> uris, CancellationToken tk);
    }
}