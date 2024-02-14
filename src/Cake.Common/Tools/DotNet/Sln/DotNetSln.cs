// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System;
using System.IO;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Sln
{
    /// <summary>
    /// 'dotnet sln' commands
    /// </summary>
    public sealed class DotNetSln : DotNetTool<DotNetSlnSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetSln" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetSln(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        public void Add(string? solution, string project, string? solutionFolder, DotNetSlnSettings settings)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            Run(settings, GetAddArguments(solution, project, solutionFolder, settings));
        }

        private ProcessArgumentBuilder GetAddArguments(string? solution, string project, string? solutionFolder,
            DotNetSlnSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("sln");

            // Specific path?
            if (solution != null)
            {
                builder.AppendQuoted(solution);
            }

            builder.Append("add");

            if (solutionFolder != null)
            {
                builder.Append("-s");
                builder.AppendQuoted(solutionFolder);
            }
            else
            {
                builder.Append("--in-root");
            }

            builder.AppendQuoted(project);

            return builder;
        }

        public void Remove(string? solution, string project, DotNetSlnSettings settings)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            Run(settings, GetRemoveArguments(solution, project, settings));
        }

        private ProcessArgumentBuilder GetRemoveArguments(string? solution, string project, DotNetSlnSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("sln");

            // Specific path?
            if (solution != null)
            {
                builder.AppendQuoted(solution);
            }

            builder.Append("remove");

            builder.AppendQuoted(project);

            return builder;
        }

        /// <summary>
        /// Cleans the project's output using the specified path and settings.
        /// </summary>
        /// <param name="project">The target project path.</param>
        /// <param name="settings">The settings.</param>
        public string[] List(string? solution, DotNetSlnSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            string[] projects = Array.Empty<string>();
            Run(settings, GetListArguments(solution, settings), new ProcessSettings { RedirectStandardOutput = true },
                process =>
                {
                    var slnDir = solution is null
                        ? _environment.WorkingDirectory
                        : new FileInfo(solution).DirectoryName;

                    projects =
                        process.GetStandardOutput().Skip(2).Select(p =>
                                System.IO.Path.GetFullPath(System.IO.Path.Join(slnDir.ToString(), p))
                                    .Replace('/', '\\'))
                            .ToArray();
                });

            return projects;
        }

        private ProcessArgumentBuilder GetListArguments(string? solution, DotNetSlnSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("sln");

            // Specific path?
            if (solution != null)
            {
                builder.AppendQuoted(solution);
            }

            builder.Append("list");

            return builder;
        }
    }
}