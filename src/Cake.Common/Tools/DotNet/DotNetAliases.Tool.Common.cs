// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Tool;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/dotnet/cli">.NET CLI</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, the .NET CLI tools will need to be installed on the machine where
    /// the Cake script is being executed.  See this <see href="https://www.microsoft.com/net/core">page</see> for information
    /// on how to install.
    /// </para>
    /// </summary>
    public static partial class DotNetAliases
    {
        private static void RunDotNetToolCommand(
            ICakeContext context,
            FilePath projectPath,
            ProcessArgumentBuilder arguments,
            DotNetToolSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(arguments);
            ArgumentNullException.ThrowIfNull(settings);

            context.DotNetTool(projectPath, "tool", arguments, settings);
        }

        private static void RunDotNetToolCommandWithoutVerbosity(
            ICakeContext context,
            FilePath projectPath,
            ProcessArgumentBuilder arguments,
            DotNetToolSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(arguments);
            ArgumentNullException.ThrowIfNull(settings);

            var verbosity = settings.Verbosity;
            settings.Verbosity = null;

            try
            {
                context.DotNetTool(projectPath, "tool", arguments, settings);
            }
            finally
            {
                settings.Verbosity = verbosity;
            }
        }

        private static void RunDotNetToolCommandWithForwardedArguments(
            ICakeContext context,
            FilePath projectPath,
            ProcessArgumentBuilder arguments,
            ProcessArgumentBuilder forwardedArguments,
            DotNetToolSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(arguments);
            ArgumentNullException.ThrowIfNull(settings);

            if (forwardedArguments.IsNullOrEmpty())
            {
                context.DotNetTool(projectPath, "tool", arguments, settings);
                return;
            }

            var argumentCustomization = settings.ArgumentCustomization;
            settings.ArgumentCustomization = dotnetArguments =>
            {
                var customizedArguments = argumentCustomization?.Invoke(dotnetArguments) ?? dotnetArguments;
                customizedArguments.Append("--");
                forwardedArguments.CopyTo(customizedArguments);
                return customizedArguments;
            };

            try
            {
                context.DotNetTool(projectPath, "tool", arguments, settings);
            }
            finally
            {
                settings.ArgumentCustomization = argumentCustomization;
            }
        }

        private static void RunDotNetToolCommandWithForwardedArgumentsWithoutVerbosity(
            ICakeContext context,
            FilePath projectPath,
            ProcessArgumentBuilder arguments,
            ProcessArgumentBuilder forwardedArguments,
            DotNetToolSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(arguments);
            ArgumentNullException.ThrowIfNull(settings);

            var verbosity = settings.Verbosity;
            settings.Verbosity = null;

            try
            {
                RunDotNetToolCommandWithForwardedArguments(context, projectPath, arguments, forwardedArguments, settings);
            }
            finally
            {
                settings.Verbosity = verbosity;
            }
        }

        private static void AppendToolExecuteSettings(ProcessArgumentBuilder builder, DotNetToolExecuteSettings settings, ICakeEnvironment environment)
        {
            AppendPackageResolutionSettings(builder, settings.Version, settings.ConfigFile, settings.Source, settings.AddSource, settings.Prerelease, environment);
            AppendSwitch(builder, "--allow-roll-forward", settings.AllowRollForward);
            AppendRestoreSettings(builder, settings.DisableParallel, settings.IgnoreFailedSources, settings.NoHttpCache, settings.Interactive);
        }

        private static void AppendToolInstallSettings(ProcessArgumentBuilder builder, DotNetToolInstallSettings settings, ICakeConfiguration configuration, ICakeEnvironment environment)
        {
            AppendInstallationScope(builder, settings.InstallationScope, settings.ToolInstallationPath, settings.WorkingDirectory, configuration, environment);
            AppendPackageResolutionSettings(builder, settings.Version, settings.ConfigFile, settings.Source, settings.AddSource, settings.Prerelease, environment);
            AppendFilePath(builder, "--tool-manifest", settings.ToolManifest, environment);
            AppendString(builder, "--framework", settings.Framework);
            AppendRestoreSettings(builder, settings.DisableParallel, settings.IgnoreFailedSources, settings.NoHttpCache, settings.Interactive);
            AppendSwitch(builder, "--allow-downgrade", settings.AllowDowngrade);
            AppendString(builder, "--arch", settings.Architecture);
            AppendSwitch(builder, "--create-manifest-if-needed", settings.CreateManifestIfNeeded);
            AppendSwitch(builder, "--allow-roll-forward", settings.AllowRollForward);
        }

        private static void AppendToolListSettings(ProcessArgumentBuilder builder, DotNetToolListSettings settings, ICakeConfiguration configuration, ICakeEnvironment environment)
        {
            AppendInstallationScope(builder, settings.InstallationScope, settings.ToolInstallationPath, settings.WorkingDirectory, configuration, environment);

            if (settings.Format.HasValue)
            {
                builder.AppendSwitch("--format", settings.Format.Value == DotNetToolListFormat.Json ? "json" : "table");
            }
        }

        private static void AppendToolRestoreSettings(ProcessArgumentBuilder builder, DotNetToolRestoreSettings settings, ICakeEnvironment environment)
        {
            AppendFilePath(builder, "--configfile", settings.ConfigFile, environment);
            AppendSources(builder, "--add-source", settings.AddSource);
            AppendFilePath(builder, "--tool-manifest", settings.ToolManifest, environment);
            AppendRestoreSettings(builder, settings.DisableParallel, settings.IgnoreFailedSources, settings.NoHttpCache, settings.Interactive);
        }

        private static void AppendToolRunSettings(ProcessArgumentBuilder builder, DotNetToolRunSettings settings)
        {
            AppendSwitch(builder, "--allow-roll-forward", settings.AllowRollForward);
        }

        private static void AppendToolSearchSettings(ProcessArgumentBuilder builder, DotNetToolSearchSettings settings)
        {
            AppendSwitch(builder, "--detail", settings.Detail);

            if (settings.Skip.HasValue)
            {
                builder.AppendSwitch("--skip", settings.Skip.Value.ToString());
            }

            if (settings.Take.HasValue)
            {
                builder.AppendSwitch("--take", settings.Take.Value.ToString());
            }

            AppendSwitch(builder, "--prerelease", settings.Prerelease);
        }

        private static void AppendToolUninstallSettings(ProcessArgumentBuilder builder, DotNetToolUninstallSettings settings, ICakeConfiguration configuration, ICakeEnvironment environment)
        {
            AppendInstallationScope(builder, settings.InstallationScope, settings.ToolInstallationPath, settings.WorkingDirectory, configuration, environment);
            AppendFilePath(builder, "--tool-manifest", settings.ToolManifest, environment);
        }

        private static void AppendToolUpdateSettings(ProcessArgumentBuilder builder, DotNetToolUpdateSettings settings, ICakeConfiguration configuration, ICakeEnvironment environment)
        {
            AppendInstallationScope(builder, settings.InstallationScope, settings.ToolInstallationPath, settings.WorkingDirectory, configuration, environment);
            AppendPackageResolutionSettings(builder, settings.Version, settings.ConfigFile, settings.Source, settings.AddSource, settings.Prerelease, environment);
            AppendFilePath(builder, "--tool-manifest", settings.ToolManifest, environment);
            AppendString(builder, "--framework", settings.Framework);
            AppendRestoreSettings(builder, settings.DisableParallel, settings.IgnoreFailedSources, settings.NoHttpCache, settings.Interactive);
            AppendSwitch(builder, "--allow-downgrade", settings.AllowDowngrade);
            AppendSwitch(builder, "--all", settings.All);
        }

        private static void AppendInstallationScope(
            ProcessArgumentBuilder builder,
            DotNetToolInstallationScope scope,
            DirectoryPath toolInstallationPath,
            DirectoryPath settingsWorkingDirectory,
            ICakeConfiguration configuration,
            ICakeEnvironment environment)
        {
            switch (scope)
            {
                case DotNetToolInstallationScope.Global:
                    builder.Append("--global");
                    break;
                case DotNetToolInstallationScope.Local:
                    builder.Append("--local");
                    break;
                case DotNetToolInstallationScope.ToolPath:
                    var root = settingsWorkingDirectory ?? environment.WorkingDirectory;
                    var path = toolInstallationPath ?? configuration.GetToolPath(root, environment);
                    AppendDirectoryPath(builder, "--tool-path", path, environment);
                    break;
            }
        }

        private static void AppendPackageResolutionSettings(
            ProcessArgumentBuilder builder,
            string version,
            FilePath configFile,
            ICollection<string> source,
            ICollection<string> addSource,
            bool prerelease,
            ICakeEnvironment environment)
        {
            AppendString(builder, "--version", version);
            AppendFilePath(builder, "--configfile", configFile, environment);
            AppendSources(builder, "--source", source);
            AppendSources(builder, "--add-source", addSource);
            AppendSwitch(builder, "--prerelease", prerelease);
        }

        private static void AppendRestoreSettings(ProcessArgumentBuilder builder, bool disableParallel, bool ignoreFailedSources, bool noHttpCache, bool interactive)
        {
            AppendSwitch(builder, "--disable-parallel", disableParallel);
            AppendSwitch(builder, "--ignore-failed-sources", ignoreFailedSources);
            AppendSwitch(builder, "--no-http-cache", noHttpCache);
            AppendSwitch(builder, "--interactive", interactive);
        }

        private static void AppendRequiredArgument(ProcessArgumentBuilder builder, string argument, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentNullException(parameterName);
            }

            builder.Append(argument);
        }

        private static void AppendOptionalArgument(ProcessArgumentBuilder builder, string argument)
        {
            if (!string.IsNullOrWhiteSpace(argument))
            {
                builder.Append(argument);
            }
        }

        private static void AppendSwitch(ProcessArgumentBuilder builder, string switchName, bool value)
        {
            if (value)
            {
                builder.Append(switchName);
            }
        }

        private static void AppendString(ProcessArgumentBuilder builder, string switchName, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                builder.AppendSwitchQuoted(switchName, value);
            }
        }

        private static void AppendFilePath(ProcessArgumentBuilder builder, string switchName, FilePath value, ICakeEnvironment environment)
        {
            if (value != null)
            {
                builder.AppendSwitchQuoted(switchName, value.MakeAbsolute(environment).FullPath);
            }
        }

        private static void AppendDirectoryPath(ProcessArgumentBuilder builder, string switchName, DirectoryPath value, ICakeEnvironment environment)
        {
            if (value != null)
            {
                builder.AppendSwitchQuoted(switchName, value.MakeAbsolute(environment).FullPath);
            }
        }

        private static void AppendSources(ProcessArgumentBuilder builder, string switchName, ICollection<string> sources)
        {
            if (sources == null)
            {
                return;
            }

            foreach (var source in sources)
            {
                AppendString(builder, switchName, source);
            }
        }
    }
}
