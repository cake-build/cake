// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Solution.Project
{
    /// <summary>
    /// Contains functionality related to MSBuild project files.
    /// </summary>
    [CakeAliasCategory("MSBuild Resource")]
    public static class ProjectAliases
    {
        /// <summary>
        /// Parses project information from project file
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectPath">The project file path.</param>
        /// <returns>A parsed project.</returns>
        /// <example>
        /// <code>
        /// var parsedProject = ParseProject("./src/Cake/Cake.csproj");
        /// Information(
        ///     @"    Parsed project file:
        ///     Configuration         : {0}
        ///     Platform              : {1}
        ///     OutputType            : {2}
        ///     RootNameSpace         : {3}
        ///     AssemblyName          : {4}
        ///     TargetFrameworkVersion: {5}
        ///     Files                 : {6}",
        ///     parsedProject.Configuration,
        ///     parsedProject.Platform,
        ///     parsedProject.OutputType,
        ///     parsedProject.RootNameSpace,
        ///     parsedProject.AssemblyName,
        ///     parsedProject.TargetFrameworkVersion,
        ///     string.Concat(
        ///         parsedProject
        ///             .Files
        ///             .Select(
        ///                 file=>  string.Format(
        ///                             "\r\n            {0}",
        ///                             file.FilePath
        ///                         )
        ///             )
        ///     )
        /// );
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static ProjectParserResult ParseProject(this ICakeContext context, FilePath projectPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var parser = new ProjectParser(context.FileSystem, context.Environment);
            return parser.Parse(projectPath);
        }
    }
}
