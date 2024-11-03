// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Sln.List
{
    /// <summary>
    /// .NET projects lister.
    /// </summary>
    public sealed class DotNetSlnLister : DotNetTool<DotNetSlnListSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetSlnLister" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetSlnLister(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Lists all projects in a solution file.
        /// </summary>
        /// <param name="solution">The solution file to use. If not specified, the command searches the current directory for one. If it finds no solution file or multiple solution files, the command fails.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The list of project-to-project references.</returns>
        public IEnumerable<string> List(string solution, DotNetSlnListSettings settings)
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
            RunCommand(settings, GetArguments(solution, settings), processSettings,
                process => result = process.GetStandardOutput());

            return ParseResult(result).ToList();
        }

        private ProcessArgumentBuilder GetArguments(string solution, DotNetSlnListSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("sln");

            // Solution path
            if (!string.IsNullOrWhiteSpace(solution))
            {
                builder.AppendQuoted(solution);
            }

            builder.Append("list");

            return builder;
        }

        private static IEnumerable<string> ParseResult(IEnumerable<string> result)
        {
            bool first = true;
            foreach (var line in result)
            {
                if (first)
                {
                    if (line?.StartsWith("Project(s)") == true)
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
