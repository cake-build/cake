// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Build.AppVeyor
{
    /// <summary>
    /// AppVeyor upload artifacts settings.
    /// </summary>
    public class AppVeyorUploadArtifactsSettings
    {
        /// <summary>
        /// Gets or sets a value indicating the type of artifact being uploaded to AppVeyor.
        /// </summary>
        public AppVeyorUploadArtifactType ArtifactType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating a deployment name to set for the uploaded artifact.
        /// </summary>
        public string DeploymentName { get; set; }

        /// <summary>
        /// Sets the type of artifact being uploaded to AppVeyor.
        /// </summary>
        /// <param name="type">The type of artifact being uploaded.</param>
        /// <returns>The settings.</returns>
        public AppVeyorUploadArtifactsSettings SetArtifactType(AppVeyorUploadArtifactType type)
        {
            ArtifactType = type;
            return this;
        }

        /// <summary>
        /// Sets the deployment name.
        /// </summary>
        /// <param name="deploymentName">The deployment name to attach to the artifact, required when using the AppVeyor deployment agent.  <paramref name="deploymentName"/> should not have any spaces.</param>
        /// <returns>The settings.</returns>
        public AppVeyorUploadArtifactsSettings SetDeploymentName(string deploymentName)
        {
            DeploymentName = deploymentName;
            return this;
        }
    }
}