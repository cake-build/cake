using Cake.Common.Solution.Project.Properties;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing.Fakes;
using NSubstitute;
using Xunit;
using System.IO;

namespace Cake.Common.Tests.Fixtures
{
    internal sealed class AssemblyInfoFixture
    {
        public IFileSystem FileSystem { get; set; }
        public ICakeLog Log { get; set; }
        public ICakeEnvironment Environment { get; set; }

        public AssemblyInfoSettings Settings { get; set; }
        public FilePath Path { get; set; }

        public AssemblyInfoFixture()
        {
            FileSystem = new FakeFileSystem(false);
            
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns(new DirectoryPath("/Working"));

            Log = Substitute.For<ICakeLog>();
            Settings = new AssemblyInfoSettings();
            Path = "AssemblyInfo.cs";
        }

        public string CreateAndReturnContent()
        {
            var creator = new AssemblyInfoCreator(FileSystem, Environment, Log);
            creator.Create(Path, Settings);

            var file = FileSystem.GetFile(Path.MakeAbsolute(Environment));
            Assert.True(file.Exists, "File was not created.");
            using (var reader = new StreamReader(file.OpenRead()))
            {
                return reader.ReadToEnd();
            }
        }
    }
}