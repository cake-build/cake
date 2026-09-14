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
        /// Restores .NET tools with <c>dotnet tool restore</c>.
        /// </summary>
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-restore">dotnet tool restore</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetToolRestore();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolRestore(this ICakeContext context)
        {
            context.DotNetToolRestore(null);
        }

        /// <inheritdoc cref="DotNetToolRestore(ICakeContext)" />
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-restore">dotnet tool restore</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotNetToolRestore(new DotNetToolRestoreSettings
        /// {
        ///     WorkingDirectory = "./src"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolRestore(this ICakeContext context, DotNetToolRestoreSettings settings)
        {
            settings ??= new DotNetToolRestoreSettings();

            var arguments = new ProcessArgumentBuilder().Append("restore");
            AppendToolRestoreSettings(arguments, settings, context.Environment);
            RunDotNetToolCommand(context, null, arguments, settings);
        }
    }
}
