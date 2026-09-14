// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.Cake;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    /// <summary>
    /// Runs the Cake tool through the .NET host, which is what happens when the
    /// resolved tool path is Cake.dll instead of a Cake executable.
    /// </summary>
    internal sealed class CakeRunnerCoreFixture : ToolFixture<CakeSettings>
    {
        public FilePath ScriptPath { get; set; }

        public CakeRunnerCoreFixture()
            : base("dotnet.exe")
        {
            ScriptPath = new FilePath("./build.cake");
            FileSystem.CreateFile(ScriptPath.MakeAbsolute(Environment));

            Settings.ToolPath = new FilePath("./tools/Cake.dll");
            this.GivenSettingsToolPathExist();
        }

        protected override void RunTool()
        {
            var runner = new CakeRunner(FileSystem, Environment, Globber, ProcessRunner, Tools);
            runner.ExecuteScript(ScriptPath, Settings);
        }
    }
}
