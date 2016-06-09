// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Net.Http;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Net
{
    /// <summary>
    /// Contains functionality related to HTTP operations
    /// </summary>
    [CakeAliasCategory("HTTP Operations")]
    public static class HttpAliases
    {
        /// <summary>
        /// Downloads the specified resource over HTTP to a temporary file.
        /// </summary>
        /// <example>
        /// <code>
        /// var resource = DownloadFile("http://www.example.org/index.html");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="address">The URL of the resource to download.</param>
        /// <returns>The path to the downloaded file.</returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Download")]
        public static FilePath DownloadFile(this ICakeContext context, string address)
        {
            return DownloadFile(context, new Uri(address));
        }

        /// <summary>
        /// Downloads the specified resource over HTTP to a temporary file.
        /// </summary>
        /// <example>
        /// <code>
        /// var address = new Uri("http://www.example.org/index.html");
        /// var resource = DownloadFile(address);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="address">The URL of file to download.</param>
        /// <returns>The path to the downloaded file.</returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Download")]
        public static FilePath DownloadFile(this ICakeContext context, Uri address)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var tempFolder = context.Environment.GetSpecialPath(SpecialPath.LocalTemp);
            var tempFilename = tempFolder.CombineWithFilePath(new FilePath(System.IO.Path.GetRandomFileName()));
            DownloadFile(context, address, tempFilename);
            return tempFilename;
        }

        /// <summary>
        /// Downloads the specified resource over HTTP to the specified output path.
        /// </summary>
        /// <example>
        /// <code>
        /// DownloadFile("http://www.example.org/index.html", "./outputdir");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="address">The URL of the resource to download.</param>
        /// <param name="outputPath">The output path.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Download")]
        public static void DownloadFile(this ICakeContext context, string address, FilePath outputPath)
        {
            DownloadFile(context, new Uri(address), outputPath);
        }

        /// <summary>
        /// Downloads the specified resource over HTTP to the specified output path.
        /// </summary>
        /// <example>
        /// <code>
        /// var address = new Uri("http://www.example.org/index.html");
        /// DownloadFile(address, "./outputdir");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="address">The URL of the resource to download.</param>
        /// <param name="outputPath">The output path.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Download")]
        public static void DownloadFile(this ICakeContext context, Uri address, FilePath outputPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            if (outputPath == null)
            {
                throw new ArgumentNullException("outputPath");
            }

            context.Log.Verbose("Downloading file: {0}", address);

            // We track the last posted value since the event seems to fire many times for the same value.
            var percentComplete = 0;

            using (var http = new HttpClient())
            {
                var progress = new Progress<int>(progressPercentage =>
                {
                    // Only write to log if the value changed and only ever 5%.
                    if (percentComplete != progressPercentage && progressPercentage % 5 == 0)
                    {
                        percentComplete = progressPercentage;
                        context.Log.Verbose("Downloading file: {0}%", progressPercentage);
                    }
                });

                // Download file async so we get the progress events, but block for it to complete anyway.
                http.DownloadFileAsync(address, outputPath.FullPath, progress).Wait();
            }

            context.Log.Verbose("Download complete, saved to: {0}", outputPath.FullPath);
        }
    }
}
