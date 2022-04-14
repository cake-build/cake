// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Workload.Search
{
    /// <summary>
    /// .NET workloads searcher.
    /// </summary>
    public sealed class DotNetWorkloadSearcher : DotNetTool<DotNetWorkloadSearchSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetWorkloadSearcher" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetWorkloadSearcher(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Lists the latest available version of the .NET SDK and .NET Runtime, for each feature band.
        /// </summary>
        /// <param name="searchString">The workload ID to search for, or part of it.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The list of available workloads.</returns>
        public IEnumerable<DotNetWorkload> Search(string searchString, DotNetWorkloadSearchSettings settings)
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
            RunCommand(settings, GetArguments(searchString, settings), processSettings,
                process => result = process.GetStandardOutput());

            return ParseResult(result).ToList();
        }

        private ProcessArgumentBuilder GetArguments(string searchString, DotNetWorkloadSearchSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("workload search");

            if (!string.IsNullOrEmpty(searchString))
            {
                builder.Append(searchString);
            }

            return builder;
        }

        private static IEnumerable<DotNetWorkload> ParseResult(IEnumerable<string> result)
        {
            bool first = true;
            int descriptionIndex = -1;
            foreach (var line in result)
            {
                if (first)
                {
                    if (line?.StartsWith("Workload ID") == true
                        && (descriptionIndex = line?.IndexOf("Description") ?? -1) > 11)
                    {
                        first = false;
                    }
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var trimmedLine = line.Trim();

                if (trimmedLine.Trim().All(c => c == '-'))
                {
                    continue;
                }

                yield return new DotNetWorkload(
                    string.Concat(trimmedLine.Take(descriptionIndex)).TrimEnd(),
                    string.Concat(trimmedLine.Skip(descriptionIndex)));
            }
        }
    }
}
