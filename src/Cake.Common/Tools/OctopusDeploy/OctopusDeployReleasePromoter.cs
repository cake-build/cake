// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// The Octopus Deploy Promote Release runner. This class facilitates promoting existing releases in Octopus Deploy.
    /// </summary>
    public sealed class OctopusDeployReleasePromoter : Tool<OctopusDeployPromoteReleaseSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="OctopusDeployReleasePromoter"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public OctopusDeployReleasePromoter(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Requests a promotion of a specified release to an environment.
        /// </summary>
        /// <param name="server">Octopus Server URL</param>
        /// <param name="apiKey">The user's API key</param>
        /// <param name="projectName">Name of the target project</param>
        /// <param name="deployFrom">Environment to promote from, e.g., Staging</param>
        /// <param name="deployTo">Environment to promote to, e.g., Production</param>
        /// <param name="settings">Settings for the deployment</param>
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

            var argumentBuilder = new PromoteReleaseArgumentBuilder(server, apiKey, projectName, deployFrom, deployTo, settings, _environment);
            Run(settings, argumentBuilder.Get());
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "Octo";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "Octo.exe" };
        }
    }
}