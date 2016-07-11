// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.SetApiKey
{
    /// <summary>
    /// The NuGet set API key used to set API key used for API/feed authentication.
    /// </summary>
    public sealed class NuGetSetApiKey : NuGetTool<NuGetSetApiKeySettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetSetApiKey"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The NuGet tool resolver.</param>
        public NuGetSetApiKey(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            INuGetToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
            _environment = environment;
        }

        /// <summary>
        /// Installs NuGet packages using the specified package id and settings.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="source">The Server URL where the API key is valid.</param>
        /// <param name="settings">The settings.</param>
        public void SetApiKey(string apiKey, string source, NuGetSetApiKeySettings settings)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException("apiKey");
            }

            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException("source");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            string output = null;
            var processSettings = new ProcessSettings
            {
                Arguments = GetArguments(apiKey, source, settings),
                RedirectStandardOutput = true
            };
            Run(settings, null, processSettings, process => output = string.Join("\r\n", process.GetStandardOutput()));

            if (string.IsNullOrWhiteSpace(output) ||
                !output.Contains(string.Concat("The API Key '", apiKey, "' was saved for '", source, "'.")))
            {
                throw new CakeException("SetApiKey returned unexpected response.");
            }
        }

        private ProcessArgumentBuilder GetArguments(string apiKey, string source, NuGetSetApiKeySettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("setapikey");
            builder.AppendQuotedSecret(apiKey);

            // Source
            builder.Append("-Source");
            builder.AppendQuoted(source);

            // Verbosity?
            if (settings.Verbosity.HasValue)
            {
                builder.Append("-Verbosity");
                builder.Append(settings.Verbosity.Value.ToString().ToLowerInvariant());
            }

            // Configuration file
            if (settings.ConfigFile != null)
            {
                builder.Append("-ConfigFile");
                builder.AppendQuoted(settings.ConfigFile.MakeAbsolute(_environment).FullPath);
            }

            builder.Append("-NonInteractive");

            return builder;
        }
    }
}
