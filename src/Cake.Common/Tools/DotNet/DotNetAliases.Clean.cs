// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNet.Clean;
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
        /// Cleans a project's output.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project's path.</param>
        /// <example>
        /// <code>
        /// DotNetClean("./src/project");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Clean")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Clean")]
        public static void DotNetClean(this ICakeContext context, string project)
        {
            context.DotNetClean(project, null);
        }

        /// <summary>
        /// Cleans a project's output.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The projects path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetCleanSettings
        /// {
        ///     Framework = "netcoreapp2.0",
        ///     Configuration = "Debug",
        ///     OutputDirectory = "./artifacts/"
        /// };
        ///
        /// DotNetClean("./src/project", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Clean")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Clean")]
        public static void DotNetClean(this ICakeContext context, string project, DotNetCleanSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetCleanSettings();
            }

            var cleaner = new DotNetCleaner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            cleaner.Clean(project, settings);
        }
    }
}
