// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using Cake.Common.Tools.DotNet.Format;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.List
{
    /// <summary>
    /// 'dotnet list' commands
    /// </summary>
    public sealed class DotNetList : DotNetTool<DotNetListSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetList" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetList(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Given a project, outputs all project references
        /// </summary>
        /// <param name="project">The target project path.</param>
        /// <param name="settings">The settings.</param>
        public string[] ListProjectReferences(string project, DotNetListSettings settings)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var references = Array.Empty<string>();
            Run(settings, GetListProjectReferencesArguments(project, settings),
                new ProcessSettings { RedirectStandardOutput = true },
                process =>
                {
                    var projDir = new FileInfo(project).DirectoryName;

                    references =
                        process.GetStandardOutput().Skip(2)
                            .Select(p =>
                                System.IO.Path.GetFullPath(System.IO.Path.Join(projDir, p)).Replace('/', '\\'))
                            .ToArray();
                });

            return references;
        }

        private ProcessArgumentBuilder GetListProjectReferencesArguments(string project, DotNetListSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("list");

            // Specific path?
            if (project != null)
            {
                builder.AppendQuoted(project);
            }

            builder.Append("reference");

            return builder;
        }
    }
}