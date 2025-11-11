// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Sln.Remove
{
    /// <summary>
    /// .NET project remover.
    /// </summary>
    public sealed class DotNetSlnRemover : DotNetTool<DotNetSlnRemoveSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetSlnRemover" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetSlnRemover(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Removes a project or multiple projects from the solution file.
        /// </summary>
        /// <param name="solution">The solution file to use. If it is unspecified, the command searches the current directory for one and fails if there are multiple solution files.</param>
        /// <param name="projectPath">The path to the project or projects to remove from the solution.</param>
        /// <param name="settings">The settings.</param>
        public void Remove(FilePath solution, IEnumerable<FilePath> projectPath, DotNetSlnRemoveSettings settings)
        {
            if (projectPath == null || !projectPath.Any())
            {
                throw new ArgumentNullException(nameof(projectPath));
            }
            ArgumentNullException.ThrowIfNull(settings);

            RunCommand(settings, GetArguments(solution, projectPath, settings));
        }

        private ProcessArgumentBuilder GetArguments(FilePath solution, IEnumerable<FilePath> projectPath, DotNetSlnRemoveSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("sln");

            // Solution path
            if (solution != null)
            {
                builder.AppendQuoted(solution.MakeAbsolute(_environment).FullPath);
            }

            builder.Append("remove");

            // Project path
            foreach (var project in projectPath)
            {
                builder.AppendQuoted(project.MakeAbsolute(_environment).FullPath);
            }

            return builder;
        }
    }
}
