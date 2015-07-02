using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common
{
    /// <summary>
    /// Contains functionality related to HTTP operations
    /// </summary>
    [CakeAliasCategory("HTTP Operations")]
    public static class HttpAliases
    {
        /// <summary>
        /// Downloads the specified File over HTTP to the specified local output path
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="url">URL of file to download.</param>
        /// <param name="outputPath">Where to download the file to.</param>
        [CakeMethodAlias]
        public static void DownloadFile(this ICakeContext context, string url, FilePath outputPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            if (outputPath == null)
            {
                throw new ArgumentNullException("outputPath");
            }

            context.Log.Write(Verbosity.Verbose, LogLevel.Information, "Downloading File: {0}", url);

            // We track the last posted value since the event seems to fire many times for the same value
            var percentComplete = 0;

            using (var http = new System.Net.WebClient())
            {
                http.DownloadProgressChanged += (sender, e) =>
                {
                    // Only write to log if the value changed and only ever 5%
                    if (percentComplete != e.ProgressPercentage && e.ProgressPercentage % 5 == 0)
                    {
                        percentComplete = e.ProgressPercentage;
                        context.Log.Write(Verbosity.Verbose, LogLevel.Information, "Downloading File: {0}%", e.ProgressPercentage);
                    }
                };

                // Download file async so we get the progress events, but block for it to complete anyway
                http.DownloadFileTaskAsync(url, outputPath.FullPath).Wait();
            }

            context.Log.Write(Verbosity.Verbose, LogLevel.Information, "Download Complete, saved to: {0}", outputPath.FullPath);
        }
    }
}
