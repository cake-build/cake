using Cake.Core.IO;
using Cake.Core.Scripting;
using Cake.Core.Tests.Fakes;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    public sealed class ScriptProcessorFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }

        public FilePath ScriptPath { get; set; }
        public string Source { get; private set; }

        public ScriptProcessorFixture(string scriptPath = "./build.cake", bool scriptExist = true,
            string scriptSource = "Console.WriteLine();")
        {
            ScriptPath = new FilePath(scriptPath);
            Source = scriptSource;

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            FileSystem = new FakeFileSystem(true);
            if (scriptExist)
            {
                FileSystem.GetCreatedFile(ScriptPath.MakeAbsolute(Environment), Source);
            }
        }

        public ScriptProcessor CreateProcessor()
        {
            return new ScriptProcessor(FileSystem, Environment);
        }

        public ScriptProcessorResult Process()
        {
            return CreateProcessor().Process(ScriptPath);
        }
    }
}
