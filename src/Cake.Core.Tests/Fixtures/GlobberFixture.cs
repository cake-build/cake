using Cake.Core.IO;
using Cake.Testing.Fakes;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    internal sealed class GlobberFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }

        public GlobberFixture()
            : this(false)
        {            
        }

        public GlobberFixture(bool isFileSystemCaseSensitive)
        {
            FileSystem = new FakeFileSystem(isFileSystemCaseSensitive);
            FileSystem.GetCreatedDirectory("/Temp");
            FileSystem.GetCreatedDirectory("/Temp/Hello");
            FileSystem.GetCreatedDirectory("/Temp/Hello/World");
            FileSystem.GetCreatedDirectory("/Temp/Goodbye");
            FileSystem.GetCreatedFile("/Presentation.ppt");
            FileSystem.GetCreatedFile("/Budget.xlsx");
            FileSystem.GetCreatedFile("/Text.txt");
            FileSystem.GetCreatedFile("/Temp");
            FileSystem.GetCreatedFile("/Temp/Hello/World/Text.txt");
            FileSystem.GetCreatedFile("/Temp/Hello/World/Picture.png");
            FileSystem.GetCreatedFile("/Temp/Goodbye/OtherText.txt");
            FileSystem.GetCreatedFile("/Temp/Goodbye/OtherPicture.png");
            FileSystem.GetCreatedFile("/Working/Text.txt");
            FileSystem.GetCreatedFile("C:/Temp/Hello/World/Text.txt");

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.IsUnix().Returns(isFileSystemCaseSensitive);
            Environment.WorkingDirectory.Returns("/Temp");
        }

        public void SetWorkingDirectory(DirectoryPath path)
        {
            Environment.WorkingDirectory.Returns(path);
        }

        public Globber CreateGlobber()
        {
            return new Globber(FileSystem, Environment);
        }
    }
}
