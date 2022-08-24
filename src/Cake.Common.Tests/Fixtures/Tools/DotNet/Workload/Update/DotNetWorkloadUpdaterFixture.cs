// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Workload.Update;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Workload.Update
{
    internal sealed class DotNetWorkloadUpdaterFixture : DotNetFixture<DotNetWorkloadUpdateSettings>
    {
        protected override void RunTool()
        {
            var tool = new DotNetWorkloadUpdater(FileSystem, Environment, ProcessRunner, Tools);
            tool.Update(Settings);
        }
    }
}
