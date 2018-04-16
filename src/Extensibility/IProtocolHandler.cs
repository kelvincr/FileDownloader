using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Extensibility
{
    public interface IProtocolHandler
    {
        IEnumerable<string> Scheme { get; }

        Task<long> FetchSizeAsync(Uri uri, ICredentials credentials, CancellationToken cancellationToken);

        Task<CompletedState> DownloadAsync(Uri uri, Stream writeStream, ICredentials credentials, CancellationToken cancellationToken);

        Task<CompletedState> ContinueDownloadAsync(Uri uri, Stream writeStream, long responseOffset, ICredentials credentials, CancellationToken cancellationToken);
    }
}