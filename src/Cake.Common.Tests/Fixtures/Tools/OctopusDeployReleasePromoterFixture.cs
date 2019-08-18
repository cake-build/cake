// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.OctopusDeploy;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    public sealed class OctopusDeployReleasePromoterFixture : ToolFixture<OctopusDeployPromoteReleaseSettings>
    {
        internal string Server { get; set; }

        internal string ApiKey { get; set; }

        internal string Project { get; set; }

        internal string DeployFrom { get; set; }

        internal string DeployTo { get; set; }

        public OctopusDeployReleasePromoterFixture()
            : base("Octo.exe")
        {
            Server = "http://octopus";
            ApiKey = "API-12345";
            Project = "MyProject";
            DeployFrom = "Testing";
            DeployTo = "Staging";
        }

        protected override void RunTool()
        {
            var tool = new OctopusDeployReleasePromoter(FileSystem, Environment, ProcessRunner, Tools);
            tool.PromoteRelease(Server, ApiKey, Project, DeployFrom, DeployTo, Settings);
        }
    }
}
