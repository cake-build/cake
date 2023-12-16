// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Reference.Add
{
    /// <summary>
    /// .NET reference adder.
    /// </summary>
    public sealed class DotNetReferenceAdder : DotNetTool<DotNetReferenceAddSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetReferenceAdder" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetReferenceAdder(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Adds project-to-project (P2P) references.
        /// </summary>
        /// <param name="project">The target project file path. If not specified, the command searches the current directory for one.</param>
        /// <param name="projectReferences">One or more project references to add. Glob patterns are supported on Unix/Linux-based systems.</param>
        /// <param name="settings">The settings.</param>
        public void Add(string project, IEnumerable<FilePath> projectReferences, DotNetReferenceAddSettings settings)
        {
            if (projectReferences == null || !projectReferences.Any())
            {
                throw new ArgumentNullException(nameof(projectReferences));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(project, projectReferences, settings));
        }

        private ProcessArgumentBuilder GetArguments(string project, IEnumerable<FilePath> projectReferences, DotNetReferenceAddSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("add");

            // Project path
            if (!string.IsNullOrWhiteSpace(project))
            {
                builder.AppendQuoted(project);
            }

            // References
            builder.Append("reference");
            foreach (var reference in projectReferences)
            {
                builder.AppendQuoted(reference.MakeAbsolute(_environment).FullPath);
            }

            // Framework
            if (!string.IsNullOrEmpty(settings.Framework))
            {
                builder.AppendSwitch("--framework", settings.Framework);
            }

            // Interactive
            if (settings.Interactive)
            {
                builder.Append("--interactive");
            }

            return builder;
        }
    }
}
