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
        private const string Separator = "=";

        private readonly IChocolateyToolResolver _resolver;
        private readonly ICakeEnvironment _environment;

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
            _environment = environment;
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
        protected ProcessArgumentBuilder AddGlobalArguments(ChocolateySettings settings, ProcessArgumentBuilder builder)
        {
            // Debug
            if (settings.Debug)
            {
                builder.Append("--debug");
            }

            // Verbose
            if (settings.Verbose)
            {
                builder.Append("--verbose");
            }

            // Trace
            if (settings.Trace)
            {
                builder.Append("--trace");
            }

            // NoColor
            if (settings.NoColor)
            {
                builder.Append("--no-color");
            }

            // Accept License
            if (settings.AcceptLicense)
            {
                builder.Append("--accept-license");
            }

            // Always say yes, so as to not show interactive prompt
            builder.Append("--confirm");

            // Force
            if (settings.Force)
            {
                builder.Append("--force");
            }

            // Noop
            if (settings.Noop)
            {
                builder.Append("--what-if");
            }

            // Limit Output
            if (settings.LimitOutput)
            {
                builder.Append("--limit-output");
            }

            // Execution Timeout
            if (settings.ExecutionTimeout != 0)
            {
                builder.AppendSwitchQuoted("--execution-timeout", Separator, settings.ExecutionTimeout.ToString(CultureInfo.InvariantCulture));
            }

            // Cache Location
            if (!string.IsNullOrWhiteSpace(settings.CacheLocation))
            {
                builder.AppendSwitchQuoted("--cache-location", Separator, settings.CacheLocation);
            }

            // Allow Unofficial
            if (settings.AllowUnofficial)
            {
                builder.Append("--allow-unofficial");
            }

            // Fail On Error Output
            if (settings.FailOnErrorOutput)
            {
                builder.Append("--fail-on-error-output");
            }

            // Use System PowerShell
            if (settings.UseSystemPowerShell)
            {
                builder.Append("--use-system-powershell");
            }

            // No Progress
            if (settings.NoProgress)
            {
                builder.Append("--no-progress");
            }

            // Proxy
            if (!string.IsNullOrEmpty(settings.Proxy))
            {
                builder.AppendSwitchQuoted("--proxy", Separator, settings.Proxy);
            }

            // Proxy User
            if (!string.IsNullOrEmpty(settings.ProxyUser))
            {
                builder.AppendSwitchQuoted("--proxy-user", Separator, settings.ProxyUser);
            }

            // Proxy Password
            if (!string.IsNullOrEmpty(settings.ProxyPassword))
            {
                builder.AppendSwitchQuotedSecret("--proxy-password", Separator, settings.ProxyPassword);
            }

            // Proxy By-Pass List
            if (!string.IsNullOrEmpty(settings.ProxyByPassList))
            {
                builder.AppendSwitchQuoted("--proxy-bypass-list", Separator, settings.ProxyByPassList);
            }

            // Proxy ByPass On Local
            if (settings.ProxyBypassOnLocal)
            {
                builder.Append("--proxy-bypass-on-local");
            }

            // Log File
            if (settings.LogFile != null)
            {
                var logFilePath = settings.LogFile.MakeAbsolute(_environment);
                builder.AppendSwitchQuoted("--log-file", Separator, logFilePath.FullPath);
            }

            // Skip Compatibility Checks
            if (settings.SkipCompatibilityChecks)
            {
                builder.Append("--skip-compatibility-checks");
            }

            return builder;
        }

        /// <summary>
        /// Adds shared arguments to the process builder.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="builder">The process arguments builder.</param>
        /// <returns>The process arguments builder.</returns>
        protected ProcessArgumentBuilder AddSharedArguments(ChocolateySharedSettings settings, ProcessArgumentBuilder builder)
        {
            // Source
            if (!string.IsNullOrEmpty(settings.Source))
            {
                builder.AppendSwitchQuoted("--source", Separator, settings.Source);
            }

            // Version
            if (!string.IsNullOrEmpty(settings.Version))
            {
                builder.AppendSwitchQuoted("--version", Separator, settings.Version);
            }

            // Override Arguments
            if (settings.OverrideArguments)
            {
                builder.Append("--override-arguments");
            }

            // Not Silent
            if (settings.NotSilent)
            {
                builder.Append("--not-silent");
            }

            // Package Parameters
            if (!string.IsNullOrEmpty(settings.PackageParameters))
            {
                builder.AppendSwitchQuoted("--package-parameters", Separator, settings.PackageParameters);
            }

            // Global Arguments
            if (settings.ApplyInstallArgumentsToDependencies)
            {
                builder.Append("--apply-install-arguments-to-dependencies");
            }

            // Global Package Parameters
            if (settings.ApplyPackageParametersToDependencies)
            {
                builder.Append("--apply-package-parameters-to-dependencies");
            }

            // Side By Side
            if (settings.SideBySide)
            {
                builder.Append("--side-by-side");
            }

            // Skip PowerShell
            if (settings.SkipPowerShell)
            {
                builder.Append("--skip-automation-scripts");
            }

            // Ignore Package Codes
            if (settings.IgnorePackageExitCodes)
            {
                builder.Append("--ignore-package-exit-codes");
            }

            // Use Package Exit Codes
            if (settings.UsePackageExitCodes)
            {
                builder.Append("--use-package-exit-codes");
            }

            // Stop On First Failure
            if (settings.StopOnFirstFailure)
            {
                builder.Append("--stop-on-first-package-failure");
            }

            // Exit Where Reboot Detected
            if (settings.ExitWhenRebootDetected)
            {
                builder.Append("--exit-when-reboot-detected");
            }

            // Ignore Detected Reboot
            if (settings.IgnoreDetectedReboot)
            {
                builder.Append("--ignore-detected-reboot");
            }

            // Skip Hooks
            if (settings.SkipHooks)
            {
                builder.Append("--skip-hooks");
            }

            return builder;
        }
    }
}