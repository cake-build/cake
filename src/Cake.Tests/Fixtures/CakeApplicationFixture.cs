using System.IO;
using Cake.Bootstrapping;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Scripting;
using NSubstitute;

namespace Cake.Tests.Fixtures
{
    public sealed class CakeApplicationFixture
    {
        public ICakeBootstrapper Bootstrapper { get; set; }
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IFile File { get; set; }
        public ICakeLog Log { get; set; }
        public IScriptRunner ScriptRunner { get; set; }

        public CakeApplicationFixture()
        {
            Bootstrapper = Substitute.For<ICakeBootstrapper>();

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            File = Substitute.For<IFile>();
            File.Exists.Returns(true);
            File.Open(FileMode.Open, FileAccess.Read, FileShare.Read).Returns(c => CreateCodeStream());

            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.GetFile(Arg.Is<FilePath>(p => p.FullPath == "/Build/script.csx")).Returns(File);

            Log = Substitute.For<ICakeLog>();

            ScriptRunner = Substitute.For<IScriptRunner>();
        }

        private Stream CreateCodeStream()
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
    }
}
