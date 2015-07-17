using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.NuGet.SetProxy
{
    /// <summary>
    /// The NuGet set command used to set the proxy settings to be used while connecting to your NuGet feed.
    /// </summary>
    public sealed class NuGetSetProxy : Tool<NuGetSetProxySettings>
    {
        private readonly ICakeLog _log;
        private readonly ICakeEnvironment _environment;
        private readonly IToolResolver _nugetToolResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetSetProxy"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="nugetToolResolver">The NuGet tool resolver.</param>
        public NuGetSetProxy(ICakeLog log, IFileSystem fileSystem, ICakeEnvironment environment,
            IProcessRunner processRunner, IGlobber globber, IToolResolver nugetToolResolver)
            : base(fileSystem, environment, processRunner, globber)
        {
            _log = log;
            _environment = environment;
            _nugetToolResolver = nugetToolResolver;
        }

        /// <summary>
        /// Set the proxy settings to be used while connecting to your NuGet feed.
        /// </summary>
        /// <param name="url">The url of the proxy.</param>
        /// <param name="username">The username used to access the proxy.</param>
        /// <param name="password">The password used to access the proxy.</param>
        /// <param name="settings">The settings.</param>
        public void SetProxy(string url, string username, string password, NuGetSetProxySettings settings)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            string output = null;
            var processSettings = new ProcessSettings
            {
                Arguments = GetArguments(url, username, password, settings),
                RedirectStandardOutput = true
            };
            Run(settings, null, settings.ToolPath, processSettings, process => output = string.Join("\r\n", process.GetStandardOutput()));

            if (!string.IsNullOrWhiteSpace(output))
            {
                throw new CakeException("Set command returned unexpected response.");
            }
        }

        private ProcessArgumentBuilder GetArguments(string url, string username, string password, NuGetSetProxySettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("config");

            // Source
            builder.Append("-Set http_proxy=" + url);

            if (!string.IsNullOrEmpty(username))
            {
                builder.Append("-Set http_proxy.user=" + username);
            }

            if (!string.IsNullOrEmpty(password))
            {
                builder.AppendSecret("-Set http_proxy.password=" + password);
            }

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
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(NuGetSetProxySettings settings)
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
