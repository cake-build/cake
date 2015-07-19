using Cake.Core;
using Cake.Core.IO;
using Cake.Testing.Fakes;

namespace Cake.Common.Tests.Fixtures
{
    internal sealed class FileSystemFixture
    {
        public ICakeEnvironment Environment { get; set; }
        public IFileSystem FileSystem { get; set; }

        public FileSystemFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = CreateFileSystem(Environment);
        }

        private static FakeFileSystem CreateFileSystem(ICakeEnvironment environment)
        {
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateDirectory("/Temp");
            fileSystem.CreateDirectory("/Temp/HasDirectories");
            fileSystem.CreateDirectory("/Temp/HasDirectories/A");
            fileSystem.CreateDirectory("/Temp/HasFiles");
            fileSystem.CreateDirectory("/Temp/Hello");
            fileSystem.CreateDirectory("/Temp/Hello/Empty");
            fileSystem.CreateDirectory("/Temp/Hello/More/Empty");
            fileSystem.CreateDirectory("/Temp/Hello/World");
            fileSystem.CreateDirectory("/Temp/Goodbye");
            fileSystem.CreateDirectory("/Temp/Hello/Hidden").Hide();
            fileSystem.CreateFile("/Presentation.ppt");
            fileSystem.CreateFile("/Budget.xlsx");
            fileSystem.CreateFile("/Text.txt");
            fileSystem.CreateFile("/Temp");
            fileSystem.CreateFile("/Temp/Hello/Document.txt");
            fileSystem.CreateFile("/Temp/Hello/World/Text.txt");
            fileSystem.CreateFile("/Temp/Hello/World/Picture.png");
            fileSystem.CreateFile("/Temp/Hello/Hidden.txt").Hide();
            fileSystem.CreateFile("/Temp/Goodbye/OtherText.txt");
            fileSystem.CreateFile("/Temp/Goodbye/OtherPicture.png");
            fileSystem.CreateFile("/Temp/HasFiles/A.txt");
            return fileSystem;
        }
    }
}
