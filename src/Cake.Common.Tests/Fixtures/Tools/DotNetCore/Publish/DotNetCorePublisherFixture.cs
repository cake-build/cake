// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.DotNetCore.Publish;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.Publish
{
    internal sealed class DotNetCorePublisherFixture : DotNetCoreFixture<DotNetCorePublishSettings>
    {
        public string Project { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetCorePublisher(FileSystem, Environment, ProcessRunner, Tools);
            tool.Publish(Project, Settings);
        }
    }
}
