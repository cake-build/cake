using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;

namespace Cake.Common.Tools.NuGet.SetProxy
{
    /// <summary>
    /// The NuGet set command used to set the proxy settings to be used while connecting to your NuGet feed.
    /// </summary>
    public sealed class NuGetSetProxy : NuGetTool<NuGetSetProxySettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetSetProxy"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="resolver">The NuGet tool resolver.</param>
        public NuGetSetProxy(IFileSystem fileSystem, ICakeEnvironment environment,
            IProcessRunner processRunner, IGlobber globber, INuGetToolResolver resolver)
            : base(fileSystem, environment, processRunner, globber, resolver)
        {
            _environment = environment;
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
    }
}
