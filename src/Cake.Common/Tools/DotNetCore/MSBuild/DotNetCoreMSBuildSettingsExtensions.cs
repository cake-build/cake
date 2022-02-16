// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.WithTarget(settings, target);
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
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.WithProperty(settings, name, values);
        }

        /// <summary>
        /// Shows detailed information at the end of the build log about the configurations that were built and how they were scheduled to nodes.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings ShowDetailedSummary(this DotNetCoreMSBuildSettings settings)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.ShowDetailedSummary(settings);
        }

        /// <summary>
        /// Adds a extension to ignore when determining which project file to build.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="extension">The extension to ignore.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings WithIgnoredProjectExtension(this DotNetCoreMSBuildSettings settings, string extension)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.WithIgnoredProjectExtension(settings, extension);
        }

        /// <summary>
        /// Sets the maximum CPU count. Without this set MSBuild will compile projects in this solution one at a time.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="maxCpuCount">The maximum CPU count. Set this value to zero to use as many MSBuild processes as available CPUs.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetMaxCpuCount(this DotNetCoreMSBuildSettings settings, int? maxCpuCount)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SetMaxCpuCount(settings, maxCpuCount);
        }

        /// <summary>
        /// Exclude any MSBuild.rsp files automatically.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings ExcludeAutoResponseFiles(this DotNetCoreMSBuildSettings settings)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.ExcludeAutoResponseFiles(settings);
        }

        /// <summary>
        /// Hide the startup banner and the copyright message.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings HideLogo(this DotNetCoreMSBuildSettings settings)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.HideLogo(settings);
        }

        /// <summary>
        /// Sets a value indicating whether to normalize stored file paths used when producing deterministic builds.
        /// </summary>
        /// <remarks>
        /// For more information see https://devblogs.microsoft.com/dotnet/producing-packages-with-source-link/#deterministic-builds.
        /// </remarks>
        /// <param name="settings">The settings.</param>
        /// <param name="continuousIntegrationBuild">A value indicating whether to normalize stored file paths used when producing deterministic builds.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetContinuousIntegrationBuild(this DotNetCoreMSBuildSettings settings, bool? continuousIntegrationBuild = true)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SetContinuousIntegrationBuild(settings, continuousIntegrationBuild);
        }

        /// <summary>
        /// Sets the version of the Toolset to use to build the project.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="version">The version.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings UseToolVersion(this DotNetCoreMSBuildSettings settings, MSBuildVersion version)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.UseToolVersion(settings, version);
        }

        /// <summary>
        /// Validate the project file and, if validation succeeds, build the project.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings ValidateProjectFile(this DotNetCoreMSBuildSettings settings)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.ValidateProjectFile(settings);
        }

        /// <summary>
        /// Adds a response file to use.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="responseFile">The response file to add.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// A response file is a text file that is used to insert command-line switches. For more information see https://docs.microsoft.com/en-gb/visualstudio/msbuild/msbuild-response-files.
        /// </remarks>
        public static DotNetCoreMSBuildSettings WithResponseFile(this DotNetCoreMSBuildSettings settings, FilePath responseFile)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.WithResponseFile(settings, responseFile);
        }

        /// <summary>
        /// Log the build output of each MSBuild node to its own file.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings UseDistributedFileLogger(this DotNetCoreMSBuildSettings settings)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.UseDistributedFileLogger(settings);
        }

        /// <summary>
        /// Adds a distributed loggers to use.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="logger">The response file to add.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// A distributed logger consists of a central and forwarding logger. MSBuild will attach an instance of the forwarding logger to each secondary node.
        /// For more information see https://msdn.microsoft.com/en-us/library/bb383987.aspx.
        /// </remarks>
        public static DotNetCoreMSBuildSettings WithDistributedLogger(this DotNetCoreMSBuildSettings settings, MSBuildDistributedLogger logger)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.WithDistributedLogger(settings, logger);
        }

        /// <summary>
        /// Sets the parameters for the console logger.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="consoleLoggerParameters">The console logger parameters to set.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetConsoleLoggerSettings(this DotNetCoreMSBuildSettings settings, MSBuildLoggerSettings consoleLoggerParameters)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SetConsoleLoggerSettings(settings, consoleLoggerParameters);
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
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.AddFileLogger(settings);
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
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.AddFileLogger(settings, fileLoggerParameters);
        }

        /// <summary>
        /// Enables the binary logger with all the default settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings EnableBinaryLogger(this DotNetCoreMSBuildSettings settings)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.EnableBinaryLogger(settings);
        }

        /// <summary>
        /// Enables the binary logger with the specified imports and default file name.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="imports">The imports.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings EnableBinaryLogger(this DotNetCoreMSBuildSettings settings, MSBuildBinaryLoggerImports imports)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.EnableBinaryLogger(settings, imports);
        }

        /// <summary>
        /// Enables the binary logger with the specified log file name and no imports.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="fileName">The log file name.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings EnableBinaryLogger(this DotNetCoreMSBuildSettings settings, string fileName)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.EnableBinaryLogger(settings, fileName);
        }

        /// <summary>
        /// Enables the binary logger with the specified log file name and imports.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="fileName">The log file name.</param>
        /// <param name="imports">The imports.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings EnableBinaryLogger(this DotNetCoreMSBuildSettings settings, string fileName, MSBuildBinaryLoggerImports imports)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.EnableBinaryLogger(settings, fileName, imports);
        }

        /// <summary>
        /// Adds a custom logger.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="loggerAssembly">The assembly containing the logger. Should match the format {AssemblyName[,StrongName] | AssemblyFile}.</param>
        /// <param name="loggerClass">The class implementing the logger. Should match the format [PartialOrFullNamespace.]LoggerClassName. If the assembly contains only one logger, class does not need to be specified.</param>
        /// <param name="loggerParameters">Parameters to be passed to the logger.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings WithLogger(this DotNetCoreMSBuildSettings settings, string loggerAssembly, string loggerClass = null, string loggerParameters = null)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.WithLogger(settings, loggerAssembly, loggerClass, loggerParameters);
        }

        /// <summary>
        /// Disables the default console logger, and not log events to the console.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings DisableConsoleLogger(this DotNetCoreMSBuildSettings settings)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.DisableConsoleLogger(settings);
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
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SetWarningCodeAsError(settings, warningCode);
        }

        /// <summary>
        /// Sets the warning code to treats as a message.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="warningCode">The warning code to treat as a message.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetWarningCodeAsMessage(this DotNetCoreMSBuildSettings settings, string warningCode)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SetWarningCodeAsMessage(settings, warningCode);
        }

        /// <summary>
        /// Sets how all warnings should be treated.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="behaviour">How all warning should be treated.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings TreatAllWarningsAs(this DotNetCoreMSBuildSettings settings, MSBuildTreatAllWarningsAs behaviour)
        {
            return (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.TreatAllWarningsAs(settings, behaviour);
        }

        /// <summary>
        /// Sets the configuration.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetConfiguration(this DotNetCoreMSBuildSettings settings, string configuration)
            => (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SetConfiguration(settings, configuration);

        /// <summary>
        /// Sets the version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="version">The version.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// Version will override VersionPrefix and VersionSuffix if set.
        /// This may also override version settings during packaging.
        /// </remarks>
        public static DotNetCoreMSBuildSettings SetVersion(this DotNetCoreMSBuildSettings settings, string version)
            => (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SetVersion(settings, version);

        /// <summary>
        /// Sets the file version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="fileVersion">The file version.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetFileVersion(this DotNetCoreMSBuildSettings settings, string fileVersion)
            => (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SetFileVersion(settings, fileVersion);

        /// <summary>
        /// Sets the assembly version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="assemblyVersion">The assembly version.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetAssemblyVersion(this DotNetCoreMSBuildSettings settings, string assemblyVersion)
            => (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SetAssemblyVersion(settings, assemblyVersion);

        /// <summary>
        /// Sets the informational version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="informationalVersion">The informational version.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetInformationalVersion(this DotNetCoreMSBuildSettings settings, string informationalVersion)
            => (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SetInformationalVersion(settings, informationalVersion);

        /// <summary>
        /// Sets the package version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="packageVersion">The package version.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetPackageVersion(this DotNetCoreMSBuildSettings settings, string packageVersion)
            => (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SetPackageVersion(settings, packageVersion);

        /// <summary>
        /// Sets the package release notes.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="packageReleaseNotes">The package release notes.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetPackageReleaseNotes(this DotNetCoreMSBuildSettings settings, string packageReleaseNotes)
            => (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SetPackageReleaseNotes(settings, packageReleaseNotes);

        /// <summary>
        /// Suppress warning CS7035.
        /// This is useful when using semantic versioning and either the file or informational version
        /// doesn't match the recommended format.
        /// The recommended format is: major.minor.build.revision where
        /// each is an integer between 0 and 65534 (inclusive).
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SuppressVersionRecommendedFormatWarning(this DotNetCoreMSBuildSettings settings)
            => (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SuppressVersionRecommendedFormatWarning(settings);

        /// <summary>
        /// Sets the version prefix.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="versionPrefix">The version prefix.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetVersionPrefix(this DotNetCoreMSBuildSettings settings, string versionPrefix)
            => (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SetVersionPrefix(settings, versionPrefix);

        /// <summary>
        /// Sets the version Suffix.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="versionSuffix">The version prefix.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotNetCoreMSBuildSettings SetVersionSuffix(this DotNetCoreMSBuildSettings settings, string versionSuffix)
            => (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SetVersionSuffix(settings, versionSuffix);

        /// <summary>
        /// Adds a framework to target.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="targetFramework">The framework to target.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// For list of target frameworks see https://docs.microsoft.com/en-us/dotnet/standard/frameworks.
        /// </remarks>
        public static DotNetCoreMSBuildSettings SetTargetFramework(this DotNetCoreMSBuildSettings settings, string targetFramework)
            => (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SetTargetFramework(settings, targetFramework);

        /// <summary>
        /// Sets a target operating systems where the application or assembly will run.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="runtimeId">The runtime id of the operating system.</param>
        /// <returns>The same <see cref="DotNetCoreMSBuildSettings"/> instance so that multiple calls can be chained.</returns>
        /// <remarks>
        /// For list of runtime ids see https://docs.microsoft.com/en-us/dotnet/core/rid-catalog.
        /// </remarks>
        public static DotNetCoreMSBuildSettings SetRuntime(this DotNetCoreMSBuildSettings settings, string runtimeId)
            => (DotNetCoreMSBuildSettings)DotNet.MSBuild.DotNetMSBuildSettingsExtensions.SetRuntime(settings, runtimeId);
    }
}
