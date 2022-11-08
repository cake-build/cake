// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNet.VSTest;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.VSTest
{
    internal sealed class DotNetVSTesterFixture : DotNetFixture<DotNetVSTestSettings>
    {
        public ICollection<FilePath> TestFiles { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetVSTester(FileSystem, Environment, ProcessRunner, Tools);
            tool.Test(TestFiles, Settings);
        }
    }
}