// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.Tools.DotNet.MSBuild;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.MSBuild
{
    internal sealed class DotNetMSBuildBuilderFixture : DotNetFixture<DotNetMSBuildSettings>
    {
        public string Project { get; set; }
        public Action<IEnumerable<string>> StandardOutputAction { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetMSBuildBuilder(FileSystem, Environment, ProcessRunner, Tools);
            tool.Build(Project, Settings, StandardOutputAction);
        }
    }
}