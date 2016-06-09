// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Common.Tools.WiX;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class LightFixture : ToolFixture<LightSettings>
    {
        public List<FilePath> ObjectFiles { get; set; }

        public LightFixture()
            : base("light.exe")
        {
            ObjectFiles = new List<FilePath>();
            ObjectFiles.Add(new FilePath("./Test.wixobj"));
        }

        protected override void RunTool()
        {
            var tool = new LightRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(ObjectFiles, Settings);
        }
    }
}
