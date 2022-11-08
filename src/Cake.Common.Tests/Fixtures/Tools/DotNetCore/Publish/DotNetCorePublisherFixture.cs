// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Publish;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Publish
{
    internal sealed class DotNetPublisherFixture : DotNetFixture<DotNetPublishSettings>
    {
        public string Project { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetPublisher(FileSystem, Environment, ProcessRunner, Tools);
            tool.Publish(Project, Settings);
        }
    }
}