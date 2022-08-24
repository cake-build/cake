// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Workload.Repair;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Workload.Repair
{
    internal sealed class DotNetWorkloadRepairerFixture : DotNetFixture<DotNetWorkloadRepairSettings>
    {
        protected override void RunTool()
        {
            var tool = new DotNetWorkloadRepairer(FileSystem, Environment, ProcessRunner, Tools);
            tool.Repair(Settings);
        }
    }
}
