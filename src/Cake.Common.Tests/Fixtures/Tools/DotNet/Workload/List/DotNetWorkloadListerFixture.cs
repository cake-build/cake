// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Workload.List;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Workload.List
{
    internal sealed class DotNetWorkloadListerFixture : DotNetFixture<DotNetWorkloadListSettings>
    {
        public IEnumerable<DotNetWorkloadListItem> Workloads { get; set; }

        public void GivenInstalledWorkloadsResult()
        {
            ProcessRunner.Process.SetStandardOutput(new string[]
            {
                "Installed Workload Ids      Manifest Version      Installation Source",
                "--------------------------------------------------------------------------------------",
                "maui-ios                    6.0.312/6.0.300       VS 17.3.32804.467, VS 17.4.32804.182",
                "maui-windows                6.0.312/6.0.300       VS 17.3.32804.467, VS 17.4.32804.182",
                "android                     32.0.301/6.0.300      VS 17.3.32804.467, VS 17.4.32804.182",
                "",
                "Use `dotnet workload search` to find additional workloads to install."
            });
        }

        public void GivenEmptyInstalledWorkloadsResult()
        {
            ProcessRunner.Process.SetStandardOutput(new string[]
            {
                "Installed Workload Ids      Manifest Version      Installation Source",
                "---------------------------------------------------------------------",
                "",
                "Use `dotnet workload search` to find additional workloads to install."
            });
        }

        protected override void RunTool()
        {
            var tool = new DotNetWorkloadLister(FileSystem, Environment, ProcessRunner, Tools);
            Workloads = tool.List(Settings);
        }
    }
}
