using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Scripting;
using Cake.Testing;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    internal sealed class ScriptProcessorFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public ICakeLog Log { get; set; }
        public FilePath ScriptPath { get; set; }
        public string Source { get; private set; }
        public IGlobber Globber{ get; set; }
        public INuGetToolResolver NuGetToolResolver{ get; private set; }
        
        public ScriptProcessorFixture(
            string scriptPath = "./build.cake", 
            bool scriptExist = true,
            string scriptSource = "Console.WriteLine();")
        {            
            ScriptPath = new FilePath(scriptPath);
            Source = scriptSource;

            Log = Substitute.For<ICakeLog>();
            Globber = Substitute.For<IGlobber>();

            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            if (scriptExist)
            {
                FileSystem.CreateFile(ScriptPath.MakeAbsolute(Environment)).SetContent(Source);
            }

            NuGetToolResolver = new NuGetToolResolver(FileSystem, Environment, Globber);
        }

        public ScriptProcessor CreateProcessor()
        {
            return new ScriptProcessor(FileSystem, Environment, Log, NuGetToolResolver);
        }

        public ScriptProcessorContext Process()
        {
            var context = new ScriptProcessorContext();
            CreateProcessor().Process(ScriptPath, context);
            return context;
        }

        public string GetExpectedSource()
        {
            return string.Concat("#line 1 \"/Working/build.cake\"", "\r\n", Source);
        }

        public string GetActualSource(ScriptProcessorContext context)
        {
            return string.Join("\r\n", context.Lines);
        }
    }
}
