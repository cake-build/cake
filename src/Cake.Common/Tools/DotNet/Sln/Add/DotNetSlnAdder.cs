// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Sln.Add
{
    /// <summary>
    /// .NET project adder.
    /// </summary>
    public sealed class DotNetSlnAdder : DotNetTool<DotNetSlnAddSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetSlnAdder" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetSlnAdder(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Adds one or more projects to the solution file.
        /// </summary>
        /// <param name="solution">The solution file to use. If it is unspecified, the command searches the current directory for one and fails if there are multiple solution files.</param>
        /// <param name="projectPath">The path to the project or projects to add to the solution. Glob patterns are supported on Unix/Linux-based systems.</param>
        /// <param name="settings">The settings.</param>
        public void Add(FilePath solution, IEnumerable<FilePath> projectPath, DotNetSlnAddSettings settings)
        {
            if (projectPath == null || !projectPath.Any())
            {
                throw new ArgumentNullException(nameof(projectPath));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (settings.InRoot && settings.SolutionFolder != null)
            {
                throw new ArgumentException("InRoot and SolutionFolder cannot be used together.");
            }

            RunCommand(settings, GetArguments(solution, projectPath, settings));
        }

        private ProcessArgumentBuilder GetArguments(FilePath solution, IEnumerable<FilePath> projectPath, DotNetSlnAddSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("sln");

            // Solution path
            if (solution != null)
            {
                builder.AppendQuoted(solution.MakeAbsolute(_environment).FullPath);
            }

            builder.Append("add");

            // Solution folder
            if (settings.SolutionFolder != null)
            {
                builder.AppendSwitchQuoted("--solution-folder", settings.SolutionFolder.MakeAbsolute(_environment).FullPath);
            }

            // In root
            if (settings.InRoot)
            {
                builder.Append("--in-root");
            }

            // Project path
            foreach (var project in projectPath)
            {
                builder.AppendQuoted(project.MakeAbsolute(_environment).FullPath);
            }

            return builder;
        }
    }
}
