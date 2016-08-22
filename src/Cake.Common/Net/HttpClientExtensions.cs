// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public static async Task<HttpResponseMessage> UploadFileAsync(this HttpClient client, Uri requestUri, string path, string contentType = "application/octet-stream")
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Unable to find specified file", path);
            }

            var file = new FileInfo(path);
            var fileName = file.Name;

            return await UploadFileAsync(client, requestUri, file.OpenRead(), fileName, contentType).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> UploadFileAsync(this HttpClient client, Uri requestUri, Stream stream, string fileName, string contentType = "application/octet-stream")
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream), "Stream can not be null");
            }
            if (!stream.CanRead)
            {
                throw new IOException("Unable to read from stream");
            }

            var boundary = "------------" + DateTime.Now.Ticks.ToString("x");
            var multipartFormDataContent = new MultipartFormDataContent(boundary);

            stream.Position = 0;
            var streamContent = new StreamContent(stream)
            {
                Headers = { ContentType = MediaTypeHeaderValue.Parse(contentType) }
            };

            multipartFormDataContent.Add(streamContent, fileName, fileName);
            var response = await client.PostAsync(requestUri, multipartFormDataContent).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public static async Task<HttpResponseMessage> UploadFileAsync(this HttpClient client, Uri requestUri, byte[] data, string fileName, string contentType = "application/octet-stream")
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "No data to send");
            }

            return await UploadFileAsync(client, requestUri, new MemoryStream(data), fileName, contentType).ConfigureAwait(false);
        }
    }
}