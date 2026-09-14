// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        /// Runs a local .NET tool with <c>dotnet tool run</c>.
        /// </summary>
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-run">dotnet tool run</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="commandName">The tool command name to run.</param>
        /// <example>
        /// <code>
        /// DotNetToolRun("dotnetsay");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolRun(this ICakeContext context, string commandName)
        {
            context.DotNetToolRun(commandName, null, null);
        }

        /// <inheritdoc cref="DotNetToolRun(ICakeContext,string)" />
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-run">dotnet tool run</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="commandName">The tool command name to run.</param>
        /// <param name="arguments">The arguments forwarded to the tool.</param>
        /// <example>
        /// <code>
        /// var arguments = new ProcessArgumentBuilder().Append("--version");
        /// DotNetToolRun("DPI", arguments);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolRun(this ICakeContext context, string commandName, ProcessArgumentBuilder arguments)
        {
            context.DotNetToolRun(commandName, arguments, null);
        }

        /// <inheritdoc cref="DotNetToolRun(ICakeContext,string)" />
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-run">dotnet tool run</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="commandName">The tool command name to run.</param>
        /// <param name="arguments">The arguments forwarded to the tool.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotNetToolRun("DPI", "--version", new DotNetToolRunSettings
        /// {
        ///     WorkingDirectory = "./src"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolRun(this ICakeContext context, string commandName, ProcessArgumentBuilder arguments, DotNetToolRunSettings settings)
        {
            settings ??= new DotNetToolRunSettings();

            var dotnetArguments = new ProcessArgumentBuilder().Append("run");
            AppendRequiredArgument(dotnetArguments, commandName, nameof(commandName));
            AppendToolRunSettings(dotnetArguments, settings);
            RunDotNetToolCommandWithForwardedArgumentsWithoutVerbosity(context, null, dotnetArguments, arguments, settings);
        }

        /// <inheritdoc cref="DotNetToolRun(ICakeContext,string)" />
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-run">dotnet tool run</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="commandName">The tool command name to run.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotNetToolRun("DPI", new DotNetToolRunSettings
        /// {
        ///     WorkingDirectory = "./src"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolRun(this ICakeContext context, string commandName, DotNetToolRunSettings settings)
        {
            context.DotNetToolRun(commandName, null, settings);
        }
    }
}
