﻿using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// Contains functionality for working with Octopus Deploy.
    /// </summary>
    [CakeAliasCategory("Octopus Deploy")]
    public static class OctopusDeployAliases
    {
        /// <summary>
        /// Creates a release for the specified Octopus Deploy Project.
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="projectName">The name of the project.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void OctoCreateRelease(this ICakeContext context, string projectName, CreateReleaseSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var packer = new OctopusDeployReleaseCreator(context.FileSystem, context.Environment, context.Globber,
                context.ProcessRunner);
            packer.CreateRelease(projectName, settings);
        }
    }
}
