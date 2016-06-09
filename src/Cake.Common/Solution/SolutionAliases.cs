// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Solution
{
    /// <summary>
    /// Contains functionality related to MSBuild solution files.
    /// </summary>
    [CakeAliasCategory("MSBuild Resource")]
    public static class SolutionAliases
    {
        /// <summary>
        /// Parses project information from a solution file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solutionPath">The solution path.</param>
        /// <returns>A parsed solution.</returns>
        /// <example>
        /// <code>
        /// var solutionPath = "./src/Cake.sln";
        /// Information("Parsing {0}", solutionPath);
        /// var parsedSolution = ParseSolution(solutionPath);
        /// foreach(var project in parsedSolution.Projects)
        /// {
        ///     Information(
        ///         @"Solution project file:
        ///     Name: {0}
        ///     Path: {1}
        ///     Id  : {2}
        ///     Type: {3}",
        ///         project.Name,
        ///         project.Path,
        ///         project.Id,
        ///         project.Type
        ///     );
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static SolutionParserResult ParseSolution(this ICakeContext context, FilePath solutionPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var parser = new SolutionParser(context.FileSystem, context.Environment);
            return parser.Parse(solutionPath);
        }
    }
}
