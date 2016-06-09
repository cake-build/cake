// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Solution.Project.Properties
{
    /// <summary>
    /// Contains functionality related to assembly info.
    /// </summary>
    [CakeAliasCategory("Assembly Info")]
    public static class AssemblyInfoAliases
    {
        /// <summary>
        /// Creates an assembly information file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var file = "./SolutionInfo.cs";
        /// var version = "0.0.1";
        /// var buildNo = "123";
        /// var semVersion = string.Concat(version + "-" + buildNo);
        /// CreateAssemblyInfo(file, new AssemblyInfoSettings {
        ///     Product = "SampleProject",
        ///     Version = version,
        ///     FileVersion = version,
        ///     InformationalVersion = semVersion,
        ///     Copyright = string.Format("Copyright (c) Contoso 2014 - {0}", DateTime.Now.Year)
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void CreateAssemblyInfo(this ICakeContext context, FilePath outputPath, AssemblyInfoSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var creator = new AssemblyInfoCreator(context.FileSystem, context.Environment, context.Log);
            creator.Create(outputPath, settings);
        }

        /// <summary>
        /// Parses an existing assembly information file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblyInfoPath">The assembly info path.</param>
        /// <returns>The content of the assembly info file.</returns>
        /// <example>
        /// <code>
        /// var assemblyInfo = ParseAssemblyInfo("./SolutionInfo.cs");
        /// Information("Version: {0}", assemblyInfo.AssemblyVersion);
        /// Information("File version: {0}", assemblyInfo.AssemblyFileVersion);
        /// Information("Informational version: {0}", assemblyInfo.AssemblyInformationalVersion);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static AssemblyInfoParseResult ParseAssemblyInfo(this ICakeContext context, FilePath assemblyInfoPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var parser = new AssemblyInfoParser(context.FileSystem, context.Environment);
            return parser.Parse(assemblyInfoPath);
        }
    }
}
