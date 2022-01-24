// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        public void Search(string searchString)
        {
            var settings = new DotNetWorkloadSearchSettings();
            RunCommand(settings, GetArguments(searchString, settings));
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
    }
}
