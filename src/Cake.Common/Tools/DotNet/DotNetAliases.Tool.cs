// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNet.Tool;
using Cake.Core;
using Cake.Core.Annotations;
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
        /// <summary>
        /// Execute an .NET Core Extensibility Tool.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="command">The command to execute.</param>
        /// <example>
        /// <code>
        /// DotNetTool("cake");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetTool(this ICakeContext context, string command)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var arguments = new ProcessArgumentBuilder();
            var settings = new DotNetToolSettings();

            context.DotNetTool(null, command, arguments, settings);
        }

        /// <summary>
        /// Execute an .NET Core Extensibility Tool.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="command">The command to execute.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetToolSettings
        /// {
        ///     DiagnosticOutput = true
        /// };
        ///
        /// DotNetTool("cake", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetTool(this ICakeContext context, string command, DotNetToolSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var arguments = new ProcessArgumentBuilder();

            context.DotNetTool(null, command, arguments, settings);
        }

        /// <summary>
        /// Execute an .NET Core Extensibility Tool.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectPath">The project path.</param>
        /// <param name="command">The command to execute.</param>
        /// <example>
        /// <code>
        /// DotNetTool("./src/project", "xunit", "-xml report.xml");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetTool(this ICakeContext context, FilePath projectPath, string command)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var arguments = new ProcessArgumentBuilder();
            var settings = new DotNetToolSettings();

            context.DotNetTool(projectPath, command, arguments, settings);
        }

        /// <summary>
        /// Execute an .NET Core Extensibility Tool.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectPath">The project path.</param>
        /// <param name="command">The command to execute.</param>
        /// <param name="arguments">The arguments.</param>
        /// <example>
        /// <code>
        /// DotNetTool("./src/project", "xunit", "-xml report.xml");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetTool(this ICakeContext context, FilePath projectPath, string command, ProcessArgumentBuilder arguments)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var settings = new DotNetToolSettings();

            context.DotNetTool(projectPath, command, arguments, settings);
        }

        /// <summary>
        /// Execute an .NET Core Extensibility Tool.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectPath">The project path.</param>
        /// <param name="command">The command to execute.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotNetTool("./src/project", "xunit", "-xml report.xml");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetTool(this ICakeContext context, FilePath projectPath, string command, ProcessArgumentBuilder arguments, DotNetToolSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new DotNetToolRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

            runner.Execute(projectPath, command, arguments, settings);
        }
    }
}
