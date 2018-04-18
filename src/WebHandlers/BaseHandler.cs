// <copyright file="BaseHandler.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WebHandlers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensibility;

    /// <summary>
    /// Base Helper handler.
    /// </summary>
    public class BaseHandler
    {
        /// <summary>
        /// Gets the response asynchronous.
        /// </summary>
        /// <param name="writeStream">The write stream.</param>
        /// <param name="request">The request.</param>
        /// <param name="responseOffset">The response offset.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected async Task<CompletedState> GetResponseAsync(
            Stream writeStream, WebRequest request, long responseOffset, CancellationToken cancellationToken)
        {
            var result = CompletedState.NonStarted;
            try
            {
                using (var responseFileDownload = request.GetResponse())
                using (var responseStream = responseFileDownload.GetResponseStream())
                {
                    responseStream.Seek(responseOffset, SeekOrigin.Begin);
                    const int length = 2048;
                    var buffer = new byte[length];
                    var bytesRead = responseStream.Read(buffer, 0, length);
                    var bytes = 0;

                    while (bytesRead > 0)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            result = CompletedState.Canceled;
                            break;
                        }

                        await writeStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                        bytesRead = await responseStream.ReadAsync(buffer, 0, length, cancellationToken);
                        bytes += bytesRead;
                        result = CompletedState.Partial;
                    }

                    result = result != CompletedState.Canceled ? CompletedState.Succeeded : result;
                }
            }
            catch (Exception)
            {
                result = CompletedState.Failed;
            }

            return result;
        }
    }
}