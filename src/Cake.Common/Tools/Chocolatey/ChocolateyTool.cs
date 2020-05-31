// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey
{
    /// <summary>
    /// Base class for all Chocolatey related tools.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    public abstract class ChocolateyTool<TSettings> : Tool<TSettings>
        where TSettings : ToolSettings
    {
        private readonly IChocolateyToolResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyTool{TSettings}"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        protected ChocolateyTool(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            IChocolateyToolResolver resolver) : base(fileSystem, environment, processRunner, tools)
        {
            _resolver = resolver;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected sealed override string GetToolName()
        {
            return "Chocolatey";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected sealed override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "Choco.exe", "choco.exe" };
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected sealed override IEnumerable<FilePath> GetAlternativeToolPaths(TSettings settings)
        {
            var path = _resolver.ResolvePath();
            if (path != null)
            {
                return new[] { path };
            }
            return Enumerable.Empty<FilePath>();
        }

        /// <summary>
        /// Adds common arguments to the process builder.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="builder">The process argument builder.</param>
        /// <returns>The process argument builder.</returns>
        protected ProcessArgumentBuilder AddCommonArguments(ChocolateySettings settings, ProcessArgumentBuilder builder)
        {
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

            // Accept License
            if (settings.AcceptLicense)
            {
                builder.Append("--acceptLicense");
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

            // Package source
            if (!string.IsNullOrWhiteSpace(settings.Source))
            {
                builder.Append("-s");
                builder.AppendQuoted(settings.Source);
            }

            // Version
            if (settings.Version != null)
            {
                builder.Append("--version");
                builder.AppendQuoted(settings.Version);
            }

            // OverrideArguments
            if (settings.OverrideArguments)
            {
                builder.Append("-o");
            }

            // NotSilent
            if (settings.NotSilent)
            {
                builder.Append("--notSilent");
            }

            // Package Parameters
            if (!string.IsNullOrWhiteSpace(settings.PackageParameters))
            {
                builder.Append("--params");
                builder.AppendQuoted(settings.PackageParameters);
            }

            // Side by side installation
            if (settings.SideBySide)
            {
                builder.Append("-m");
            }

            // Skip PowerShell
            if (settings.SkipPowerShell)
            {
                builder.Append("-n");
            }

            return builder;
        }
    }
}