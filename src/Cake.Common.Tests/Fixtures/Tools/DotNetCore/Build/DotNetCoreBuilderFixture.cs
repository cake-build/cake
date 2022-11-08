// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Build;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Build
{
    internal sealed class DotNetBuilderFixture : DotNetFixture<DotNetBuildSettings>
    {
        public string Project { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetBuilder(FileSystem, Environment, ProcessRunner, Tools);
            tool.Build(Project, Settings);
        }
    }
}