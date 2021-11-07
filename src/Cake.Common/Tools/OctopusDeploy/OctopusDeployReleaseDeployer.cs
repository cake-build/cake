// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// The Octopus Deploy Release Deploy runner. This class facilitates deploying existing releases in Octopus Deploy.
    /// </summary>
    public sealed class OctopusDeployReleaseDeployer : OctopusDeployTool<OctopusDeployReleaseDeploymentSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OctopusDeployReleaseDeployer"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public OctopusDeployReleaseDeployer(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Requests a deployment of a specified release to an environment.
        /// </summary>
        /// <param name="server">Octopus Server URL.</param>
        /// <param name="apiKey">The user's API key.</param>
        /// <param name="projectName">Name of the target project.</param>
        /// <param name="deployTo">Environment to deploy to, e.g., Production.</param>
        /// <param name="releaseNumber">Release number to be deployed to.</param>
        /// <param name="settings">Settings for the deployment.</param>
        public void DeployRelease(string server, string apiKey, string projectName, string[] deployTo, string releaseNumber, OctopusDeployReleaseDeploymentSettings settings)
        {
            if (String.IsNullOrEmpty(server))
            {
                throw new ArgumentNullException(nameof(server));
            }

            if (String.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            if (String.IsNullOrEmpty(projectName))
            {
                throw new ArgumentNullException(nameof(projectName));
            }

            if (deployTo == null || deployTo.Length == 0 || deployTo.Any(environment => string.IsNullOrEmpty(environment)))
            {
                throw new ArgumentNullException(nameof(deployTo));
            }

            if (String.IsNullOrEmpty(releaseNumber))
            {
                throw new ArgumentNullException(nameof(releaseNumber));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var argumentBuilder = new DeployReleaseArgumentBuilder(server, apiKey, projectName, deployTo, releaseNumber, settings, Environment);
            Run(settings, argumentBuilder.Get());
        }
    }
}
