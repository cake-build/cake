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
        /// Lists .NET tools with <c>dotnet tool list</c>.
        /// </summary>
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-list">dotnet tool list</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetToolList();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolList(this ICakeContext context)
        {
            context.DotNetToolList(null, null);
        }

        /// <inheritdoc cref="DotNetToolList(ICakeContext)" />
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-list">dotnet tool list</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotNetToolList(new DotNetToolListSettings
        /// {
        ///     InstallationScope = DotNetToolInstallationScope.Local,
        ///     WorkingDirectory = "./src"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolList(this ICakeContext context, DotNetToolListSettings settings)
        {
            context.DotNetToolList(null, settings);
        }

        /// <summary>
        /// Lists a .NET tool package with <c>dotnet tool list</c>.
        /// </summary>
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-list">dotnet tool list</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The package ID to list.</param>
        /// <example>
        /// <code>
        /// DotNetToolList("dotnetsay");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolList(this ICakeContext context, string packageId)
        {
            context.DotNetToolList(packageId, null);
        }

        /// <inheritdoc cref="DotNetToolList(ICakeContext,string)" />
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-list">dotnet tool list</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The package ID to list.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotNetToolList("DPI", new DotNetToolListSettings
        /// {
        ///     InstallationScope = DotNetToolInstallationScope.Local,
        ///     WorkingDirectory = "./src"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolList(this ICakeContext context, string packageId, DotNetToolListSettings settings)
        {
            settings ??= new DotNetToolListSettings();

            var arguments = new ProcessArgumentBuilder().Append("list");
            AppendOptionalArgument(arguments, packageId);
            AppendToolListSettings(arguments, settings, context.Configuration, context.Environment);
            RunDotNetToolCommandWithoutVerbosity(context, null, arguments, settings);
        }
    }
}
