// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.OctopusDeploy
{
    internal class OctopusDeploymentQueryArgumentBuilder : OctopusDeployArgumentBuilder<OctopusDeploymentQuerySettings>
    {
        public OctopusDeploymentQueryArgumentBuilder(string server, string apiKey, ICakeEnvironment environment, OctopusDeploymentQuerySettings settings) : base(server, apiKey, environment, settings)
        {
        }

        public ProcessArgumentBuilder Get()
        {
            AppendPackageArguments();
            AppendCommonArguments();
            return Builder;
        }

        private void AppendPackageArguments()
        {
            Builder.Append("list-deployments");

            if (!string.IsNullOrEmpty(Settings.EnvironmentName))
            {
                Builder.Append("--environment \"{0}\"", Settings.EnvironmentName);
            }

            if (!string.IsNullOrEmpty(Settings.ProjectName))
            {
                Builder.Append("--project \"{0}\"", Settings.ProjectName);
            }

            if (!string.IsNullOrEmpty(Settings.TenantName))
            {
                Builder.Append("--tenant \"{0}\"", Settings.TenantName);
            }

            Builder.Append("--number {0}", Settings.Count);
        }
    }
}
