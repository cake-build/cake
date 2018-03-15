// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.OctopusDeploy;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class OctopusDeploymentQueryierFixture : ToolFixture<OctopusDeploymentQuerySettings>
    {
        internal string Server { get; set; }

        internal string ApiKey { get; set; }

        public OctopusDeploymentQueryierFixture()
            : base("Octo.exe")
        {
            Server = "http://octopus";
            ApiKey = "API-12345";
        }

        public DeploymentQueryResultParser Parser { get; set; } = new DeploymentQueryResultParser();

        protected override void RunTool()
        {
            var tool = new OctopusDeployDeploymentQuerier(FileSystem, Environment, ProcessRunner, Tools);
            var results = tool.QueryOctopusDeployments(Server, ApiKey, Settings);
        }
    }
}