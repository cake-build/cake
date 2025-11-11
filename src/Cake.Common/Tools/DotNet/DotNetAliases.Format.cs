// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNet.Format;
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
        /// Formats code to match editorconfig settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">The project or solution path.</param>
        /// <example>
        /// <code>
        /// DotNetFormat("./src/project");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Format")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Format")]
        public static void DotNetFormat(this ICakeContext context, string root)
        {
            context.DotNetFormat(root, null);
        }

        /// <summary>
        /// Formats code to match editorconfig settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">The project or solution path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetFormatSettings
        /// {
        ///     NoRestore = true,
        ///     Include = "Program.cs Utility\Logging.cs",
        ///     Severity = DotNetFormatSeverity.Error
        /// };
        ///
        /// DotNetFormat("./src/project", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Format")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Format")]
        public static void DotNetFormat(this ICakeContext context, string root, DotNetFormatSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (settings is null)
            {
                settings = new DotNetFormatSettings();
            }

            var formatter = new DotNetFormatter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            formatter.Format(root, null, settings);
        }

        /// <summary>
        /// Format code to match editorconfig settings for whitespace.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">The project or solution path.</param>
        /// <example>
        /// <code>
        /// DotNetFormatWhitespace("./src/*");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Format")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Format")]
        public static void DotNetFormatWhitespace(this ICakeContext context, string root)
        {
            context.DotNetFormatWhitespace(root, null);
        }

        /// <summary>
        /// Format code to match editorconfig settings for whitespace.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">The project or solution path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetFormatSettings
        /// {
        ///     NoRestore = true,
        ///     Include = "Program.cs Utility\Logging.cs"
        /// };
        ///
        /// DotNetFormatWhitespace("./src/*", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Format")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Format")]
        public static void DotNetFormatWhitespace(this ICakeContext context, string root, DotNetFormatSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (settings == null)
            {
                settings = new DotNetFormatSettings();
            }

            var formatter = new DotNetFormatter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            formatter.Format(root, "whitespace", settings);
        }

        /// <summary>
        /// Format code to match editorconfig settings for code style.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">The project or solution path.</param>
        /// <example>
        /// <code>
        /// DotNetFormatStyle("./src/*");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Format")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Format")]
        public static void DotNetFormatStyle(this ICakeContext context, string root)
        {
            context.DotNetFormatStyle(root, null);
        }

        /// <summary>
        /// Format code to match editorconfig settings for code style.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">The project or solution path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetFormatSettings
        /// {
        ///     NoRestore = true,
        ///     Include = "Program.cs Utility\Logging.cs"
        /// };
        ///
        /// DotNetFormatStyle("./src/*", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Format")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Format")]
        public static void DotNetFormatStyle(this ICakeContext context, string root, DotNetFormatSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (settings == null)
            {
                settings = new DotNetFormatSettings();
            }

            var formatter = new DotNetFormatter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            formatter.Format(root, "style", settings);
        }

        /// <summary>
        /// Format code to match editorconfig settings for analyzers.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project or solution path.</param>
        /// <example>
        /// <code>
        /// DotNetFormatAnalyzers("./src/*");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Format")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Format")]
        public static void DotNetFormatAnalyzers(this ICakeContext context, string project)
        {
            context.DotNetFormatAnalyzers(project, null);
        }

        /// <summary>
        /// Format code to match editorconfig settings for analyzers.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="root">The project or solution path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetFormatSettings
        /// {
        ///     NoRestore = true,
        ///     Include = "Program.cs Utility\Logging.cs"
        /// };
        ///
        /// DotNetFormatAnalyzers("./src/*", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Format")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Format")]
        public static void DotNetFormatAnalyzers(this ICakeContext context, string root, DotNetFormatSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (settings == null)
            {
                settings = new DotNetFormatSettings();
            }

            var formatter = new DotNetFormatter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            formatter.Format(root, "analyzers", settings);
        }
    }
}
