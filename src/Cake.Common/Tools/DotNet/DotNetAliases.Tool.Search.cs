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
        /// Searches for .NET tool packages with <c>dotnet tool search</c>.
        /// </summary>
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-search">dotnet tool search</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <example>
        /// <code>
        /// DotNetToolSearch("cake");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolSearch(this ICakeContext context, string searchTerm)
        {
            context.DotNetToolSearch(searchTerm, null);
        }

        /// <inheritdoc cref="DotNetToolSearch(ICakeContext,string)" />
        /// <remarks>For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-search">dotnet tool search</see>.</remarks>
        /// <param name="context">The context.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotNetToolSearch("DPI", new DotNetToolSearchSettings
        /// {
        ///     WorkingDirectory = "./src"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Tool")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Tool")]
        public static void DotNetToolSearch(this ICakeContext context, string searchTerm, DotNetToolSearchSettings settings)
        {
            settings ??= new DotNetToolSearchSettings();

            var arguments = new ProcessArgumentBuilder().Append("search");
            AppendRequiredArgument(arguments, searchTerm, nameof(searchTerm));
            AppendToolSearchSettings(arguments, settings);
            RunDotNetToolCommandWithoutVerbosity(context, null, arguments, settings);
        }
    }
}
