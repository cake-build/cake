// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.Tool
{
    /// <summary>
    /// The installation scope for <c>dotnet tool</c> commands that support global, local, or tool-path installation.
    /// </summary>
    public enum DotNetToolInstallationScope
    {
        /// <summary>
        /// Maps to <c>--tool-path</c>. Uses <see cref="DotNetToolInstallSettings.ToolInstallationPath"/> when set;
        /// otherwise falls back to the Cake tools directory (same as the <c>#tool</c> directive).
        /// </summary>
        ToolPath,

        /// <summary>
        /// Maps to <c>--global</c>.
        /// </summary>
        Global,

        /// <summary>
        /// Maps to <c>--local</c>. Use <see cref="DotNetToolInstallSettings.ToolManifest"/> to target a specific manifest.
        /// </summary>
        Local,
    }

    /// <summary>
    /// The output format for <c>dotnet tool list</c>.
    /// </summary>
    public enum DotNetToolListFormat
    {
        /// <summary>
        /// Outputs the result as a table.
        /// </summary>
        Table,

        /// <summary>
        /// Outputs the result as JSON.
        /// </summary>
        Json
    }

    /// <summary>
    /// Contains settings used by <c>dotnet tool exec</c>.
    /// </summary>
    public sealed class DotNetToolExecuteSettings : DotNetToolSettings
    {
        /// <summary>
        /// Gets or sets the tool package version to execute.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow a .NET tool to roll forward to newer versions of the .NET runtime.
        /// </summary>
        public bool AllowRollForward { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include prerelease packages.
        /// </summary>
        public bool Prerelease { get; set; }

        /// <summary>
        /// Gets or sets the NuGet configuration file to use.
        /// </summary>
        public FilePath ConfigFile { get; set; }

        /// <summary>
        /// Gets or sets the package sources to use instead of configured NuGet package sources.
        /// </summary>
        public ICollection<string> Source { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets additional package sources to use during installation.
        /// </summary>
        public ICollection<string> AddSource { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether to prevent restoring multiple projects in parallel.
        /// </summary>
        public bool DisableParallel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to treat package source failures as warnings.
        /// </summary>
        public bool IgnoreFailedSources { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not cache packages and HTTP requests.
        /// </summary>
        public bool NoHttpCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow the command to stop and wait for user input or action.
        /// </summary>
        public bool Interactive { get; set; }
    }

    /// <summary>
    /// Contains settings used by <c>dotnet tool install</c>.
    /// </summary>
    public sealed class DotNetToolInstallSettings : DotNetToolSettings
    {
        /// <summary>
        /// Gets or sets the installation scope for the tool.
        /// </summary>
        public DotNetToolInstallationScope InstallationScope { get; set; }

        /// <summary>
        /// Gets or sets the directory where the tool will be installed when <see cref="InstallationScope"/> is <see cref="DotNetToolInstallationScope.ToolPath"/>.
        /// When not set, the Cake tools directory is used.
        /// </summary>
        public DirectoryPath ToolInstallationPath { get; set; }

        /// <summary>
        /// Gets or sets the tool package version to install.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the NuGet configuration file to use.
        /// </summary>
        public FilePath ConfigFile { get; set; }

        /// <summary>
        /// Gets or sets the path to the local tool manifest file.
        /// </summary>
        public FilePath ToolManifest { get; set; }

        /// <summary>
        /// Gets or sets additional package sources to use during installation.
        /// </summary>
        public ICollection<string> AddSource { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the package sources to use instead of configured NuGet package sources.
        /// </summary>
        public ICollection<string> Source { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the target framework to install the tool for.
        /// </summary>
        public string Framework { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include prerelease packages.
        /// </summary>
        public bool Prerelease { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to prevent restoring multiple projects in parallel.
        /// </summary>
        public bool DisableParallel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to treat package source failures as warnings.
        /// </summary>
        public bool IgnoreFailedSources { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not cache packages and HTTP requests.
        /// </summary>
        public bool NoHttpCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow the command to stop and wait for user input or action.
        /// </summary>
        public bool Interactive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow package downgrade when installing a .NET tool package.
        /// </summary>
        public bool AllowDowngrade { get; set; }

        /// <summary>
        /// Gets or sets the target architecture.
        /// </summary>
        public string Architecture { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to create a tool manifest if one is not found.
        /// </summary>
        public bool CreateManifestIfNeeded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow a .NET tool to roll forward to newer versions of the .NET runtime.
        /// </summary>
        public bool AllowRollForward { get; set; }
    }

    /// <summary>
    /// Contains settings used by <c>dotnet tool list</c>.
    /// </summary>
    public sealed class DotNetToolListSettings : DotNetToolSettings
    {
        /// <summary>
        /// Gets or sets the installation scope for listing tools.
        /// </summary>
        public DotNetToolInstallationScope InstallationScope { get; set; }

        /// <summary>
        /// Gets or sets the directory containing the tools to list when <see cref="InstallationScope"/> is <see cref="DotNetToolInstallationScope.ToolPath"/>.
        /// When not set, the Cake tools directory is used.
        /// </summary>
        public DirectoryPath ToolInstallationPath { get; set; }

        /// <summary>
        /// Gets or sets the output format.
        /// </summary>
        public DotNetToolListFormat? Format { get; set; }
    }

    /// <summary>
    /// Contains settings used by <c>dotnet tool restore</c>.
    /// </summary>
    public sealed class DotNetToolRestoreSettings : DotNetToolSettings
    {
        /// <summary>
        /// Gets or sets the NuGet configuration file to use.
        /// </summary>
        public FilePath ConfigFile { get; set; }

        /// <summary>
        /// Gets or sets additional package sources to use during installation.
        /// </summary>
        public ICollection<string> AddSource { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the path to the local tool manifest file.
        /// </summary>
        public FilePath ToolManifest { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to prevent restoring multiple projects in parallel.
        /// </summary>
        public bool DisableParallel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to treat package source failures as warnings.
        /// </summary>
        public bool IgnoreFailedSources { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not cache packages and HTTP requests.
        /// </summary>
        public bool NoHttpCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow the command to stop and wait for user input or action.
        /// </summary>
        public bool Interactive { get; set; }
    }

    /// <summary>
    /// Contains settings used by <c>dotnet tool run</c>.
    /// </summary>
    public sealed class DotNetToolRunSettings : DotNetToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to allow a .NET tool to roll forward to newer versions of the .NET runtime.
        /// </summary>
        public bool AllowRollForward { get; set; }
    }

    /// <summary>
    /// Contains settings used by <c>dotnet tool search</c>.
    /// </summary>
    public sealed class DotNetToolSearchSettings : DotNetToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show detailed query results.
        /// </summary>
        public bool Detail { get; set; }

        /// <summary>
        /// Gets or sets the number of results to skip for pagination.
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// Gets or sets the number of results to return for pagination.
        /// </summary>
        public int? Take { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include prerelease packages.
        /// </summary>
        public bool Prerelease { get; set; }
    }

    /// <summary>
    /// Contains settings used by <c>dotnet tool uninstall</c>.
    /// </summary>
    public sealed class DotNetToolUninstallSettings : DotNetToolSettings
    {
        /// <summary>
        /// Gets or sets the installation scope for uninstalling the tool.
        /// </summary>
        public DotNetToolInstallationScope InstallationScope { get; set; }

        /// <summary>
        /// Gets or sets the directory containing the tool to uninstall when <see cref="InstallationScope"/> is <see cref="DotNetToolInstallationScope.ToolPath"/>.
        /// When not set, the Cake tools directory is used.
        /// </summary>
        public DirectoryPath ToolInstallationPath { get; set; }

        /// <summary>
        /// Gets or sets the path to the local tool manifest file.
        /// </summary>
        public FilePath ToolManifest { get; set; }
    }

    /// <summary>
    /// Contains settings used by <c>dotnet tool update</c>.
    /// </summary>
    public sealed class DotNetToolUpdateSettings : DotNetToolSettings
    {
        /// <summary>
        /// Gets or sets the installation scope for updating the tool.
        /// </summary>
        public DotNetToolInstallationScope InstallationScope { get; set; }

        /// <summary>
        /// Gets or sets the directory where the tool is installed when <see cref="InstallationScope"/> is <see cref="DotNetToolInstallationScope.ToolPath"/>.
        /// When not set, the Cake tools directory is used.
        /// </summary>
        public DirectoryPath ToolInstallationPath { get; set; }

        /// <summary>
        /// Gets or sets the tool package version to update to.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the NuGet configuration file to use.
        /// </summary>
        public FilePath ConfigFile { get; set; }

        /// <summary>
        /// Gets or sets the path to the local tool manifest file.
        /// </summary>
        public FilePath ToolManifest { get; set; }

        /// <summary>
        /// Gets or sets additional package sources to use during installation.
        /// </summary>
        public ICollection<string> AddSource { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the package sources to use instead of configured NuGet package sources.
        /// </summary>
        public ICollection<string> Source { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the target framework to install the tool for.
        /// </summary>
        public string Framework { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include prerelease packages.
        /// </summary>
        public bool Prerelease { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to prevent restoring multiple projects in parallel.
        /// </summary>
        public bool DisableParallel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to treat package source failures as warnings.
        /// </summary>
        public bool IgnoreFailedSources { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not cache packages and HTTP requests.
        /// </summary>
        public bool NoHttpCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow the command to stop and wait for user input or action.
        /// </summary>
        public bool Interactive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow package downgrade when updating a .NET tool package.
        /// </summary>
        public bool AllowDowngrade { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to update all tools.
        /// </summary>
        public bool All { get; set; }
    }
}
