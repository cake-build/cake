using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.Chocolatey.ApiKey
{
    /// <summary>
    /// The Chocolatey package pinner used to pin Chocolatey packages.
    /// </summary>
    public sealed class ChocolateyApiKeySetter : ChocolateyTool<ChocolateyApiKeySettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyApiKeySetter"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        public ChocolateyApiKeySetter(IFileSystem fileSystem, ICakeEnvironment environment,
            IProcessRunner processRunner, IGlobber globber, IChocolateyToolResolver resolver)
            : base(fileSystem, environment, processRunner, globber, resolver)
        {
        }

        /// <summary>
        /// Pins Chocolatey packages using the specified package id and settings.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="source">The Server URL where the API key is valid.</param>
        /// <param name="settings">The settings.</param>
        public void Set(string apiKey, string source, ChocolateyApiKeySettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException("apiKey");
            }

            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException("source");
            }

            Run(settings, GetArguments(apiKey, source, settings));
        }

        private ProcessArgumentBuilder GetArguments(string apiKey, string source, ChocolateyApiKeySettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("apikey");

            builder.Append("-s");
            builder.AppendQuoted(source);

            builder.Append("-k");
            builder.AppendQuoted(apiKey);

            // Debug
            if (settings.Debug)
            {
                builder.Append("-d");
            }

            // Verbose
            if (settings.Verbose)
            {
                builder.Append("-v");
            }

            // Always say yes, so as to not show interactive prompt
            builder.Append("-y");

            // Force
            if (settings.Force)
            {
                builder.Append("-f");
            }

            // Noop
            if (settings.Noop)
            {
                builder.Append("--noop");
            }

            // Limit Output
            if (settings.LimitOutput)
            {
                builder.Append("-r");
            }

            // Execution Timeout
            if (settings.ExecutionTimeout != 0)
            {
                builder.Append("--execution-timeout");
                builder.AppendQuoted(settings.ExecutionTimeout.ToString(CultureInfo.InvariantCulture));
            }

            // Cache Location
            if (!string.IsNullOrWhiteSpace(settings.CacheLocation))
            {
                builder.Append("-c");
                builder.AppendQuoted(settings.CacheLocation);
            }

            // Allow Unofficial
            if (settings.AllowUnofficial)
            {
                builder.Append("--allowunofficial");
            }

            return builder;
        }
    }
}