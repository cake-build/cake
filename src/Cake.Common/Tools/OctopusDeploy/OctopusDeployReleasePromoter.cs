// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// The Octopus Deploy Promote Release runner. This class facilitates promoting existing releases in Octopus Deploy.
    /// </summary>
    public sealed class OctopusDeployReleasePromoter : OctopusDeployTool<OctopusDeployPromoteReleaseSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OctopusDeployReleasePromoter"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public OctopusDeployReleasePromoter(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Requests a promotion of a specified release to an environment.
        /// </summary>
        /// <param name="server">Octopus Server URL.</param>
        /// <param name="apiKey">The user's API key.</param>
        /// <param name="projectName">Name of the target project.</param>
        /// <param name="deployFrom">Environment to promote from, e.g., Staging.</param>
        /// <param name="deployTo">Environment to promote to, e.g., Production.</param>
        /// <param name="settings">Settings for the deployment.</param>
        public void PromoteRelease(string server, string apiKey, string projectName, string deployFrom, string deployTo, OctopusDeployPromoteReleaseSettings settings)
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

            if (String.IsNullOrEmpty(deployFrom))
            {
                throw new ArgumentNullException(nameof(deployFrom));
            }

            if (String.IsNullOrEmpty(deployTo))
            {
                throw new ArgumentNullException(nameof(deployTo));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var argumentBuilder = new PromoteReleaseArgumentBuilder(server, apiKey, projectName, deployFrom, deployTo, settings, Environment);
            Run(settings, argumentBuilder.Get());
        }
    }
}
