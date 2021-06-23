// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.OctopusDeploy;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    public sealed class OctopusDeployReleaseDeployerFixture : ToolFixture<OctopusDeployReleaseDeploymentSettings>
    {
        internal string Server { get; set; }

        internal string ApiKey { get; set; }

        internal string Project { get; set; }

        internal string[] DeployTo { get; set; }

        internal string ReleaseNumber { get; set; }

        public OctopusDeployReleaseDeployerFixture()
            : base("Octo.exe")
        {
            Server = "http://octopus";
            ApiKey = "API-12345";
            Project = "MyProject";
            DeployTo = new string[] { "Testing" };
            ReleaseNumber = "0.15.1";
        }

        protected override void RunTool()
        {
            var tool = new OctopusDeployReleaseDeployer(FileSystem, Environment, ProcessRunner, Tools);
            tool.DeployRelease(Server, ApiKey, Project, DeployTo, ReleaseNumber, Settings);
        }
    }
}
