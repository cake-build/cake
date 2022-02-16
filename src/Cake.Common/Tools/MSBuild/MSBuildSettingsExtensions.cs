// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cake.Core.Diagnostics;

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Contains functionality related to MSBuild settings.
    /// </summary>
    public static class MSBuildSettingsExtensions
    {
        /// <summary>
        /// Adds a MSBuild target to the configuration.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="target">The MSBuild target.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings WithTarget(this MSBuildSettings settings, string target)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            settings.Targets.Add(target);
            return settings;
        }

        /// <summary>
        /// Sets the tool version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="version">The version.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings UseToolVersion(this MSBuildSettings settings, MSBuildToolVersion version)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            settings.ToolVersion = version;
            return settings;
        }

        /// <summary>
        /// Sets the tool version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="version">The string version.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings UseToolVersion(this MSBuildSettings settings, string version)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            // check to see if the version is null/empty or is not a num
            if (string.IsNullOrEmpty(version))
            {
                throw new ArgumentException(nameof(version));
            }
            var sanitizeVersion = version.Replace(" ", string.Empty);
            var mSBuildToolVersion = GetMSBuildToolVersionFromString(sanitizeVersion.ToUpper());
            if (mSBuildToolVersion == MSBuildToolVersion.NETCustom || mSBuildToolVersion == MSBuildToolVersion.VSCustom)
            {
                settings.CustomVersion = version;
            }
            settings.ToolVersion = mSBuildToolVersion;
            return settings;
        }

        /// <summary>
        /// Helper that gets the MSBuildToolVersion from the version string.
        /// </summary>
        /// <param name="version">The string version.</param>
        /// <returns>The matched MSBuildToolVersion enum <see cref="MSBuildToolVersion"/>.</returns>
        private static MSBuildToolVersion GetMSBuildToolVersionFromString(string version)
        {
            switch (version)
            {
                case "2":
                case "2.0":
                case "NET20": return MSBuildToolVersion.NET20;
                case "3":
                case "3.0":
                case "NET30": return MSBuildToolVersion.NET30;
                case "3.5":
                case "NET35": return MSBuildToolVersion.NET35;
                case "4":
                case "4.0":
                case "NET40": return MSBuildToolVersion.NET40;
                case "4.5":
                case "NET45": return MSBuildToolVersion.NET45;
                case "4.5.1":
                case "NET451": return MSBuildToolVersion.NET451;
                case "4.6":
                case "NET46": return MSBuildToolVersion.NET46;
                case "4.5.2":
                case "NET452": return MSBuildToolVersion.NET452;
                case "2005":
                case "VS2005": return MSBuildToolVersion.VS2005;
                case "2008":
                case "VS2008": return MSBuildToolVersion.VS2008;
                case "2010":
                case "VS2010": return MSBuildToolVersion.VS2010;
                case "2011":
                case "VS2011": return MSBuildToolVersion.VS2011;
                case "2012":
                case "VS2012": return MSBuildToolVersion.VS2011;
                case "2013":
                case "VS2013": return MSBuildToolVersion.VS2013;
                case "2015":
                case "VS2015": return MSBuildToolVersion.VS2015;
                case "2017":
                case "VS2017": return MSBuildToolVersion.VS2017;
                case "2019":
                case "VS2019": return MSBuildToolVersion.VS2019;
                case string vs when vs.Contains("VS") || Regex.Match(vs, @"\d{4}").Success: return MSBuildToolVersion.VSCustom;
                case string dotNet when Regex.Match(dotNet, @"^[0-9,.]*$").Success: return MSBuildToolVersion.NETCustom;
                default: return MSBuildToolVersion.Default;
            }
        }

        /// <summary>
        /// Sets the platform target.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="target">The target.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetPlatformTarget(this MSBuildSettings settings, PlatformTarget target)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            settings.PlatformTarget = target;
            return settings;
        }

        /// <summary>
        /// Sets the platform target.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="target">The target.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetPlatformTarget(this MSBuildSettings settings, string target)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.WithProperty("Platform", target);
            return settings;
        }

        /// <summary>
        /// Sets the MSBuild platform.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="platform">The platform.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetMSBuildPlatform(this MSBuildSettings settings, MSBuildPlatform platform)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            settings.MSBuildPlatform = platform;
            return settings;
        }

        /// <summary>
        /// Adds a property to the configuration.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="name">The property name.</param>
        /// <param name="values">The property values.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings WithProperty(this MSBuildSettings settings, string name, params string[] values)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            IList<string> currentValue;
            currentValue = new List<string>(
                settings.Properties.TryGetValue(name, out currentValue) && currentValue != null
                    ? currentValue.Concat(values)
                    : values);

            settings.Properties[name] = currentValue;

            return settings;
        }

        /// <summary>
        /// Sets the configuration.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetConfiguration(this MSBuildSettings settings, string configuration)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            settings.Configuration = configuration;
            return settings;
        }

        /// <summary>
        /// Sets the maximum CPU count. Without this set MSBuild will compile projects in this solution one at a time.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="maxCpuCount">The maximum CPU count. Set this value to zero to use as many MSBuild processes as available CPUs.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetMaxCpuCount(this MSBuildSettings settings, int? maxCpuCount)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (maxCpuCount.HasValue)
            {
                settings.MaxCpuCount = Math.Max(0, maxCpuCount.Value);
            }
            else
            {
                settings.MaxCpuCount = null;
            }
            return settings;
        }

        /// <summary>
        /// Sets whether or not node reuse should be enabled.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="reuse"><c>true</c> if node reuse should be enabled; otherwise <c>false</c>.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetNodeReuse(this MSBuildSettings settings, bool reuse)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            settings.NodeReuse = reuse;
            return settings;
        }

        /// <summary>
        /// Sets whether or not detailed summary should be enabled.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="detailedSummary"><c>true</c> if detailed summary should be enabled; otherwise <c>false</c>.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetDetailedSummary(this MSBuildSettings settings, bool detailedSummary)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            settings.DetailedSummary = detailedSummary;
            return settings;
        }

        /// <summary>
        /// Sets whether or not no console logging should be enabled.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="noConsoleLog"><c>true</c> if no console log should be enabled; otherwise <c>false</c>.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetNoConsoleLogger(this MSBuildSettings settings, bool noConsoleLog)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            settings.NoConsoleLogger = noConsoleLog;
            return settings;
        }

        /// <summary>
        /// Sets whether or not copyright information at the start of the program should be shown.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="noLogo"><c>true</c> if no copyright information at the start of the program should be shown; otherwise <c>false</c>.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetNoLogo(this MSBuildSettings settings, bool noLogo)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            settings.NoLogo = noLogo;
            return settings;
        }

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
        public static MSBuildSettings SetVersion(this MSBuildSettings settings, string version)
            => settings.WithProperty("Version", version);

        /// <summary>
        /// Sets the version prefix.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="versionPrefix">The version prefix.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetVersionPrefix(this MSBuildSettings settings, string versionPrefix)
            => settings.WithProperty("VersionPrefix", versionPrefix);

        /// <summary>
        /// Sets the version suffix.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="versionSuffix">The version suffix.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetVersionSuffix(this MSBuildSettings settings, string versionSuffix)
            => settings.WithProperty("VersionSuffix", versionSuffix);

        /// <summary>
        /// Sets the file version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="fileVersion">The file version.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetFileVersion(this MSBuildSettings settings, string fileVersion)
            => settings.WithProperty("FileVersion", fileVersion);

        /// <summary>
        /// Sets the assembly version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="assemblyVersion">The assembly version.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetAssemblyVersion(this MSBuildSettings settings, string assemblyVersion)
            => settings.WithProperty("AssemblyVersion", assemblyVersion);

        /// <summary>
        /// Sets the informational version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="informationalVersion">The informational version.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetInformationalVersion(this MSBuildSettings settings, string informationalVersion)
            => settings.WithProperty("InformationalVersion", informationalVersion);

        /// <summary>
        /// Sets the package version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="packageVersion">The package version.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetPackageVersion(this MSBuildSettings settings, string packageVersion)
            => settings.WithProperty("PackageVersion", packageVersion);

        /// <summary>
        /// Sets the package release notes.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="packageReleaseNotes">The package release notes.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetPackageReleaseNotes(this MSBuildSettings settings, string packageReleaseNotes)
            => settings.WithProperty("PackageReleaseNotes", packageReleaseNotes);

        /// <summary>
        /// Sets a value indicating whether to normalize stored file paths used when producing deterministic builds.
        /// </summary>
        /// <remarks>
        /// For more information see https://devblogs.microsoft.com/dotnet/producing-packages-with-source-link/#deterministic-builds.
        /// </remarks>
        /// <param name="settings">The settings.</param>
        /// <param name="continuousIntegrationBuild">A value indicating whether to normalize stored file paths used when producing deterministic builds.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetContinuousIntegrationBuild(this MSBuildSettings settings, bool? continuousIntegrationBuild = true)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.ContinuousIntegrationBuild = continuousIntegrationBuild;
            return settings;
        }

        /// <summary>
        /// Sets whether or not any targets should be passed to MSBuild.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="noImplicitTarget"><c>true</c> if no implicit target should be passed to MSBuild; otherwise <c>false</c>.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetNoImplicitTarget(this MSBuildSettings settings, bool noImplicitTarget)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            settings.NoImplicitTarget = noImplicitTarget;
            return settings;
        }

        /// <summary>
        /// Sets whether or not a symbol package should be created.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="includeSymbols"><c>true</c> if a symbol package should be created; otherwise <c>false</c>.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetIncludeSymbols(this MSBuildSettings settings, bool includeSymbols)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            settings.IncludeSymbols = includeSymbols;
            return settings;
        }

        /// <summary>
        /// Sets the symbol package format.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="symbolPackageFormat">The symbol package format.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetSymbolPackageFormat(this MSBuildSettings settings, string symbolPackageFormat)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            settings.SymbolPackageFormat = symbolPackageFormat;
            return settings;
        }

        /// <summary>
        /// Sets the build log verbosity.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="verbosity">The build log verbosity.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetVerbosity(this MSBuildSettings settings, Verbosity verbosity)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            settings.Verbosity = verbosity;
            return settings;
        }

        /// <summary>
        /// Adds a custom logger.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="loggerAssembly">The assembly containing the logger. Should match the format {AssemblyName[,StrongName] | AssemblyFile}.</param>
        /// <param name="loggerClass">The class implementing the logger. Should match the format [PartialOrFullNamespace.]LoggerClassName. If the assembly contains only one logger, class does not need to be specified.</param>
        /// <param name="loggerParameters">Parameters to be passed to the logger.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings WithLogger(this MSBuildSettings settings, string loggerAssembly, string loggerClass = null, string loggerParameters = null)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
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
        /// Adds a file logger with all the default settings.
        /// Each file logger will be declared in the order added.
        /// The first file logger will match up to the /fl parameter.
        /// The next nine (max) file loggers will match up to the /fl1 through /fl9 respectively.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings AddFileLogger(this MSBuildSettings settings)
        {
            return AddFileLogger(settings, new MSBuildFileLogger());
        }

        /// <summary>
        /// Adds a file logger.
        /// Each file logger will be declared in the order added.
        /// The first file logger will match up to the /fl parameter.
        /// The next nine (max) file loggers will match up to the /fl1 through /fl9 respectively.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="fileLoggerParameters">Parameters to be passed to the logger.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings AddFileLogger(this MSBuildSettings settings, MSBuildFileLogger fileLoggerParameters)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
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
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings EnableBinaryLogger(this MSBuildSettings settings)
        {
            return EnableBinaryLogger(settings, MSBuildBinaryLogImports.Unspecified);
        }

        /// <summary>
        /// Enables the binary logger with the specified imports and default file name.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="imports">The imports.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings EnableBinaryLogger(this MSBuildSettings settings, MSBuildBinaryLogImports imports)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.BinaryLogger = new MSBuildBinaryLogSettings
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
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings EnableBinaryLogger(this MSBuildSettings settings, string fileName)
        {
            return EnableBinaryLogger(settings, fileName, MSBuildBinaryLogImports.Unspecified);
        }

        /// <summary>
        /// Enables the binary logger with the specified log file name and imports.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="fileName">The log file name.</param>
        /// <param name="imports">The imports.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings EnableBinaryLogger(this MSBuildSettings settings, string fileName, MSBuildBinaryLogImports imports)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.BinaryLogger = new MSBuildBinaryLogSettings
            {
                Enabled = true,
                FileName = fileName,
                Imports = imports,
            };

            return settings;
        }

        /// <summary>
        /// Treat warnings as errors, if no codes specified all errors will be treated as errors.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="codes">Only treat specified warning codes as errors.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings WithWarningsAsError(this MSBuildSettings settings, params string[] codes)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.WarningsAsError = true;

            foreach (var code in codes)
            {
                settings.WarningsAsErrorCodes.Add(code);
            }

            return settings;
        }

        /// <summary>
        /// Warnings to not treat as errors.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="codes">Warning codes to not treat as errors.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings WithWarningsAsMessage(this MSBuildSettings settings, params string[] codes)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            foreach (var code in codes)
            {
                settings.WarningsAsMessageCodes.Add(code);
            }

            return settings;
        }

        /// <summary>
        /// Invoke the Restore target before any other target.
        /// </summary>
        /// <param name="settings">The setting.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings WithRestore(this MSBuildSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.Restore = true;

            return settings;
        }

        /// <summary>
        /// Sets whether or not to lock the package dependency graph while restoring, using the packages.lock.json file.
        /// This setting is available with atleast Visual Studio 2017 version 15.9 and above or NET SDK version 2.1.500 and above.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="restoreLockedMode"><c>true</c> if locked mode restore should be enabled; otherwise <c>false</c>.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings SetRestoreLockedMode(this MSBuildSettings settings, bool restoreLockedMode)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.RestoreLockedMode = restoreLockedMode;
            return settings;
        }

        /// <summary>
        /// Adds a console logger parameter.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="parameter">The console logger parameter.</param>
        /// <returns>The same <see cref="MSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static MSBuildSettings WithConsoleLoggerParameter(this MSBuildSettings settings, string parameter)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.ConsoleLoggerParameters.Add(parameter);
            return settings;
        }
    }
}