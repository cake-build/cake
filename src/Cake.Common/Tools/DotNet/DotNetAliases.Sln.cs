// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Sln.Add;
using Cake.Common.Tools.DotNet.Sln.List;
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
        /// Lists all projects in a solution file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The list of projects.</returns>
        /// <example>
        /// <code>
        /// var projects = DotNetSlnList();
        ///
        /// foreach (var project in projects)
        /// {
        ///      Information(project);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Sln")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Sln.List")]
        public static IEnumerable<string> DotNetSlnList(this ICakeContext context)
        {
            return context.DotNetSlnList(null);
        }

        /// <summary>
        /// Lists all projects in a solution file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution file to use. If this argument is omitted, the command searches the current directory for one. If it finds no solution file or multiple solution files, the command fails.</param>
        /// <returns>The list of projects.</returns>
        /// <example>
        /// <code>
        /// var projects = DotNetSlnList("./app/app.sln");
        ///
        /// foreach (var project in projects)
        /// {
        ///      Information(project);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Sln")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Sln.List")]
        public static IEnumerable<string> DotNetSlnList(this ICakeContext context, FilePath solution)
        {
            return context.DotNetSlnList(solution, null);
        }

        /// <summary>
        /// Lists all projects in a solution file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution file to use. If this argument is omitted, the command searches the current directory for one. If it finds no solution file or multiple solution files, the command fails.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The list of projects.</returns>
        /// <example>
        /// <code>
        /// var settings = new DotNetSlnListSettings
        /// {
        ///     Verbosity = DotNetVerbosity.Diagnostic
        /// };
        ///
        /// var projects = DotNetSlnList("./app/app.sln");
        ///
        /// foreach (var project in projects)
        /// {
        ///      Information(project);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Sln")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Sln.List")]
        public static IEnumerable<string> DotNetSlnList(this ICakeContext context, FilePath solution, DotNetSlnListSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetSlnListSettings();
            }

            var lister = new DotNetSlnLister(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return lister.List(solution, settings);
        }

        /// <summary>
        /// Adds one or more projects to the solution file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectPath">The path to the project or projects to add to the solution. Glob patterns are supported on Unix/Linux-based systems.</param>
        /// <example>
        /// <code>
        /// DotNetSlnAdd(GetFiles("./*.csproj"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Sln")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Sln.Add")]
        public static void DotNetSlnAdd(this ICakeContext context, IEnumerable<FilePath> projectPath)
        {
            context.DotNetSlnAdd(null, projectPath);
        }

        /// <summary>
        /// Adds one or more projects to the solution file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution file to use. If it is unspecified, the command searches the current directory for one and fails if there are multiple solution files.</param>
        /// <param name="projectPath">The path to the project or projects to add to the solution. Glob patterns are supported on Unix/Linux-based systems.</param>
        /// <example>
        /// <code>
        /// DotNetSlnAdd("app.sln", GetFiles("./*.csproj"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Sln")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Sln.Add")]
        public static void DotNetSlnAdd(this ICakeContext context, FilePath solution, IEnumerable<FilePath> projectPath)
        {
            context.DotNetSlnAdd(solution, projectPath, null);
        }

        /// <summary>
        /// Adds one or more projects to the solution file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectPath">The path to the project or projects to add to the solution. Glob patterns are supported on Unix/Linux-based systems.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetSlnAddSettings
        /// {
        ///     SolutionFolder = "libs/math"
        /// };
        ///
        /// DotNetSlnAdd(GetFiles("./*.csproj"), settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Sln")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Sln.Add")]
        public static void DotNetSlnAdd(this ICakeContext context, IEnumerable<FilePath> projectPath, DotNetSlnAddSettings settings)
        {
            context.DotNetSlnAdd(null, projectPath, settings);
        }

        /// <summary>
        /// Adds one or more projects to the solution file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution file to use. If it is unspecified, the command searches the current directory for one and fails if there are multiple solution files.</param>
        /// <param name="projectPath">The path to the project or projects to add to the solution. Glob patterns are supported on Unix/Linux-based systems.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetSlnAddSettings
        /// {
        ///     SolutionFolder = "libs/math"
        /// };
        ///
        /// DotNetSlnAdd("app.sln", GetFiles("./*.csproj"), settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Sln")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Sln.Add")]
        public static void DotNetSlnAdd(this ICakeContext context, FilePath solution, IEnumerable<FilePath> projectPath, DotNetSlnAddSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetSlnAddSettings();
            }

            var adder = new DotNetSlnAdder(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            adder.Add(solution, projectPath, settings);
        }
    }
}
