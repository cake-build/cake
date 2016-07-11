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
    /// The Octopus Deploy release creator runner
    /// </summary>
    public sealed class OctopusDeployReleaseCreator : Tool<CreateReleaseSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="OctopusDeployReleaseCreator"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public OctopusDeployReleaseCreator(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Creates a release for the specified project in OctopusDeploy
        /// </summary>
        /// <param name="projectName">The target project name</param>
        /// <param name="settings">The settings</param>
        public void CreateRelease(string projectName, CreateReleaseSettings settings)
        {
            if (projectName == null)
            {
                throw new ArgumentNullException("projectName");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (string.IsNullOrEmpty(settings.Server))
            {
                throw new ArgumentException("No server specified.", "settings");
            }
            if (string.IsNullOrEmpty(settings.ApiKey))
            {
                throw new ArgumentException("No API key specified.", "settings");
            }

            var argumentBuilder = new CreateReleaseArgumentBuilder(projectName, settings, _environment);
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
