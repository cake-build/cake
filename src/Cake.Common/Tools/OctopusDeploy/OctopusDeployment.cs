// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// An object representing a deployment in Octopus Deploy.
    /// </summary>
    public class OctopusDeployment
    {
        /// <summary>
        /// gets or sets the Name of the Deployment's Project.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// gets or sets the Environment the project was deployed to.
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// gets or sets the Deployment's channel.
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// gets or sets When the deployment was created.
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// gets or sets when the deployment was assembled.
        /// </summary>
        public DateTimeOffset Assembled { get; set; }

        /// <summary>
        /// gets or sets the deployed project version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// gets or sets the list of packages in the deployment.
        /// </summary>
        public string PackageVersions { get; set; }

        /// <summary>
        /// gets or sets the release notes for the deployment (HTML Markup).
        /// </summary>
        public string ReleaseNotesHtml { get; set; }
    }
}
