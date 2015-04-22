using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// Contains functionality for working with Octopus Deploy.
    /// </summary>
    [CakeAliasCategory("Octo")]
    public static class OctopusDeployAliases
    {
        /// <summary>
        /// Creates a release for the specified Octopus Deploy Project.
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="projectName">The name of the project.</param>
        /// <param name="server">The octopus server url.</param>
        /// <param name="apiKey">The user's api key.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void OctoCreateRelease(this ICakeContext context, string projectName, string server, string apiKey, CreateReleaseSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var packer = new OctopusDeployRunner(context.FileSystem, context.Environment, context.Globber,
                context.ProcessRunner);
            packer.CreateRelease(projectName, server, apiKey, settings);
        }
    }
}
