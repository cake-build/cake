// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.NSIS;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    // ReSharper disable once InconsistentNaming
    internal sealed class NSISFixture : ToolFixture<MakeNSISSettings>
    {
        public FilePath ScriptPath { get; set; }

        public NSISFixture()
            : base("makensis.exe")
        {
            ScriptPath = new FilePath("./Test.nsi");
        }

        protected override void RunTool()
        {
            var tool = new MakeNSISRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(ScriptPath, Settings);
        }
    }
}
