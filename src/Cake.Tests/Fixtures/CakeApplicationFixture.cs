using System.IO;
using Cake.Bootstrapping;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;
using NSubstitute;

namespace Cake.Tests.Fixtures
{
    public sealed class CakeApplicationFixture
    {
        private readonly string _scriptPath;
        private readonly bool _showDescription;

        public ICakeBootstrapper Bootstrapper { get; set; }
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IFile File { get; set; }
        public ICakeLog Log { get; set; }
        public IScriptRunner ScriptRunner { get; set; }

        public CakeApplicationFixture(string scriptPath = "/Build/script.csx", bool showDescription = false)
        {
            _scriptPath = scriptPath;
            _showDescription = showDescription;

            Bootstrapper = Substitute.For<ICakeBootstrapper>();

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            File = Substitute.For<IFile>();
            File.Exists.Returns(true);
            File.Open(FileMode.Open, FileAccess.Read, FileShare.Read).Returns(c => CreateCodeStream());

            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.GetFile(Arg.Is<FilePath>(p => p.FullPath == _scriptPath)).Returns(File);

            Log = Substitute.For<ICakeLog>();
            ScriptRunner = Substitute.For<IScriptRunner>();
        }

        private static Stream CreateCodeStream()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(@"var lol = 123;");
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public CakeApplication CreateApplication()
        {
            return new CakeApplication(Bootstrapper, FileSystem, Environment, Log, ScriptRunner);
        }

        public void Run()
        {           
            CreateApplication().Run(new CakeOptions { Script = _scriptPath, ShowDescription = _showDescription});
        }
    }
}
