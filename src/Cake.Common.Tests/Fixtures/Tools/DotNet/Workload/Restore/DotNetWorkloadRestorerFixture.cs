// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Workload.Restore;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Workload.Restore
{
    internal sealed class DotNetWorkloadRestorerFixture : DotNetFixture<DotNetWorkloadRestoreSettings>
    {
        public string Project { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetWorkloadRestorer(FileSystem, Environment, ProcessRunner, Tools);
            tool.Restore(Project, Settings);
        }
    }
}
