// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Common.Tools.DNU.Build;
using Cake.Common.Tools.DNU.Pack;
using Cake.Common.Tools.DNU.Restore;
using Cake.Common.Tools.DotNetCore;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.DNU
{
    /// <summary>
    /// Contains functionality for working with the DNU Utility.
    /// <para>These aliases have been marked as Obsolete.  Use the <see cref="DotNetCoreAliases" /> instead.</para>
    /// </summary>
    [CakeAliasCategory("DNU")]
    public static class DNUAliases
    {
        /// <summary>
        /// Restore all NuGet Packages.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        ///     DNURestore();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Common.Tools.DNU.Restore")]
        [Obsolete("Please use DotNetCoreRestore() instead.", false)]
        public static void DNURestore(this ICakeContext context)
        {
            context.DNURestore(null, null);
        }

        /// <summary>
        /// Restore all NuGet Packages in the specified path.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file to restore.</param>
        /// <example>
        /// <code>
        ///     var projects = GetFiles("./src/**/project.json");
        ///     foreach(var project in projects)
        ///     {
        ///         DNURestore(project);
        ///     }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Common.Tools.DNU.Restore")]
        [Obsolete("Please use DotNetCoreRestore(string) instead.", false)]
        public static void DNURestore(this ICakeContext context, FilePath filePath)
        {
            context.DNURestore(filePath, null);
        }

        /// <summary>
        /// Restore all NuGet Packages with the settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var settings = new DNURestoreSettings
        ///     {
        ///         Sources = new[] {"https://www.example.com/nugetfeed", "https://www.example.com/nugetfeed2"},
        ///         FallbackSources = new[] {"https://www.example.com/fallbacknugetfeed"},
        ///         Proxy = "exampleproxy",
        ///         NoCache = true,
        ///         Packages = "./packages",
        ///         IgnoreFailedSources = true,
        ///         Quiet = true,
        ///         Parallel = true,
        ///         Locked = DNULocked.Lock,
        ///         Runtimes = new[] {"runtime1", "runtime2"}
        ///     };
        ///
        ///     DNURestore(settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Common.Tools.DNU.Restore")]
        [Obsolete("Please use DotNetCoreRestore(DotNetCoreRestoreSettings) instead.", false)]
        public static void DNURestore(this ICakeContext context, DNURestoreSettings settings)
        {
            context.DNURestore(null, settings);
        }

        /// <summary>
        /// Restore all NuGet Packages in the specified path with settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file to restore.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var settings = new DNURestoreSettings
        ///     {
        ///         Sources = new[] {"https://www.example.com/nugetfeed", "https://www.example.com/nugetfeed2"},
        ///         FallbackSources = new[] {"https://www.example.com/fallbacknugetfeed"},
        ///         Proxy = "exampleproxy",
        ///         NoCache = true,
        ///         Packages = "./packages",
        ///         IgnoreFailedSources = true,
        ///         Quiet = true,
        ///         Parallel = true,
        ///         Locked = DNULocked.Lock,
        ///         Runtimes = new[] {"runtime1", "runtime2"}
        ///     };
        ///
        ///     var projects = GetFiles("./src/**/project.json");
        ///     foreach(var project in projects)
        ///     {
        ///         DNURestore(project, settings);
        ///     }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Common.Tools.DNU.Restore")]
        [Obsolete("Please use DotNetCoreRestore(string, DotNetCoreRestoreSettings) instead.", false)]
        public static void DNURestore(this ICakeContext context, FilePath filePath, DNURestoreSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (settings == null)
            {
                settings = new DNURestoreSettings();
            }

            var restorer = new DNURestorer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            restorer.Restore(filePath, settings);
        }

        /// <summary>
        /// Build all projects.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="path">The projects path.</param>
        /// <example>
        /// <code>
        ///     DNUBuild("./src/*");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Build")]
        [CakeNamespaceImport("Cake.Common.Tools.DNU.Build")]
        [Obsolete("Please use DotNetCoreBuild(string) instead.", false)]
        public static void DNUBuild(this ICakeContext context, string path)
        {
            context.DNUBuild(path, null);
        }

        /// <summary>
        /// Build all projects.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="path">The projects path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var settings = new DNUBuildSettings
        ///     {
        ///         Frameworks = new[] { "dnx451", "dnxcore50" },
        ///         Configurations = new[] { "Debug", "Release" },
        ///         OutputDirectory = "./artifacts/",
        ///         Quiet = true
        ///     };
        ///
        ///     DNUBuild("./src/*", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Build")]
        [CakeNamespaceImport("Cake.Common.Tools.DNU.Build")]
        [Obsolete("Please use DotNetCoreBuild(string, DotNetCoreBuildSettings) instead.", false)]
        public static void DNUBuild(this ICakeContext context, string path, DNUBuildSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (settings == null)
            {
                settings = new DNUBuildSettings();
            }

            var restorer = new DNUBuilder(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            restorer.Build(path, settings);
        }

        /// <summary>
        /// Pack all projects.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="path">The projects path.</param>
        /// <example>
        /// <code>
        ///     DNUPack("./src/*");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Pack")]
        [CakeNamespaceImport("Cake.Common.Tools.DNU.Pack")]
        [Obsolete("Please use DotNetCorePack(string) instead.", false)]
        public static void DNUPack(this ICakeContext context, string path)
        {
            context.DNUPack(path, null);
        }

        /// <summary>
        /// Pack all projects.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="path">The projects path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var settings = new DNUPackSettings
        ///     {
        ///         Frameworks = new[] { "dnx451", "dnxcore50" },
        ///         Configurations = new[] { "Debug", "Release" },
        ///         OutputDirectory = "./artifacts/",
        ///         Quiet = true
        ///     };
        ///
        ///     DNUPack("./src/*", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Pack")]
        [CakeNamespaceImport("Cake.Common.Tools.DNU.Pack")]
        [Obsolete("Please use DotNetCorePack(string, DotNetCorePackSettings) instead.", false)]
        public static void DNUPack(this ICakeContext context, string path, DNUPackSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (settings == null)
            {
                settings = new DNUPackSettings();
            }

            var restorer = new DNUPacker(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            restorer.Pack(path, settings);
        }
    }
}
