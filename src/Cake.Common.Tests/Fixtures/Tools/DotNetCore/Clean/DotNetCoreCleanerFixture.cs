// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNetCore.Clean;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.Clean
{
    internal sealed class DotNetCoreCleanerFixture : DotNetCoreFixture<DotNetCoreCleanSettings>
    {
        public string Project { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetCoreCleaner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Clean(Project, Settings);
        }
    }
}