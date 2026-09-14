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
        /// Executes a .NET tool package with <c>dotnet tool exec</c>.
        /// </summary>
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-exec">dotnet tool exec</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The package ID to execute. Use the <c>package@version</c> syntax to request a specific version.</param>
        /// <example>
        /// <code>
        /// DotNetToolExecute("DPI");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolExecute(this ICakeContext context, string packageId)
        {
            context.DotNetToolExecute(packageId, (ProcessArgumentBuilder)null, null);
        }

        /// <inheritdoc cref="DotNetToolExecute(ICakeContext,string)" />
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-exec">dotnet tool exec</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The package ID to execute. Use the <c>package@version</c> syntax to request a specific version.</param>
        /// <param name="arguments">The arguments forwarded to the tool.</param>
        /// <example>
        /// <code>
        /// var arguments = new ProcessArgumentBuilder().Append("--version");
        /// DotNetToolExecute("DPI", arguments);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolExecute(this ICakeContext context, string packageId, ProcessArgumentBuilder arguments)
        {
            context.DotNetToolExecute(packageId, arguments, null);
        }

        /// <inheritdoc cref="DotNetToolExecute(ICakeContext,string)" />
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-exec">dotnet tool exec</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The package ID to execute. Use the <c>package@version</c> syntax to request a specific version.</param>
        /// <param name="arguments">The arguments forwarded to the tool.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var arguments = new ProcessArgumentBuilder().Append("--version");
        /// DotNetToolExecute("DPI", arguments, new DotNetToolExecuteSettings
        /// {
        ///     WorkingDirectory = "./src"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolExecute(this ICakeContext context, string packageId, ProcessArgumentBuilder arguments, DotNetToolExecuteSettings settings)
        {
            settings ??= new DotNetToolExecuteSettings();

            var dotnetArguments = new ProcessArgumentBuilder().Append("exec");
            AppendRequiredArgument(dotnetArguments, packageId, nameof(packageId));
            AppendToolExecuteSettings(dotnetArguments, settings, context.Environment);
            RunDotNetToolCommandWithForwardedArguments(context, null, dotnetArguments, arguments, settings);
        }

        /// <inheritdoc cref="DotNetToolExecute(ICakeContext,string)" />
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-exec">dotnet tool exec</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The package ID to execute. Use the <c>package@version</c> syntax to request a specific version.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotNetToolExecute("DPI", new DotNetToolExecuteSettings
        /// {
        ///     WorkingDirectory = "./src"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolExecute(this ICakeContext context, string packageId, DotNetToolExecuteSettings settings)
        {
            context.DotNetToolExecute(packageId, (ProcessArgumentBuilder)null, settings);
        }
    }
}
