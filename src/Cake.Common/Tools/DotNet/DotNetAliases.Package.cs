// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Package.Add;
using Cake.Common.Tools.DotNet.Package.Download;
using Cake.Common.Tools.DotNet.Package.List;
using Cake.Common.Tools.DotNet.Package.Remove;
using Cake.Common.Tools.DotNet.Package.Search;
using Cake.Common.Tools.DotNet.Package.Update;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet;

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
    /// <summary>
    /// Adds or updates a package reference in a project file.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packageName">The package reference to add.</param>
    /// <example>
    /// <code>
    /// DotNetAddPackage("Cake.FileHelper");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Add")]
    public static void DotNetAddPackage(this ICakeContext context, string packageName)
    {
        context.DotNetAddPackage(packageName, null, null);
    }

    /// <summary>
    /// Adds or updates a package reference in a project file.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packageName">The package reference to add.</param>
    /// <param name="project">The target project file path.</param>
    /// <example>
    /// <code>
    /// DotNetAddPackage("Cake.FileHelper", "ToDo.csproj");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Add")]
    public static void DotNetAddPackage(this ICakeContext context, string packageName, string project)
    {
        context.DotNetAddPackage(packageName, project, null);
    }

    /// <summary>
    /// Adds or updates a package reference in a project file.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packageName">The package reference to add.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// var settings = new DotNetPackageAddSettings
    /// {
    ///     NoRestore = true,
    ///     Version = "6.1.3"
    /// };
    ///
    /// DotNetAddPackage("Cake.FileHelper", settings);
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Add")]
    public static void DotNetAddPackage(this ICakeContext context, string packageName, DotNetPackageAddSettings settings)
    {
        context.DotNetAddPackage(packageName, null, settings);
    }

    /// <summary>
    /// Adds or updates a package reference in a project file.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packageName">The package reference to add.</param>
    /// <param name="project">The target project file path.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// var settings = new DotNetPackageAddSettings
    /// {
    ///     NoRestore = true,
    ///     Version = "6.1.3"
    /// };
    ///
    /// DotNetAddPackage("Cake.FileHelper", "ToDo.csproj", settings);
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Add")]
    public static void DotNetAddPackage(this ICakeContext context, string packageName, string project, DotNetPackageAddSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (settings is null)
        {
            settings = new DotNetPackageAddSettings();
        }

        var adder = new DotNetPackageAdder(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        adder.Add(packageName, project, settings);
    }

    /// <summary>
    /// Removes package reference from a project file.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packageName">The package reference to remove.</param>
    /// <example>
    /// <code>
    /// DotNetRemovePackage("Cake.FileHelper");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Remove")]
    public static void DotNetRemovePackage(this ICakeContext context, string packageName)
    {
        context.DotNetRemovePackage(packageName, null);
    }

    /// <summary>
    /// Removes package reference from a project file.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packageName">The package reference to remove.</param>
    /// <param name="project">The target project file path.</param>
    /// <example>
    /// <code>
    /// DotNetRemovePackage("Cake.FileHelper", "ToDo.csproj");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Remove")]
    public static void DotNetRemovePackage(this ICakeContext context, string packageName, string project)
    {
        context.DotNetRemovePackage(packageName, project, null);
    }

    /// <summary>
    /// Removes package reference from a project file.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packageName">The package reference to remove.</param>
    /// <param name="project">The target project file path.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// DotNetRemovePackage(
    ///     "Cake.FileHelper",
    ///     "ToDo.csproj",
    ///     new DotNetPackageRemoveSettings { WorkingDirectory = "./src" });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Remove")]
    public static void DotNetRemovePackage(this ICakeContext context, string packageName, string project, DotNetPackageRemoveSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);
        settings ??= new DotNetPackageRemoveSettings();
        var remover = new DotNetPackageRemover(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        remover.Remove(packageName, project, settings);
    }

    /// <summary>
    /// Updates referenced packages in a project.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <example>
    /// <code>
    /// DotNetUpdatePackage();
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Update")]
    public static void DotNetUpdatePackage(this ICakeContext context)
    {
        context.DotNetUpdatePackage((IEnumerable<string>)null, null, null);
    }

    /// <summary>
    /// Updates referenced packages in a project.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// DotNetUpdatePackage(new DotNetPackageUpdateSettings {
    ///     Project = "./src/App.csproj",
    ///     Vulnerable = true
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Update")]
    public static void DotNetUpdatePackage(this ICakeContext context, DotNetPackageUpdateSettings settings)
    {
        context.DotNetUpdatePackage((IEnumerable<string>)null, null, settings);
    }

    /// <summary>
    /// Updates a package reference in a project.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packageName">The package to update.</param>
    /// <example>
    /// <code>
    /// DotNetUpdatePackage("Newtonsoft.Json");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Update")]
    public static void DotNetUpdatePackage(this ICakeContext context, string packageName)
    {
        context.DotNetUpdatePackage(packageName, null, null);
    }

    /// <summary>
    /// Updates a package reference in a project.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packageName">The package to update.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// DotNetUpdatePackage("Newtonsoft.Json", new DotNetPackageUpdateSettings {
    ///     Vulnerable = true
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Update")]
    public static void DotNetUpdatePackage(this ICakeContext context, string packageName, DotNetPackageUpdateSettings settings)
    {
        context.DotNetUpdatePackage(packageName, null, settings);
    }

    /// <summary>
    /// Updates a package reference in a project.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packageName">The package to update.</param>
    /// <param name="project">The target project file or directory.</param>
    /// <example>
    /// <code>
    /// DotNetUpdatePackage("Newtonsoft.Json", "ToDo.csproj");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Update")]
    public static void DotNetUpdatePackage(this ICakeContext context, string packageName, string project)
    {
        context.DotNetUpdatePackage(packageName, project, null);
    }

    /// <summary>
    /// Updates a package reference in a project.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packageName">The package to update.</param>
    /// <param name="project">The target project file or directory.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// DotNetUpdatePackage("Newtonsoft.Json", "ToDo.csproj", new DotNetPackageUpdateSettings {
    ///     Interactive = true
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Update")]
    public static void DotNetUpdatePackage(this ICakeContext context, string packageName, string project, DotNetPackageUpdateSettings settings)
    {
        IEnumerable<string> packages = string.IsNullOrWhiteSpace(packageName) ? null : [packageName];
        context.DotNetUpdatePackage(packages, project, settings);
    }

    /// <summary>
    /// Updates package references in a project.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packages">The packages to update.</param>
    /// <example>
    /// <code>
    /// DotNetUpdatePackage(new[] { "Contoso.Utilities", "Fabrikam.WebApi@1.2.3" });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Update")]
    public static void DotNetUpdatePackage(this ICakeContext context, IEnumerable<string> packages)
    {
        context.DotNetUpdatePackage(packages, null, null);
    }

    /// <summary>
    /// Updates package references in a project.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packages">The packages to update.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// DotNetUpdatePackage(new[] { "Contoso.Utilities", "Fabrikam.WebApi@1.2.3" }, new DotNetPackageUpdateSettings {
    ///     Vulnerable = true
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Update")]
    public static void DotNetUpdatePackage(this ICakeContext context, IEnumerable<string> packages, DotNetPackageUpdateSettings settings)
    {
        context.DotNetUpdatePackage(packages, null, settings);
    }

    /// <summary>
    /// Updates package references in a project.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packages">The packages to update.</param>
    /// <param name="project">The target project file or directory.</param>
    /// <example>
    /// <code>
    /// DotNetUpdatePackage(new[] { "Contoso.Utilities", "Fabrikam.WebApi@1.2.3" }, "ToDo.csproj");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Update")]
    public static void DotNetUpdatePackage(this ICakeContext context, IEnumerable<string> packages, string project)
    {
        context.DotNetUpdatePackage(packages, project, null);
    }

    /// <summary>
    /// Updates package references in a project.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packages">The packages to update.</param>
    /// <param name="project">The target project file or directory.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// DotNetUpdatePackage(
    ///     new[] { "Contoso.Utilities", "Fabrikam.WebApi@1.2.3" },
    ///     "ToDo.csproj",
    ///     new DotNetPackageUpdateSettings { Interactive = true });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Update")]
    public static void DotNetUpdatePackage(this ICakeContext context, IEnumerable<string> packages, string project, DotNetPackageUpdateSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);

        settings ??= new DotNetPackageUpdateSettings();

        var updater = new DotNetPackageUpdater(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        updater.Update(packages, project, settings);
    }

    /// <summary>
    /// Downloads a NuGet package to disk without changing project references.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="package">The package to download. A package id or <c>id@version</c>.</param>
    /// <example>
    /// <code>
    /// DotNetDownloadPackage("Newtonsoft.Json@13.0.3");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Download")]
    public static void DotNetDownloadPackage(this ICakeContext context, string package)
    {
        context.DotNetDownloadPackage(package, (DotNetPackageDownloadSettings)null);
    }

    /// <summary>
    /// Downloads a NuGet package to the specified directory without changing project references.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="package">The package to download. A package id or <c>id@version</c>.</param>
    /// <param name="output">The directory to download the package to.</param>
    /// <example>
    /// <code>
    /// DotNetDownloadPackage("Newtonsoft.Json@13.0.3", "./packages");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Download")]
    public static void DotNetDownloadPackage(this ICakeContext context, string package, DirectoryPath output)
    {
        context.DotNetDownloadPackage(package, new DotNetPackageDownloadSettings { Output = output });
    }

    /// <summary>
    /// Downloads a NuGet package to disk without changing project references.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="package">The package to download. A package id or <c>id@version</c>.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// DotNetDownloadPackage("Newtonsoft.Json@13.0.3", new DotNetPackageDownloadSettings {
    ///     Output = "./packages",
    ///     Prerelease = true
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Download")]
    public static void DotNetDownloadPackage(this ICakeContext context, string package, DotNetPackageDownloadSettings settings)
    {
        IEnumerable<string> packages = string.IsNullOrWhiteSpace(package) ? null : [package];
        context.DotNetDownloadPackage(packages, settings);
    }

    /// <summary>
    /// Downloads NuGet packages to the specified directory without changing project references.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packages">The packages to download. Each value is a package id or <c>id@version</c>.</param>
    /// <param name="output">The directory to download the packages to.</param>
    /// <example>
    /// <code>
    /// DotNetDownloadPackage(new[] { "Newtonsoft.Json@13.0.3", "Cake.Core" }, "./packages");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Download")]
    public static void DotNetDownloadPackage(this ICakeContext context, IEnumerable<string> packages, DirectoryPath output)
    {
        context.DotNetDownloadPackage(packages, new DotNetPackageDownloadSettings { Output = output });
    }

    /// <summary>
    /// Downloads NuGet packages to disk without changing project references.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="packages">The packages to download. Each value is a package id or <c>id@version</c>.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// DotNetDownloadPackage(
    ///     new[] { "Newtonsoft.Json@13.0.3", "Cake.Core" },
    ///     new DotNetPackageDownloadSettings { Output = "./packages" });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Download")]
    public static void DotNetDownloadPackage(this ICakeContext context, IEnumerable<string> packages, DotNetPackageDownloadSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);

        settings ??= new DotNetPackageDownloadSettings();

        var downloader = new DotNetPackageDownloader(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        downloader.Download(packages, settings);
    }

    /// <summary>
    /// List packages on available from source using specified settings.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="searchTerm">The search term.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>List of packages with their version.</returns>
    /// <example>
    /// <code>
    /// var packageList = DotNetPackageSearch("Cake", new DotNetPackageSearchSettings {
    ///     AllVersions = false,
    ///     Prerelease = false
    ///     });
    /// foreach (var package in packageList)
    /// {
    ///     Information("Found package {0}, version {1}", package.Name, package.Version);
    /// }
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Search")]
    public static IEnumerable<DotNetPackageSearchItem> DotNetSearchPackage(this ICakeContext context, string searchTerm, DotNetPackageSearchSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);
        var runner = new DotNetPackageSearcher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        return runner.Search(searchTerm, settings);
    }

    /// <summary>
    /// List packages on available from source using specified settings.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="searchTerm">The package Id.</param>
    /// <returns>List of packages with their version.</returns>
    /// <example>
    /// <code>
    /// var packageList = DotNetPackageSearch("Cake", new DotNetPackageSearchSettings {
    ///     AllVersions = false,
    ///     Prerelease = false
    ///     });
    /// foreach (var package in packageList)
    /// {
    ///     Information("Found package {0}, version {1}", package.Name, package.Version);
    /// }
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Search")]
    public static IEnumerable<DotNetPackageSearchItem> DotNetSearchPackage(this ICakeContext context, string searchTerm)
    {
        ArgumentNullException.ThrowIfNull(context);
        var runner = new DotNetPackageSearcher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        return runner.Search(searchTerm, new DotNetPackageSearchSettings());
    }

    /// <summary>
    /// List packages on available from source using specified settings.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>List of packages with their version.</returns>
    /// <example>
    /// <code>
    /// var packageList = DotNetPackageSearch("Cake", new DotNetPackageSearchSettings {
    ///     AllVersions = false,
    ///     Prerelease = false
    ///     });
    /// foreach (var package in packageList)
    /// {
    ///     Information("Found package {0}, version {1}", package.Name, package.Version);
    /// }
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.Search")]
    public static IEnumerable<DotNetPackageSearchItem> DotNetSearchPackage(this ICakeContext context, DotNetPackageSearchSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);
        var runner = new DotNetPackageSearcher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        return runner.Search(null, settings);
    }

    /// <summary>
    /// Lists the package references for a project or solution.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>The the package references.</returns>
    /// <example>
    /// <code>
    /// DotNetPackageList output = DotNetListPackage();
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.List")]
    public static DotNetPackageList DotNetListPackage(this ICakeContext context)
    {
        return context.DotNetListPackage(null);
    }

    /// <summary>
    /// Lists the package references for a project or solution.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="project">The project or solution file to operate on. If not specified, the command searches the current directory for one. If more than one solution or project is found, an error is thrown.</param>
    /// <returns>The the package references.</returns>
    /// <example>
    /// <code>
    /// DotNetPackageList output = DotNetListPackage("./src/MyProject/MyProject.csproj");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.List")]
    public static DotNetPackageList DotNetListPackage(this ICakeContext context, string project)
    {
        return context.DotNetListPackage(project, null);
    }

    /// <summary>
    /// Lists the package references for a project or solution.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="project">The project or solution file to operate on. If not specified, the command searches the current directory for one. If more than one solution or project is found, an error is thrown.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>The the package references.</returns>
    /// <example>
    /// <code>
    /// var settings = new DotNetPackageListSettings
    /// {
    ///     Outdated = true
    /// };
    ///
    /// DotNetPackageList output = DotNetListPackage("./src/MyProject/MyProject.csproj", settings);
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Package")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Package.List")]
    public static DotNetPackageList DotNetListPackage(this ICakeContext context, string project, DotNetPackageListSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (settings is null)
        {
            settings = new DotNetPackageListSettings();
        }

        var lister = new DotNetPackageLister(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        return lister.List(project, settings);
    }
}
