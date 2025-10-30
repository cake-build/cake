// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.Tools.DotNet.MSBuild;
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
        /// Builds the specified targets in a project file found in the current working directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetMSBuild();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("MSBuild")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.MSBuild")]
        public static void DotNetMSBuild(this ICakeContext context)
        {
            context.DotNetMSBuild((string)null, (DotNetMSBuildSettings)null);
        }

        /// <summary>
        /// Builds the specified targets in the project file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectOrDirectory">Project file or directory to search for project file.</param>
        /// <example>
        /// <code>
        /// DotNetMSBuild("foobar.proj");
        /// </code>
        /// </example>
        /// <remarks>
        /// If a directory is specified, MSBuild searches that directory for a project file.
        /// </remarks>
        [CakeMethodAlias]
        [CakeAliasCategory("MSBuild")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.MSBuild")]
        public static void DotNetMSBuild(this ICakeContext context, string projectOrDirectory)
        {
            if (string.IsNullOrWhiteSpace(projectOrDirectory))
            {
                throw new ArgumentNullException(nameof(projectOrDirectory));
            }

            context.DotNetMSBuild(projectOrDirectory, null);
        }

        /// <summary>
        /// Builds the specified targets in a project file found in the current working directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetMSBuildSettings
        /// {
        ///     NoLogo = true,
        ///     MaxCpuCount = -1
        /// };
        ///
        /// DotNetMSBuild(settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("MSBuild")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.MSBuild")]
        public static void DotNetMSBuild(this ICakeContext context, DotNetMSBuildSettings settings)
        {
            context.DotNetMSBuild(null, settings);
        }

        /// <summary>
        /// Builds the specified targets in a project file found in the current working directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="standardOutputAction">The action to invoke with the standard output.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetMSBuildSettings
        /// {
        ///     NoLogo = true,
        ///     MaxCpuCount = -1
        /// };
        ///
        /// DotNetMSBuild(settings,
        ///     output => foreach (var line in output) outputBuilder.AppendLine(line));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("MSBuild")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.MSBuild")]
        public static void DotNetMSBuild(this ICakeContext context, DotNetMSBuildSettings settings, Action<IEnumerable<string>> standardOutputAction)
        {
            context.DotNetMSBuild(null, settings, standardOutputAction);
        }

        /// <summary>
        /// Builds the specified targets in the project file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectOrDirectory">Project file or directory to search for project file.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetMSBuildSettings
        /// {
        ///     NoLogo = true,
        ///     MaxCpuCount = -1
        /// };
        ///
        /// DotNetMSBuild("foobar.proj", settings);
        /// </code>
        /// </example>
        /// <remarks>
        /// If a project file is not specified, MSBuild searches the current working directory for a file that has a file
        /// extension that ends in "proj" and uses that file. If a directory is specified, MSBuild searches that directory for a project file.
        /// </remarks>
        [CakeMethodAlias]
        [CakeAliasCategory("MSBuild")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.MSBuild")]
        public static void DotNetMSBuild(this ICakeContext context, string projectOrDirectory, DotNetMSBuildSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (settings is null)
            {
                settings = new DotNetMSBuildSettings();
            }

            var builder = new DotNetMSBuildBuilder(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            builder.Build(projectOrDirectory, settings, null);
        }

        /// <summary>
        /// Builds the specified targets in the project file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectOrDirectory">Project file or directory to search for project file.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="standardOutputAction">The action to invoke with the standard output.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetMSBuildSettings
        /// {
        ///     NoLogo = true,
        ///     MaxCpuCount = -1
        /// };
        ///
        /// DotNetMSBuild("foobar.proj", settings,
        ///     output => foreach (var line in output) outputBuilder.AppendLine(line));
        /// </code>
        /// </example>
        /// <remarks>
        /// If a project file is not specified, MSBuild searches the current working directory for a file that has a file
        /// extension that ends in "proj" and uses that file. If a directory is specified, MSBuild searches that directory for a project file.
        /// </remarks>
        [CakeMethodAlias]
        [CakeAliasCategory("MSBuild")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.MSBuild")]
        public static void DotNetMSBuild(this ICakeContext context, string projectOrDirectory, DotNetMSBuildSettings settings, Action<IEnumerable<string>> standardOutputAction)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (settings is null)
            {
                settings = new DotNetMSBuildSettings();
            }

            var builder = new DotNetMSBuildBuilder(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            builder.Build(projectOrDirectory, settings, standardOutputAction);
        }
    }
}
