using System;
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

            using (var http = new System.Net.WebClient())
            {
                http.DownloadProgressChanged += (sender, e) =>
                {
                    // Only write to log if the value changed and only ever 5%.
                    if (percentComplete != e.ProgressPercentage && e.ProgressPercentage % 5 == 0)
                    {
                        percentComplete = e.ProgressPercentage;
                        context.Log.Verbose("Downloading file: {0}%", e.ProgressPercentage);
                    }
                };

                // Download file async so we get the progress events, but block for it to complete anyway.
                http.DownloadFileTaskAsync(address, outputPath.FullPath).Wait();
            }

            context.Log.Verbose("Download complete, saved to: {0}", outputPath.FullPath);
        }
    }
}
