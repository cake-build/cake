// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Reference.Remove
{
    /// <summary>
    /// .NET reference remover.
    /// </summary>
    public sealed class DotNetReferenceRemover : DotNetTool<DotNetReferenceRemoveSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetReferenceRemover" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetReferenceRemover(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Removes project-to-project (P2P) references.
        /// </summary>
        /// <param name="project">The target project file path. If not specified, the command searches the current directory for one.</param>
        /// <param name="projectReferences">One or more project references to add. Glob patterns are supported on Unix/Linux-based systems.</param>
        /// <param name="settings">The settings.</param>
        public void Remove(string project, IEnumerable<FilePath> projectReferences, DotNetReferenceRemoveSettings settings)
        {
            if (projectReferences == null || !projectReferences.Any())
            {
                throw new ArgumentNullException(nameof(projectReferences));
            }
            ArgumentNullException.ThrowIfNull(settings);

            RunCommand(settings, GetArguments(project, projectReferences, settings));
        }

        private ProcessArgumentBuilder GetArguments(string project, IEnumerable<FilePath> projectReferences, DotNetReferenceRemoveSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("remove");

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

            return builder;
        }
    }
}
