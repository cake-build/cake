// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNetCore.VSTest;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.VSTest
{
    internal sealed class DotNetCoreVSTesterFixture : DotNetCoreFixture<DotNetCoreVSTestSettings>
    {
        public ICollection<FilePath> TestFiles { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetCoreVSTester(FileSystem, Environment, ProcessRunner, Tools);
            tool.Test(TestFiles, Settings);
        }
    }
}