// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Workload.Install;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Workload.Install
{
    internal sealed class DotNetWorkloadInstallerFixture : DotNetFixture<DotNetWorkloadInstallSettings>
    {
        public IEnumerable<string> WorkloadIds { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetWorkloadInstaller(FileSystem, Environment, ProcessRunner, Tools);
            tool.Install(WorkloadIds, Settings);
        }
    }
}
