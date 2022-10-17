// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Restore;
using Cake.Testing;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Restore
{
    internal sealed class DotNetRestorerFixture : DotNetFixture<DotNetRestoreSettings>
    {
        public string Root { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetRestorer(FileSystem, Environment, ProcessRunner, Tools, new FakeLog());
            tool.Restore(Root, Settings);
        }
    }
}