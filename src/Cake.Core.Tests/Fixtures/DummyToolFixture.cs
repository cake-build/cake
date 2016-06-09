// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.Tests.Stubs;
using Cake.Testing.Fixtures;

namespace Cake.Core.Tests.Fixtures
{
    public sealed class DummyToolFixture : ToolFixture<DummySettings>
    {
        public DummyToolFixture()
            : base("dummy.exe")
        {
        }

        protected override void RunTool()
        {
            var tool = new DummyTool(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(Settings);
        }
    }
}
