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
            var runner = new CakeRunner(FileSystem, Environment, Globber, ProcessRunner);
            runner.ExecuteScript(ScriptPath, Settings);
        }
    }
}