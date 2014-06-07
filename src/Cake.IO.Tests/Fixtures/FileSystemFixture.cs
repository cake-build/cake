using Cake.Core.IO;
using Cake.Core.Tests.Fakes;

namespace Cake.IO.Tests.Fixtures
{
    public sealed class FileSystemFixture
    {
        public IFileSystem FileSystem { get; set; }

        public FileSystemFixture(bool isUnix = false)
        {
            FileSystem = CreateFileSystem(isUnix);
        }

        private static FakeFileSystem CreateFileSystem(bool isUnix)
        {
            var fileSystem = new FakeFileSystem(isUnix);
            fileSystem.GetCreatedDirectory("/Temp");
            fileSystem.GetCreatedDirectory("/Temp/HasDirectories");
            fileSystem.GetCreatedDirectory("/Temp/HasDirectories/A");
            fileSystem.GetCreatedDirectory("/Temp/HasFiles");
            fileSystem.GetCreatedDirectory("/Temp/Hello");
            fileSystem.GetCreatedDirectory("/Temp/Hello/Empty");
            fileSystem.GetCreatedDirectory("/Temp/Hello/World");
            fileSystem.GetCreatedDirectory("/Temp/Goodbye");
            fileSystem.GetCreatedFile("/Presentation.ppt");
            fileSystem.GetCreatedFile("/Budget.xlsx");
            fileSystem.GetCreatedFile("/Text.txt");
            fileSystem.GetCreatedFile("/Temp");
            fileSystem.GetCreatedFile("/Temp/Hello/Document.txt");
            fileSystem.GetCreatedFile("/Temp/Hello/World/Text.txt");
            fileSystem.GetCreatedFile("/Temp/Hello/World/Picture.png");
            fileSystem.GetCreatedFile("/Temp/Goodbye/OtherText.txt");
            fileSystem.GetCreatedFile("/Temp/Goodbye/OtherPicture.png");
            fileSystem.GetCreatedFile("/Temp/HasFiles/A.txt");
            return fileSystem;
        }
    }
}
