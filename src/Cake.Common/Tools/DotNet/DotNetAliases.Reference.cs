// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Reference.Add;
using Cake.Common.Tools.DotNet.Reference.List;
using Cake.Common.Tools.DotNet.Reference.Remove;
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
        /// Adds project-to-project (P2P) references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectReferences">One or more project references to add. Glob patterns are supported on Unix/Linux-based systems.</param>
        /// <example>
        /// <code>
        /// DotNetAddReference(GetFiles("./src/*.csproj"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.Add")]
        public static void DotNetAddReference(this ICakeContext context, IEnumerable<FilePath> projectReferences)
        {
            context.DotNetAddReference(projectReferences, null);
        }

        /// <summary>
        /// Adds project-to-project (P2P) references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectReferences">One or more project references to add. Glob patterns are supported on Unix/Linux-based systems.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetReferenceAddSettings
        /// {
        ///     Framework = "net8.0"
        /// };
        ///
        /// DotNetAddReference(GetFiles("./src/*.csproj"), settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.Add")]
        public static void DotNetAddReference(this ICakeContext context, IEnumerable<FilePath> projectReferences, DotNetReferenceAddSettings settings)
        {
            context.DotNetAddReference(null, projectReferences, settings);
        }

        /// <summary>
        /// Adds project-to-project (P2P) references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The target project file path. If not specified, the command searches the current directory for one.</param>
        /// <param name="projectReferences">One or more project references to add. Glob patterns are supported on Unix/Linux-based systems.</param>
        /// <example>
        /// <code>
        /// DotNetAddReference("./app/app.csproj", GetFiles("./src/*.csproj"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.Add")]
        public static void DotNetAddReference(this ICakeContext context, string project, IEnumerable<FilePath> projectReferences)
        {
            context.DotNetAddReference(project, projectReferences, null);
        }

        /// <summary>
        /// Adds project-to-project (P2P) references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The target project file path. If not specified, the command searches the current directory for one.</param>
        /// <param name="projectReferences">One or more project references to add. Glob patterns are supported on Unix/Linux-based systems.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetReferenceAddSettings
        /// {
        ///     Framework = "net8.0"
        /// };
        ///
        /// DotNetAddReference("./app/app.csproj", GetFiles("./src/*.csproj"), settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.Add")]
        public static void DotNetAddReference(this ICakeContext context, string project, IEnumerable<FilePath> projectReferences, DotNetReferenceAddSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (settings is null)
            {
                settings = new DotNetReferenceAddSettings();
            }

            var adder = new DotNetReferenceAdder(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            adder.Add(project, projectReferences, settings);
        }

        /// <summary>
        /// Removes project-to-project (P2P) references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectReferences">Project-to-project (P2P) references to remove. You can specify one or multiple projects. Glob patterns are supported on Unix/Linux based terminals.</param>
        /// <example>
        /// <code>
        /// DotNetRemoveReference(GetFiles("./src/*.csproj"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.Remove")]
        public static void DotNetRemoveReference(this ICakeContext context, IEnumerable<FilePath> projectReferences)
        {
            context.DotNetRemoveReference(projectReferences, null);
        }

        /// <summary>
        /// Removes project-to-project (P2P) references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectReferences">Project-to-project (P2P) references to remove. You can specify one or multiple projects. Glob patterns are supported on Unix/Linux based terminals.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetReferenceRemoveSettings
        /// {
        ///     Framework = "net8.0"
        /// };
        ///
        /// DotNetRemoveReference(GetFiles("./src/*.csproj"), settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.Remove")]
        public static void DotNetRemoveReference(this ICakeContext context, IEnumerable<FilePath> projectReferences, DotNetReferenceRemoveSettings settings)
        {
            context.DotNetRemoveReference(null, projectReferences, settings);
        }

        /// <summary>
        /// Removes project-to-project (P2P) references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">Target project file. If not specified, the command searches the current directory for one.</param>
        /// <param name="projectReferences">Project-to-project (P2P) references to remove. You can specify one or multiple projects. Glob patterns are supported on Unix/Linux based terminals.</param>
        /// <example>
        /// <code>
        /// DotNetRemoveReference("./app/app.csproj", GetFiles("./src/*.csproj"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.Remove")]
        public static void DotNetRemoveReference(this ICakeContext context, string project, IEnumerable<FilePath> projectReferences)
        {
            context.DotNetRemoveReference(project, projectReferences, null);
        }

        /// <summary>
        /// Removes project-to-project (P2P) references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">Target project file. If not specified, the command searches the current directory for one.</param>
        /// <param name="projectReferences">Project-to-project (P2P) references to remove. You can specify one or multiple projects. Glob patterns are supported on Unix/Linux based terminals.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetReferenceRemoveSettings
        /// {
        ///     Framework = "net8.0"
        /// };
        ///
        /// DotNetRemoveReference("./app/app.csproj", GetFiles("./src/*.csproj"), settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.Remove")]
        public static void DotNetRemoveReference(this ICakeContext context, string project, IEnumerable<FilePath> projectReferences, DotNetReferenceRemoveSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (settings is null)
            {
                settings = new DotNetReferenceRemoveSettings();
            }

            var remover = new DotNetReferenceRemover(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            remover.Remove(project, projectReferences, settings);
        }

        /// <summary>
        /// Lists project-to-project references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The list of project-to-project references.</returns>
        /// <example>
        /// <code>
        /// var references = DotNetListReference();
        ///
        /// foreach (var reference in references)
        /// {
        ///      Information(reference);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.List")]
        public static IEnumerable<string> DotNetListReference(this ICakeContext context)
        {
            return context.DotNetListReference(null);
        }

        /// <summary>
        /// Lists project-to-project references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project file to operate on. If a file is not specified, the command will search the current directory for one.</param>
        /// <returns>The list of project-to-project references.</returns>
        /// <example>
        /// <code>
        /// var references = DotNetListReference("./app/app.csproj");
        ///
        /// foreach (var reference in references)
        /// {
        ///      Information(reference);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.List")]
        public static IEnumerable<string> DotNetListReference(this ICakeContext context, string project)
        {
            return context.DotNetListReference(project, null);
        }

        /// <summary>
        /// Lists project-to-project references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project file to operate on. If a file is not specified, the command will search the current directory for one.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The list of project-to-project references.</returns>
        /// <example>
        /// <code>
        /// var settings = new DotNetReferenceListSettings
        /// {
        ///     Verbosity = DotNetVerbosity.Diagnostic
        /// };
        ///
        /// var references = DotNetListReference("./app/app.csproj", settings);
        ///
        /// foreach (var reference in references)
        /// {
        ///      Information(reference);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Reference")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Reference.List")]
        public static IEnumerable<string> DotNetListReference(this ICakeContext context, string project, DotNetReferenceListSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (settings is null)
            {
                settings = new DotNetReferenceListSettings();
            }

            var lister = new DotNetReferenceLister(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return lister.List(project, settings);
        }
    }
}
