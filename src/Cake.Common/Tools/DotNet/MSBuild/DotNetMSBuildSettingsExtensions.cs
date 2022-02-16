// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Common.Tools.MSBuild;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.MSBuild
{
    /// <summary>
    /// Contains functionality related to .NET MSBuild settings.
    /// </summary>
    public static class DotNetMSBuildSettingsExtensions
    {
        /// <summary>
        /// Adds a MSBuild target to the configuration.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="target">The MSBuild target.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>Ignores a target if already added.</remarks>
        public static DotNetMSBuildSettings WithTarget(this DotNetMSBuildSettings settings, string target)
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
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings WithProperty(this DotNetMSBuildSettings settings, string name, params string[] values)
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
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings ShowDetailedSummary(this DotNetMSBuildSettings settings)
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
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings WithIgnoredProjectExtension(this DotNetMSBuildSettings settings, string extension)
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
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings SetMaxCpuCount(this DotNetMSBuildSettings settings, int? maxCpuCount)
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
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings ExcludeAutoResponseFiles(this DotNetMSBuildSettings settings)
        {
            EnsureSettings(settings);

            settings.ExcludeAutoResponseFiles = true;
            return settings;
        }

        /// <summary>
        /// Hide the startup banner and the copyright message.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings HideLogo(this DotNetMSBuildSettings settings)
        {
            EnsureSettings(settings);

            settings.NoLogo = true;
            return settings;
        }

        /// <summary>
        /// Sets a value indicating whether to normalize stored file paths used when producing deterministic builds.
        /// </summary>
        /// <remarks>
        /// For more information see https://devblogs.microsoft.com/dotnet/producing-packages-with-source-link/#deterministic-builds.
        /// </remarks>
        /// <param name="settings">The settings.</param>
        /// <param name="continuousIntegrationBuild">A value indicating whether to normalize stored file paths used when producing deterministic builds.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings SetContinuousIntegrationBuild(this DotNetMSBuildSettings settings, bool? continuousIntegrationBuild = true)
        {
            EnsureSettings(settings);

            settings.ContinuousIntegrationBuild = continuousIntegrationBuild;
            return settings;
        }

        /// <summary>
        /// Sets the version of the Toolset to use to build the project.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="version">The version.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings UseToolVersion(this DotNetMSBuildSettings settings, MSBuildVersion version)
        {
            EnsureSettings(settings);

            settings.ToolVersion = version;
            return settings;
        }

        /// <summary>
        /// Validate the project file and, if validation succeeds, build the project.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings ValidateProjectFile(this DotNetMSBuildSettings settings)
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
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// A response file is a text file that is used to insert command-line switches. For more information see https://docs.microsoft.com/en-gb/visualstudio/msbuild/msbuild-response-files.
        /// </remarks>
        public static DotNetMSBuildSettings WithResponseFile(this DotNetMSBuildSettings settings, FilePath responseFile)
        {
            EnsureSettings(settings);

            settings.ResponseFiles.Add(responseFile);
            return settings;
        }

        /// <summary>
        /// Log the build output of each MSBuild node to its own file.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings UseDistributedFileLogger(this DotNetMSBuildSettings settings)
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
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// A distributed logger consists of a central and forwarding logger. MSBuild will attach an instance of the forwarding logger to each secondary node.
        /// For more information see https://msdn.microsoft.com/en-us/library/bb383987.aspx.
        /// </remarks>
        public static DotNetMSBuildSettings WithDistributedLogger(this DotNetMSBuildSettings settings, MSBuildDistributedLogger logger)
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
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings SetConsoleLoggerSettings(this DotNetMSBuildSettings settings, MSBuildLoggerSettings consoleLoggerParameters)
        {
            EnsureSettings(settings);

            settings.ConsoleLoggerSettings = consoleLoggerParameters ?? throw new ArgumentNullException(nameof(consoleLoggerParameters));

            return settings;
        }

        /// <summary>
        /// Adds a file logger with all the default settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// Each file logger will be declared in the order added.
        /// The first file logger will match up to the /fl parameter.
        /// The next nine (max) file loggers will match up to the /fl1 through /fl9 respectively.
        /// </remarks>
        public static DotNetMSBuildSettings AddFileLogger(this DotNetMSBuildSettings settings)
        {
            return AddFileLogger(settings, new MSBuildFileLoggerSettings());
        }

        /// <summary>
        /// Adds a file logger.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="fileLoggerParameters">Parameters to be passed to the logger.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// Each file logger will be declared in the order added.
        /// The first file logger will match up to the /fl parameter.
        /// The next nine (max) file loggers will match up to the /fl1 through /fl9 respectively.
        /// </remarks>
        public static DotNetMSBuildSettings AddFileLogger(this DotNetMSBuildSettings settings, MSBuildFileLoggerSettings fileLoggerParameters)
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
        /// Enables the binary logger with all the default settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings EnableBinaryLogger(this DotNetMSBuildSettings settings)
        {
            return EnableBinaryLogger(settings, MSBuildBinaryLoggerImports.Unspecified);
        }

        /// <summary>
        /// Enables the binary logger with the specified imports and default file name.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="imports">The imports.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings EnableBinaryLogger(this DotNetMSBuildSettings settings, MSBuildBinaryLoggerImports imports)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.BinaryLogger = new MSBuildBinaryLoggerSettings
            {
                Enabled = true,
                Imports = imports,
            };

            return settings;
        }

        /// <summary>
        /// Enables the binary logger with the specified log file name and no imports.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="fileName">The log file name.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings EnableBinaryLogger(this DotNetMSBuildSettings settings, string fileName)
        {
            return EnableBinaryLogger(settings, fileName, MSBuildBinaryLoggerImports.Unspecified);
        }

        /// <summary>
        /// Enables the binary logger with the specified log file name and imports.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="fileName">The log file name.</param>
        /// <param name="imports">The imports.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings EnableBinaryLogger(this DotNetMSBuildSettings settings, string fileName, MSBuildBinaryLoggerImports imports)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.BinaryLogger = new MSBuildBinaryLoggerSettings
            {
                Enabled = true,
                FileName = fileName,
                Imports = imports,
            };

            return settings;
        }

        /// <summary>
        /// Adds a custom logger.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="loggerAssembly">The assembly containing the logger. Should match the format {AssemblyName[,StrongName] | AssemblyFile}.</param>
        /// <param name="loggerClass">The class implementing the logger. Should match the format [PartialOrFullNamespace.]LoggerClassName. If the assembly contains only one logger, class does not need to be specified.</param>
        /// <param name="loggerParameters">Parameters to be passed to the logger.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings WithLogger(this DotNetMSBuildSettings settings, string loggerAssembly, string loggerClass = null, string loggerParameters = null)
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
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings DisableConsoleLogger(this DotNetMSBuildSettings settings)
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
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// When a warning is treated as an error the target will continue to execute as if it was a warning but the overall build will fail.
        /// </remarks>
        public static DotNetMSBuildSettings SetWarningCodeAsError(this DotNetMSBuildSettings settings, string warningCode)
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
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings SetWarningCodeAsMessage(this DotNetMSBuildSettings settings, string warningCode)
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
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings TreatAllWarningsAs(this DotNetMSBuildSettings settings, MSBuildTreatAllWarningsAs behaviour)
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
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings SetConfiguration(this DotNetMSBuildSettings settings, string configuration)
            => settings.WithProperty("configuration", configuration);

        /// <summary>
        /// Sets the version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="version">The version.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// Version will override VersionPrefix and VersionSuffix if set.
        /// This may also override version settings during packaging.
        /// </remarks>
        public static DotNetMSBuildSettings SetVersion(this DotNetMSBuildSettings settings, string version)
            => settings.WithProperty("Version", version);

        /// <summary>
        /// Sets the file version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="fileVersion">The file version.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings SetFileVersion(this DotNetMSBuildSettings settings, string fileVersion)
            => settings.WithProperty("FileVersion", fileVersion);

        /// <summary>
        /// Sets the assembly version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="assemblyVersion">The assembly version.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings SetAssemblyVersion(this DotNetMSBuildSettings settings, string assemblyVersion)
            => settings.WithProperty("AssemblyVersion", assemblyVersion);

        /// <summary>
        /// Sets the informational version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="informationalVersion">The informational version.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings SetInformationalVersion(this DotNetMSBuildSettings settings, string informationalVersion)
            => settings.WithProperty("InformationalVersion", informationalVersion);

        /// <summary>
        /// Sets the package version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="packageVersion">The package version.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings SetPackageVersion(this DotNetMSBuildSettings settings, string packageVersion)
            => settings.WithProperty("PackageVersion", packageVersion);

        /// <summary>
        /// Sets the package release notes.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="packageReleaseNotes">The package release notes.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings SetPackageReleaseNotes(this DotNetMSBuildSettings settings, string packageReleaseNotes)
            => settings.WithProperty("PackageReleaseNotes", packageReleaseNotes);

        /// <summary>
        /// Suppress warning CS7035.
        /// This is useful when using semantic versioning and either the file or informational version
        /// doesn't match the recommended format.
        /// The recommended format is: major.minor.build.revision where
        /// each is an integer between 0 and 65534 (inclusive).
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings SuppressVersionRecommendedFormatWarning(this DotNetMSBuildSettings settings)
            => settings.WithProperty("nowarn", "7035");

        /// <summary>
        /// Sets the version prefix.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="versionPrefix">The version prefix.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings SetVersionPrefix(this DotNetMSBuildSettings settings, string versionPrefix)
            => settings.WithProperty("VersionPrefix", versionPrefix);

        /// <summary>
        /// Sets the version Suffix.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="versionSuffix">The version prefix.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetMSBuildSettings SetVersionSuffix(this DotNetMSBuildSettings settings, string versionSuffix)
            => settings.WithProperty("VersionSuffix", versionSuffix);

        /// <summary>
        /// Adds a framework to target.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="targetFramework">The framework to target.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// For list of target frameworks see https://docs.microsoft.com/en-us/dotnet/standard/frameworks.
        /// </remarks>
        public static DotNetMSBuildSettings SetTargetFramework(this DotNetMSBuildSettings settings, string targetFramework)
            => settings.WithProperty("TargetFrameworks", targetFramework);

        /// <summary>
        /// Sets a target operating systems where the application or assembly will run.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="runtimeId">The runtime id of the operating system.</param>
        /// <returns>The same <see cref="DotNetMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// For list of runtime ids see https://docs.microsoft.com/en-us/dotnet/core/rid-catalog.
        /// </remarks>
        public static DotNetMSBuildSettings SetRuntime(this DotNetMSBuildSettings settings, string runtimeId)
            => settings.WithProperty("RuntimeIdentifiers", runtimeId);

        private static void EnsureSettings(DotNetMSBuildSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
        }
    }
}
