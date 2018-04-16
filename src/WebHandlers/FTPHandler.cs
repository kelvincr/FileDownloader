namespace WebHandlers
{
    using System;
    using System.Composition;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensibility;

    [Export(typeof(IProtocolHandler))]
    public class FtpHandler : BaseHandler, IProtocolHandler
    {
        public string Scheme => "ftp";

        public async Task<long> FetchSizeAsync(Uri uri, ICredentials credentials, CancellationToken cancellationToken)
        {
            try
            {
                var request = (FtpWebRequest)WebRequest.Create(uri);
                request.Credentials = credentials;
                request.Method = WebRequestMethods.Ftp.GetFileSize;
                var response = await request.GetResponseAsync();
                return response.ContentLength;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<CompletedState> DownloadAsync(Uri uri, Stream writeStream, ICredentials credentials, CancellationToken cancellationToken)
        {
            return await this.ContinueDownloadAsync(uri, writeStream, 0, credentials, cancellationToken);
        }

        public async Task<CompletedState> ContinueDownloadAsync(Uri uri, Stream writeStream, long responseOffset, ICredentials credentials, CancellationToken cancellationToken)
        {
            var request = (FtpWebRequest)WebRequest.Create(uri);
            request.Credentials = credentials;
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            return await this.GetResponseAsync(writeStream, request, responseOffset, cancellationToken);
        }

 
    }
}
