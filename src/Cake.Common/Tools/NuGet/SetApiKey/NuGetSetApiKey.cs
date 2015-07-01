using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.NuGet.SetApiKey
{
    /// <summary>
    /// The NuGet set API key used to set API key used for API/feed authentication.
    /// </summary>
    public sealed class NuGetSetApiKey : Tool<NuGetSetApiKeySettings>
    {
        private readonly IToolResolver _nugetToolResolver;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetSetApiKey"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="nugetToolResolver">The NuGet tool resolver.</param>
        public NuGetSetApiKey(ICakeLog log, IFileSystem fileSystem, ICakeEnvironment environment, 
            IProcessRunner processRunner, IGlobber globber, IToolResolver nugetToolResolver)
            : base(fileSystem, environment, processRunner, globber)
        {
            _nugetToolResolver = nugetToolResolver;
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
            Run(settings, null, settings.ToolPath, processSettings, process => output = string.Join("\r\n", process.GetStandardOutput()));

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

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return _nugetToolResolver.Name;
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "NuGet.exe", "nuget.exe" };
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(NuGetSetApiKeySettings settings)
        {
            var path = _nugetToolResolver.ResolveToolPath();
            if (path != null)
            {
                return new[] { path };
            }

            return Enumerable.Empty<FilePath>();
        }
    }
}
