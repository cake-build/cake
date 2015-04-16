using Cake.Core.IO;
using Cake.Testing.Fakes;

namespace Cake.Common.Tests.Fixtures
{
    internal sealed class FileSystemFixture
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
            fileSystem.GetCreatedDirectory("/Temp/Hello/More/Empty");
            fileSystem.GetCreatedDirectory("/Temp/Hello/World");
            fileSystem.GetCreatedDirectory("/Temp/Goodbye");
            fileSystem.GetCreatedDirectory("/Temp/Hello/Hidden", true);
            fileSystem.GetCreatedFile("/Presentation.ppt");
            fileSystem.GetCreatedFile("/Budget.xlsx");
            fileSystem.GetCreatedFile("/Text.txt");
            fileSystem.GetCreatedFile("/Temp");
            fileSystem.GetCreatedFile("/Temp/Hello/Document.txt");
            fileSystem.GetCreatedFile("/Temp/Hello/World/Text.txt");
            fileSystem.GetCreatedFile("/Temp/Hello/World/Picture.png");
            fileSystem.GetCreatedFile("/Temp/Hello/Hidden.txt", true);
            fileSystem.GetCreatedFile("/Temp/Goodbye/OtherText.txt");
            fileSystem.GetCreatedFile("/Temp/Goodbye/OtherPicture.png");
            fileSystem.GetCreatedFile("/Temp/HasFiles/A.txt");
            return fileSystem;
        }
    }
}
