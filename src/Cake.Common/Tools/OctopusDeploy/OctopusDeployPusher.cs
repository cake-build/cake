// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// The Octopus Deploy package push runner
    /// </summary>
    public class OctopusDeployPusher : Tool<OctopusPushSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="OctopusDeployPusher"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public OctopusDeployPusher(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
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

        /// <summary>
        /// Pushes the specified packages to Octopus Deploy internal repository
        /// </summary>
        /// <param name="server">The Octopus server URL</param>
        /// <param name="apiKey">The user's API key</param>
        /// <param name="packagePaths">Paths to the packages to be pushed</param>
        /// <param name="settings">The settings</param>
        public void PushPackage(string server, string apiKey, FilePath[] packagePaths, OctopusPushSettings settings)
        {
            if (packagePaths == null || !packagePaths.Any())
            {
                throw new ArgumentNullException(nameof(packagePaths));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (string.IsNullOrEmpty(server))
            {
                throw new ArgumentException("No server specified.", nameof(settings));
            }
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException("No API key specified.", nameof(settings));
            }

            var builder = new OctopusPushArgumentBuilder(packagePaths, server, apiKey, _environment, settings);
            Run(settings, builder.Get());
        }
    }
}