// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cake.Common.Net
{
    internal static class HttpClientExtensions
    {
        private const int DefaultBufferSize = 4096;

        public static async Task DownloadFileAsync(this HttpClient client, Uri requestUri, string path, IProgress<int> progress = null)
        {
            var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            long? contentLength = null;

            if (progress != null && response.Content.Headers.ContentLength.HasValue)
            {
                contentLength = response.Content.Headers.ContentLength.Value;
            }

            using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            using (var fileStream = File.Create(path, DefaultBufferSize))
            {
                int bytesRead;
                var totalBytesRead = 0L;
                var buffer = new byte[DefaultBufferSize];

                while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);

                    totalBytesRead += bytesRead;

                    if (contentLength.HasValue)
                    {
                        var progressPercentage = totalBytesRead * 1d / (contentLength.Value * 1d);

                        progress.Report((int)(progressPercentage * 100));
                    }
                }
            }
        }
    }
}
