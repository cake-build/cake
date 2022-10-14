// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Workload.Uninstall;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Workload.Uninstall
{
    internal sealed class DotNetWorkloadUninstallerFixture : DotNetFixture<DotNetWorkloadUninstallSettings>
    {
        public IEnumerable<string> WorkloadIds { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetWorkloadUninstaller(FileSystem, Environment, ProcessRunner, Tools);
            tool.Uninstall(WorkloadIds);
        }
    }
}
