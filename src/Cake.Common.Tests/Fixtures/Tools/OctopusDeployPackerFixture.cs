// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.OctopusDeploy;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class OctopusDeployPackerFixture : ToolFixture<OctopusPackSettings>
    {
        public string Id { get; set; }

        public OctopusDeployPackerFixture()
            : base("Octo.exe")
        {
        }

        protected override void RunTool()
        {
            var tool = new OctopusDeployPacker(FileSystem, Environment, ProcessRunner, Tools);
            tool.Pack(Id, Settings);
        }
    }
}
