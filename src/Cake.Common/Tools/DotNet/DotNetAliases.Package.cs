// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Package.Add;
using Cake.Common.Tools.DotNet.Package.List;
using Cake.Common.Tools.DotNet.Package.Remove;
using Cake.Common.Tools.DotNet.Package.Search;
using Cake.Core;
using Cake.Core.Annotations;

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
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

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
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var adder = new DotNetPackageRemover(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            adder.Remove(packageName, project);
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
        /// foreach(var package in packageList)
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
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
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
        /// foreach(var package in packageList)
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
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
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
        /// foreach(var package in packageList)
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
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
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
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetPackageListSettings();
            }

            var lister = new DotNetPackageLister(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return lister.List(project, settings);
        }
    }
}
