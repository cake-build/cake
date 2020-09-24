// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// Allows you to query your Octopus Deploy server deployment history.
    /// </summary>
    public class OctopusDeployDeploymentQuerier : OctopusDeployTool<OctopusDeploymentQuerySettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OctopusDeployDeploymentQuerier"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public OctopusDeployDeploymentQuerier(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Pushes the specified packages to Octopus Deploy internal repository.
        /// </summary>
        /// <param name="server">The Octopus server URL.</param>
        /// <param name="apiKey">The user's API key.</param>
        /// <param name="querySettings">The query.</param>
        /// <returns>A list of Octopus Deployments.</returns>
        public IEnumerable<OctopusDeployment> QueryOctopusDeployments(string server, string apiKey, OctopusDeploymentQuerySettings querySettings)
        {
            if (querySettings == null)
            {
                throw new ArgumentNullException(nameof(querySettings));
            }
            if (string.IsNullOrEmpty(server))
            {
                throw new ArgumentException("No server specified.", nameof(server));
            }
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException("No API key specified.", nameof(apiKey));
            }
            if (querySettings.Count < 1)
            {
                throw new ArgumentOutOfRangeException("Query must return at least one result", nameof(querySettings.Count));
            }

            var builder = new OctopusDeploymentQueryArgumentBuilder(server, apiKey, Environment, querySettings);
            var process = RunProcess(querySettings, builder.Get());

            if (querySettings.ToolTimeout.HasValue)
            {
                if (!process.WaitForExit((int)querySettings.ToolTimeout.Value.TotalMilliseconds))
                {
                    const string message = "Tool timeout ({0}): {1}";
                    throw new TimeoutException(string.Format(CultureInfo.InvariantCulture, message, querySettings.ToolTimeout.Value, GetToolName()));
                }
            }
            else
            {
                process.WaitForExit();
            }

            ProcessExitCode(process.GetExitCode());
            var parser = new DeploymentQueryResultParser();
            return parser.ParseResults(process.GetStandardOutput());
        }
    }
}
