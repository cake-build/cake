// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Reference.List
{
    /// <summary>
    /// .NET reference lister.
    /// </summary>
    public sealed class DotNetReferenceLister : DotNetTool<DotNetReferenceListSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetReferenceLister" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetReferenceLister(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Lists project-to-project references.
        /// </summary>
        /// <param name="project">The target project file path. If not specified, the command searches the current directory for one.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The list of project-to-project references.</returns>
        public IEnumerable<string> List(string project, DotNetReferenceListSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var processSettings = new ProcessSettings
            {
                RedirectStandardOutput = true
            };

            IEnumerable<string> result = null;
            RunCommand(settings, GetArguments(project, settings), processSettings,
                process => result = process.GetStandardOutput());

            return ParseResult(result).ToList();
        }

        private ProcessArgumentBuilder GetArguments(string project, DotNetReferenceListSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("list");

            // Project path
            if (!string.IsNullOrWhiteSpace(project))
            {
                builder.AppendQuoted(project);
            }

            builder.Append("reference");

            return builder;
        }

        private static IEnumerable<string> ParseResult(IEnumerable<string> result)
        {
            bool first = true;
            foreach (var line in result)
            {
                if (first)
                {
                    if (line?.StartsWith("There are no Project to Project references") == true)
                    {
                        yield break;
                    }

                    if (line?.StartsWith("Project reference(s)") == true)
                    {
                        first = false;
                    }
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                var trimmedLine = line.Trim();

                if (trimmedLine.Trim().All(c => c == '-'))
                {
                    continue;
                }

                yield return trimmedLine;
            }
        }
    }
}
