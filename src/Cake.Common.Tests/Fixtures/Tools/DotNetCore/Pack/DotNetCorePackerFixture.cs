// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Pack;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Pack
{
    internal sealed class DotNetPackFixture : DotNetFixture<DotNetPackSettings>
    {
        public string Project { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetPacker(FileSystem, Environment, ProcessRunner, Tools);
            tool.Pack(Project, Settings);
        }
    }
}