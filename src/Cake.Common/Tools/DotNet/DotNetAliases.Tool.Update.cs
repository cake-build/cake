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
        /// Updates .NET tool packages with <c>dotnet tool update</c>.
        /// </summary>
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-update">dotnet tool update</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetToolUpdate();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolUpdate(this ICakeContext context)
        {
            context.DotNetToolUpdate(null, null);
        }

        /// <inheritdoc cref="DotNetToolUpdate(ICakeContext)" />
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-update">dotnet tool update</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolUpdate(this ICakeContext context, DotNetToolUpdateSettings settings)
        {
            context.DotNetToolUpdate(null, settings);
        }

        /// <summary>
        /// Updates a .NET tool package with <c>dotnet tool update</c>.
        /// </summary>
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-update">dotnet tool update</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The package ID to update.</param>
        /// <example>
        /// <code>
        /// DotNetToolUpdate("dotnetsay");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolUpdate(this ICakeContext context, string packageId)
        {
            context.DotNetToolUpdate(packageId, null);
        }

        /// <inheritdoc cref="DotNetToolUpdate(ICakeContext,string)" />
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-update">dotnet tool update</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The package ID to update.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotNetToolUpdate("DPI", new DotNetToolUpdateSettings
        /// {
        ///     WorkingDirectory = "./src"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolUpdate(this ICakeContext context, string packageId, DotNetToolUpdateSettings settings)
        {
            settings ??= new DotNetToolUpdateSettings();

            var arguments = new ProcessArgumentBuilder().Append("update");
            AppendOptionalArgument(arguments, packageId);
            AppendToolUpdateSettings(arguments, settings, context.Configuration, context.Environment);
            RunDotNetToolCommand(context, null, arguments, settings);
        }
    }
}
