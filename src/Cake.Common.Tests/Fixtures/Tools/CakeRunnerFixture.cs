// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.Cake;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class CakeRunnerFixture : ToolFixture<CakeSettings>
    {
        public FilePath ScriptPath { get; set;}

        public CakeRunnerFixture()
            : base("Cake.exe")
        {
            ScriptPath = new FilePath("./build.cake");
            FileSystem.CreateFile(ScriptPath.MakeAbsolute(Environment));
        }

        public void GivenScriptDoNotExist()
        {
            var path = ScriptPath.MakeAbsolute(Environment);
            if (FileSystem.Exist(path))
            {
                FileSystem.GetFile(path).Delete();
            }
        }

        protected override void RunTool()
        {
            var runner = new CakeRunner(FileSystem, Environment, Globber, ProcessRunner, Tools);
            runner.ExecuteScript(ScriptPath, Settings);
        }
    }
}
