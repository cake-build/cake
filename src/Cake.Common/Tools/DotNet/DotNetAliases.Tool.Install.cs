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
        /// Installs a .NET tool package with <c>dotnet tool install</c>.
        /// </summary>
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-install">dotnet tool install</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The package ID to install.</param>
        /// <example>
        /// <code>
        /// DotNetToolInstall("dotnetsay");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolInstall(this ICakeContext context, string packageId)
        {
            context.DotNetToolInstall(packageId, null);
        }

        /// <inheritdoc cref="DotNetToolInstall(ICakeContext,string)" />
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-install">dotnet tool install</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The package ID to install.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotNetToolInstall("DPI", new DotNetToolInstallSettings
        /// {
        ///     WorkingDirectory = "./src"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolInstall(this ICakeContext context, string packageId, DotNetToolInstallSettings settings)
        {
            settings ??= new DotNetToolInstallSettings();

            var arguments = new ProcessArgumentBuilder().Append("install");
            AppendRequiredArgument(arguments, packageId, nameof(packageId));
            AppendToolInstallSettings(arguments, settings, context.Configuration, context.Environment);
            RunDotNetToolCommand(context, null, arguments, settings);
        }
    }
}
