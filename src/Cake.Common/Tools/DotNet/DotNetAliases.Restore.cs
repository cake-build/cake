// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNet.Restore;
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
        /// Restore all NuGet Packages.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetRestore();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Restore")]
        public static void DotNetRestore(this ICakeContext context)
        {
            context.DotNetRestore(null, null);
        }

        /// <summary>
        /// Restore all NuGet Packages in the specified path.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">Path to the project file to restore.</param>
        /// <example>
        /// <code>
        /// DotNetRestore("./src/MyProject/MyProject.csproj");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Restore")]
        public static void DotNetRestore(this ICakeContext context, string root)
        {
            context.DotNetRestore(root, null);
        }

        /// <summary>
        /// Restore all NuGet Packages with the settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetRestoreSettings
        /// {
        ///     Sources = new[] {"https://www.example.com/nugetfeed", "https://www.example.com/nugetfeed2"},
        ///     FallbackSources = new[] {"https://www.example.com/fallbacknugetfeed"},
        ///     PackagesDirectory = "./packages",
        ///     DotNetVerbosity.Information,
        ///     DisableParallel = true,
        ///     InferRuntimes = new[] {"runtime1", "runtime2"}
        /// };
        ///
        /// DotNetRestore(settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Restore")]
        public static void DotNetRestore(this ICakeContext context, DotNetRestoreSettings settings)
        {
            context.DotNetRestore(null, settings);
        }

        /// <summary>
        /// Restore all NuGet Packages in the specified path with settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">Path to the project file to restore.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetRestoreSettings
        /// {
        ///     Sources = new[] {"https://www.example.com/nugetfeed", "https://www.example.com/nugetfeed2"},
        ///     FallbackSources = new[] {"https://www.example.com/fallbacknugetfeed"},
        ///     PackagesDirectory = "./packages",
        ///     DotNetVerbosity.Information,
        ///     DisableParallel = true,
        ///     InferRuntimes = new[] {"runtime1", "runtime2"}
        /// };
        ///
        /// DotNetRestore("./src/MyProject/MyProject.csproj", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Restore")]
        public static void DotNetRestore(this ICakeContext context, string root, DotNetRestoreSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetRestoreSettings();
            }

            var restorer = new DotNetRestorer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, context.Log);
            restorer.Restore(root, settings);
        }
    }
}
