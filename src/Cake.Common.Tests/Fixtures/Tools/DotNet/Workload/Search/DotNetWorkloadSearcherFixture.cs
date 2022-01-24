// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Workload.Search;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Workload.Search
{
    internal sealed class DotNetWorkloadSearcherFixture : DotNetFixture<DotNetWorkloadSearchSettings>
    {
        public string SearchString { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetWorkloadSearcher(FileSystem, Environment, ProcessRunner, Tools);
            tool.Search(SearchString);
        }
    }
}
