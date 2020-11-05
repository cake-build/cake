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
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoCDProvider" /> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The cake log.</param>
        public GoCDProvider(ICakeEnvironment environment, ICakeLog log)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            Environment = new GoCDEnvironmentInfo(environment);
        }

        /// <inheritdoc/>
        public bool IsRunningOnGoCD => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("GO_SERVER_URL"));

        /// <inheritdoc/>
        public GoCDEnvironmentInfo Environment { get; }

        /// <inheritdoc/>
        public GoCDHistoryInfo GetHistory(string username, string password)
        {
            return GetHistory(username, password, Environment.GoCDUrl);
        }

        /// <inheritdoc/>
        public GoCDHistoryInfo GetHistory(string username, string password, string serverUrl)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            if (serverUrl == null)
            {
                throw new ArgumentNullException(nameof(serverUrl));
            }

            if (!IsRunningOnGoCD)
            {
                throw new CakeException("The current build is not running on Go.CD.");
            }

            var url = new Uri(string.Format(
                CultureInfo.InvariantCulture,
                "{0}/go/api/pipelines/{1}/history/0",
                serverUrl,
                Environment.Pipeline.Name).ToLowerInvariant());

            _log.Write(Verbosity.Diagnostic, LogLevel.Verbose, "Getting [{0}]", url);
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
                    _log.Write(Verbosity.Diagnostic, LogLevel.Verbose, "Server response [{0}:{1}]:\n\r{2}", response.StatusCode, response.ReasonPhrase, content);

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
