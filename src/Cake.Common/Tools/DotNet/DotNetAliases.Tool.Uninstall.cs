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
        /// Uninstalls a .NET tool package with <c>dotnet tool uninstall</c>.
        /// </summary>
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-uninstall">dotnet tool uninstall</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The package ID to uninstall.</param>
        /// <example>
        /// <code>
        /// DotNetToolUninstall("dotnetsay");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolUninstall(this ICakeContext context, string packageId)
        {
            context.DotNetToolUninstall(packageId, null);
        }

        /// <inheritdoc cref="DotNetToolUninstall(ICakeContext,string)" />
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-uninstall">dotnet tool uninstall</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="packageId">The package ID to uninstall.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotNetToolUninstall("DPI", new DotNetToolUninstallSettings
        /// {
        ///     WorkingDirectory = "./src"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolUninstall(this ICakeContext context, string packageId, DotNetToolUninstallSettings settings)
        {
            settings ??= new DotNetToolUninstallSettings();

            var arguments = new ProcessArgumentBuilder().Append("uninstall");
            AppendRequiredArgument(arguments, packageId, nameof(packageId));
            AppendToolUninstallSettings(arguments, settings, context.Configuration, context.Environment);
            RunDotNetToolCommandWithoutVerbosity(context, null, arguments, settings);
        }
    }
}
