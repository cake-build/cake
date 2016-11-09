// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Cake.Common.Build.GoCD.Data;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Common.Build.GoCD
{
    /// <summary>
    /// Responsible for communicating with GoCD.
    /// </summary>
    public sealed class GoCDProvider : IGoCDProvider
    {
        private readonly ICakeLog _cakeLog;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoCDProvider" /> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="cakeLog">The cake log.</param>
        public GoCDProvider(ICakeEnvironment environment, ICakeLog cakeLog)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            if (cakeLog == null)
            {
                throw new ArgumentNullException(nameof(cakeLog));
            }

            _environment = environment;
            _cakeLog = cakeLog;
            Environment = new GoCDEnvironmentInfo(environment);
        }

         /// <summary>
        /// Gets a value indicating whether the current build is running on Go.CD.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on Go.CD.; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnGoCD => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("GO_SERVER_URL"));

        /// <summary>
        /// Gets the Go.CD environment.
        /// </summary>
        /// <value>
        /// The Go.CD environment.
        /// </value>
        public GoCDEnvironmentInfo Environment { get; }

        /// <summary>
        /// Gets the Go.CD build history, including the repository modifications that caused the pipeline to start.
        /// </summary>
        /// <param name="username">The Go.CD username.</param>
        /// <param name="password">The Go.CD password.</param>
        /// <returns>The Go.CD build history.</returns>
        public GoCDHistoryInfo GetHistory(string username, string password)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (!IsRunningOnGoCD)
            {
                throw new CakeException("The current build is not running on Go.CD.");
            }

            var url = new Uri(string.Format(
                CultureInfo.InvariantCulture,
                "{0}/go/api/pipelines/{1}/history/0",
                this.Environment.GoCDUrl,
                this.Environment.Pipeline.Name).ToLowerInvariant());

            _cakeLog.Write(Verbosity.Diagnostic, LogLevel.Verbose, "Getting [{0}]", url);
            return Task.Run(async () =>
            {
                var encodedCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(
                    string.Format(CultureInfo.InvariantCulture, "{0}:{1}", username, password)));
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add(
                        "Authorization",
                        string.Format(CultureInfo.InvariantCulture, "Basic {0}", encodedCredentials));
                    var response = await client.GetAsync(url);
                    var content = await response.Content.ReadAsStringAsync();
                    _cakeLog.Write(Verbosity.Diagnostic, LogLevel.Verbose, "Server response [{0}:{1}]:\n\r{2}", response.StatusCode, response.ReasonPhrase, content);

                    var jsonSerializer = new DataContractJsonSerializer(typeof(GoCDHistoryInfo));

                    using (var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
                    {
                        return jsonSerializer.ReadObject(jsonStream) as GoCDHistoryInfo;
                    }
                }
            }).GetAwaiter().GetResult();
        }
    }
}