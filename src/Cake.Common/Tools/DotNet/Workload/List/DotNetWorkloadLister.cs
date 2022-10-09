// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Workload.List
{
    /// <summary>
    /// .NET workloads lister.
    /// </summary>
    public sealed class DotNetWorkloadLister : DotNetTool<DotNetWorkloadListSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetWorkloadLister" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetWorkloadLister(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Lists all installed workloads.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The list of installed workloads.</returns>
        public IEnumerable<DotNetWorkloadListItem> List(DotNetWorkloadListSettings settings)
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
            RunCommand(settings, GetArguments(settings), processSettings,
                process => result = process.GetStandardOutput());

            return ParseResult(result).ToList();
        }

        private ProcessArgumentBuilder GetArguments(DotNetWorkloadListSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("workload list");

            return builder;
        }

        private static IEnumerable<DotNetWorkloadListItem> ParseResult(IEnumerable<string> result)
        {
            bool first = true;
            int manifestIndex = -1;
            int sourceIndex = -1;
            foreach (var line in result)
            {
                if (first)
                {
                    if (line?.StartsWith("Installed Workload Ids") == true
                        && (manifestIndex = line?.IndexOf("Manifest Version") ?? -1) > 22
                        && (sourceIndex = line?.IndexOf("Installation Source") ?? -1) > 39)
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

                yield return new DotNetWorkloadListItem(
                    string.Concat(trimmedLine.Take(manifestIndex)).TrimEnd(),
                    string.Concat(trimmedLine.Take(sourceIndex).Skip(manifestIndex)).TrimEnd(),
                    string.Concat(trimmedLine.Skip(sourceIndex)));
            }
        }
    }
}
