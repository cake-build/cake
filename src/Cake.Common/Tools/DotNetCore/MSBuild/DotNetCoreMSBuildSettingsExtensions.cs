// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Common.Tools.MSBuild;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNetCore.MSBuild
{
    /// <summary>
    /// Contains functionality related to .NET Core MSBuild settings.
    /// </summary>
    public static class DotNetCoreMSBuildSettingsExtensions
    {
        /// <summary>
        /// Adds a MSBuild target to the configuration.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="target">The MSBuild target.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>Ignores a target if already added.</remarks>
        public static DotNetCoreMSBuildSettings WithTarget(this DotNetCoreMSBuildSettings settings, string target)
        {
            EnsureSettings(settings);

            if (!settings.Targets.Contains(target))
            {
                settings.Targets.Add(target);
            }

            return settings;
        }

        /// <summary>
        /// Adds a property to the configuration.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="name">The property name.</param>
        /// <param name="values">The property values.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings WithProperty(this DotNetCoreMSBuildSettings settings, string name, params string[] values)
        {
            EnsureSettings(settings);

            ICollection<string> currentValue;

            // try to get existing values of properties and add new property values
            currentValue = settings.Properties.TryGetValue(name, out currentValue) && currentValue != null
                ? currentValue.Concat(values).ToList()
                : new List<string>(values);

            settings.Properties[name] = currentValue;

            return settings;
        }

        /// <summary>
        /// Shows detailed information at the end of the build log about the configurations that were built and how they were scheduled to nodes.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings ShowDetailedSummary(this DotNetCoreMSBuildSettings settings)
        {
            EnsureSettings(settings);

            settings.DetailedSummary = true;
            return settings;
        }

        /// <summary>
        /// Adds a extension to ignore when determining which project file to build.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="extension">The extension to ignore.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings WithIgnoredProjectExtension(this DotNetCoreMSBuildSettings settings, string extension)
        {
            EnsureSettings(settings);

            settings.IgnoreProjectExtensions.Add(extension);
            return settings;
        }

        /// <summary>
        /// Sets the maximum CPU count. Without this set MSBuild will compile projects in this solution one at a time.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="maxCpuCount">The maximum CPU count. Set this value to zero to use as many MSBuild processes as available CPUs.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetMaxCpuCount(this DotNetCoreMSBuildSettings settings, int? maxCpuCount)
        {
            EnsureSettings(settings);

            settings.MaxCpuCount = maxCpuCount.HasValue
                                    ? Math.Max(0, maxCpuCount.Value)
                                    : 0;
            return settings;
        }

        /// <summary>
        /// Exclude any MSBuild.rsp files automatically.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings ExcludeAutoResponseFiles(this DotNetCoreMSBuildSettings settings)
        {
            EnsureSettings(settings);

            settings.ExcludeAutoResponseFiles = true;
            return settings;
        }

        /// <summary>
        /// Hide the startup banner and the copyright message.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings HideLogo(this DotNetCoreMSBuildSettings settings)
        {
            EnsureSettings(settings);

            settings.NoLogo = true;
            return settings;
        }

        /// <summary>
        /// Sets the version of the Toolset to use to build the project.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="version">The version.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings UseToolVersion(this DotNetCoreMSBuildSettings settings, MSBuildVersion version)
        {
            EnsureSettings(settings);

            settings.ToolVersion = version;
            return settings;
        }

        /// <summary>
        /// Validate the project file and, if validation succeeds, build the project.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings ValidateProjectFile(this DotNetCoreMSBuildSettings settings)
        {
            EnsureSettings(settings);

            settings.ValidateProjectFile = true;
            return settings;
        }

        /// <summary>
        /// Adds a response file to use.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="responseFile">The response file to add.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// A response file is a text file that is used to insert command-line switches. For more information see https://docs.microsoft.com/en-gb/visualstudio/msbuild/msbuild-response-files
        /// </remarks>
        public static DotNetCoreMSBuildSettings WithResponseFile(this DotNetCoreMSBuildSettings settings, FilePath responseFile)
        {
            EnsureSettings(settings);

            settings.ResponseFiles.Add(responseFile);
            return settings;
        }

        /// <summary>
        /// Log the build output of each MSBuild node to its own file.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings UseDistributedFileLogger(this DotNetCoreMSBuildSettings settings)
        {
            EnsureSettings(settings);

            settings.DistributedFileLogger = true;
            return settings;
        }

        /// <summary>
        /// Adds a distributed loggers to use.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="logger">The response file to add.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// A distributed logger consists of a central and forwarding logger. MSBuild will attach an instance of the forwarding logger to each secondary node.
        /// For more information see https://msdn.microsoft.com/en-us/library/bb383987.aspx
        /// </remarks>
        public static DotNetCoreMSBuildSettings WithDistributedLogger(this DotNetCoreMSBuildSettings settings, MSBuildDistributedLogger logger)
        {
            EnsureSettings(settings);

            settings.DistributedLoggers.Add(logger);
            return settings;
        }

        /// <summary>
        /// Sets the parameters for the console logger.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="consoleLoggerParameters">The console logger parameters to set.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetConsoleLoggerSettings(this DotNetCoreMSBuildSettings settings, MSBuildLoggerSettings consoleLoggerParameters)
        {
            EnsureSettings(settings);

            settings.ConsoleLoggerSettings = consoleLoggerParameters ?? throw new ArgumentNullException(nameof(consoleLoggerParameters));

            return settings;
        }

        /// <summary>
        /// Adds a file logger.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="fileLoggerParameters">Parameters to be passed to the logger.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// Each file logger will be declared in the order added.
        /// The first file logger will match up to the /fl parameter.
        /// The next nine (max) file loggers will match up to the /fl1 through /fl9 respectively.
        /// </remarks>
        public static DotNetCoreMSBuildSettings AddFileLogger(this DotNetCoreMSBuildSettings settings, MSBuildFileLoggerSettings fileLoggerParameters)
        {
            EnsureSettings(settings);

            if (fileLoggerParameters == null)
            {
                throw new ArgumentNullException(nameof(fileLoggerParameters));
            }

            settings.FileLoggers.Add(fileLoggerParameters);

            return settings;
        }

        /// <summary>
        /// Adds a file logger with all the default settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// Each file logger will be declared in the order added.
        /// The first file logger will match up to the /fl parameter.
        /// The next nine (max) file loggers will match up to the /fl1 through /fl9 respectively.
        /// </remarks>
        public static DotNetCoreMSBuildSettings AddFileLogger(this DotNetCoreMSBuildSettings settings)
        {
            EnsureSettings(settings);

            settings.FileLoggers.Add(new MSBuildFileLoggerSettings());

            return settings;
        }

        /// <summary>
        /// Adds a custom logger.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="loggerAssembly">The assembly containing the logger. Should match the format {AssemblyName[,StrongName] | AssemblyFile}</param>
        /// <param name="loggerClass">The class implementing the logger. Should match the format [PartialOrFullNamespace.]LoggerClassName. If the assembly contains only one logger, class does not need to be specified.</param>
        /// <param name="loggerParameters">Parameters to be passed to the logger.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings WithLogger(this DotNetCoreMSBuildSettings settings, string loggerAssembly, string loggerClass = null, string loggerParameters = null)
        {
            EnsureSettings(settings);

            if (string.IsNullOrWhiteSpace(loggerAssembly))
            {
                throw new ArgumentException(nameof(loggerAssembly));
            }

            settings.Loggers.Add(new MSBuildLogger
            {
                Assembly = loggerAssembly,
                Class = loggerClass,
                Parameters = loggerParameters
            });

            return settings;
        }

        /// <summary>
        /// Disables the default console logger, and not log events to the console.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings DisableConsoleLogger(this DotNetCoreMSBuildSettings settings)
        {
            EnsureSettings(settings);

            settings.DisableConsoleLogger = true;

            return settings;
        }

        /// <summary>
        /// Sets the warning code to treats as an error.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="warningCode">The warning code to treat as an error.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// When a warning is treated as an error the target will continue to execute as if it was a warning but the overall build will fail.
        /// </remarks>
        public static DotNetCoreMSBuildSettings SetWarningCodeAsError(this DotNetCoreMSBuildSettings settings, string warningCode)
        {
            EnsureSettings(settings);

            if (string.IsNullOrWhiteSpace(warningCode))
            {
                throw new ArgumentException("Warning code cannot be null or empty", nameof(warningCode));
            }

            settings.WarningCodesAsError.Add(warningCode);

            return settings;
        }

        /// <summary>
        /// Sets the warning code to treats as a message.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="warningCode">The warning code to treat as a message.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetWarningCodeAsMessage(this DotNetCoreMSBuildSettings settings, string warningCode)
        {
            EnsureSettings(settings);

            if (string.IsNullOrWhiteSpace(warningCode))
            {
                throw new ArgumentException("Warning code cannot be null or empty", nameof(warningCode));
            }

            settings.WarningCodesAsMessage.Add(warningCode);

            return settings;
        }

        /// <summary>
        /// Sets how all warnings should be treated.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="behaviour">How all warning should be treated.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings TreatAllWarningsAs(this DotNetCoreMSBuildSettings settings, MSBuildTreatAllWarningsAs behaviour)
        {
            EnsureSettings(settings);

            settings.TreatAllWarningsAs = behaviour;

            return settings;
        }

        /// <summary>
        /// Sets the configuration.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetConfiguration(this DotNetCoreMSBuildSettings settings, string configuration)
            => settings.WithProperty("configuration", configuration);

        /// <summary>
        /// Sets the version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="version">The version.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// Version will override VersionPrefix and VersionSuffix if set.
        /// This may also override version settings during packaging.
        /// </remarks>
        public static DotNetCoreMSBuildSettings SetVersion(this DotNetCoreMSBuildSettings settings, string version)
            => settings.WithProperty("Version", version);

        /// <summary>
        /// Sets the version prefix.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="versionPrefix">The version prefix.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetVersionPrefix(this DotNetCoreMSBuildSettings settings, string versionPrefix)
            => settings.WithProperty("VersionPrefix", versionPrefix);

        /// <summary>
        /// Sets the version Suffix.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="versionSuffix">The version prefix.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetVersionSuffix(this DotNetCoreMSBuildSettings settings, string versionSuffix)
            => settings.WithProperty("VersionSuffix", versionSuffix);

        /// <summary>
        /// Adds a framework to target.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="targetFramework">The framework to target.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// For list of target frameworks see https://docs.microsoft.com/en-us/dotnet/standard/frameworks.
        /// </remarks>
        public static DotNetCoreMSBuildSettings SetTargetFramework(this DotNetCoreMSBuildSettings settings, string targetFramework)
            => settings.WithProperty("TargetFrameworks", targetFramework);

        /// <summary>
        /// Sets a target operating systems where the application or assembly will run.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="runtimeId">The runtime id of the operating system.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// For list of runtime ids see https://docs.microsoft.com/en-us/dotnet/core/rid-catalog.
        /// </remarks>
        public static DotNetCoreMSBuildSettings SetRuntime(this DotNetCoreMSBuildSettings settings, string runtimeId)
            => settings.WithProperty("RuntimeIdentifiers", runtimeId);

        private static void EnsureSettings(DotNetCoreMSBuildSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
        }
    }
}
