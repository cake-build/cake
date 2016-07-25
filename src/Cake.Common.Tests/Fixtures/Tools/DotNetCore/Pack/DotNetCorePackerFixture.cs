﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNetCore.Pack;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.Pack
{
    internal sealed class DotNetCorePackFixture : DotNetCoreFixture<DotNetCorePackSettings>
    {
        public string Project { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetCorePacker(FileSystem, Environment, ProcessRunner, Tools);
            tool.Pack(Project, Settings);
        }
    }
}