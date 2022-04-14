// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Workload.Search;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Workload.Search
{
    internal sealed class DotNetWorkloadSearcherFixture : DotNetFixture<DotNetWorkloadSearchSettings>
    {
        public string SearchString { get; set; }
        public IEnumerable<DotNetWorkload> Workloads { get; set; }

        public void GivenAvailableWorkloadsResult()
        {
            ProcessRunner.Process.SetStandardOutput(new string[]
            {
                "Workload ID           Description",
                "-----------------------------------------------------",
                "maui                  .NET MAUI SDK for all platforms",
                "maui-desktop          .NET MAUI SDK for Desktop",
                "maui-mobile           .NET MAUI SDK for Mobile"
            });
        }

        protected override void RunTool()
        {
            var tool = new DotNetWorkloadSearcher(FileSystem, Environment, ProcessRunner, Tools);
            Workloads = tool.Search(SearchString, Settings);
        }
    }
}
