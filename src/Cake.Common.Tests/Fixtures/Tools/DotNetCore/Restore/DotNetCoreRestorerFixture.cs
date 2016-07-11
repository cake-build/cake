// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.DotNetCore.Restore;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.Restore
{
    internal sealed class DotNetCoreRestorerFixture : DotNetCoreFixture<DotNetCoreRestoreSettings>
    {
        public string Root { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetCoreRestorer(FileSystem, Environment, ProcessRunner, Tools);
            tool.Restore(Root, Settings);
        }
    }
}
