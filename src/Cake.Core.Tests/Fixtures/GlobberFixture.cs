using Cake.Core.IO;
using Cake.Testing.Fakes;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    internal sealed class GlobberFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }

        public GlobberFixture(bool windows = false)
        {
            Environment = windows ? FakeEnvironment.CreateWindowsEnvironment() : FakeEnvironment.CreateUnixEnvironment();
            Environment.WorkingDirectory = new DirectoryPath("/Temp");

            FileSystem = new FakeFileSystem(Environment);
            
            FileSystem.CreateDirectory("/Temp");
            FileSystem.CreateDirectory("/Temp/Hello/World");
            FileSystem.CreateDirectory("/Temp/Goodbye");

            FileSystem.CreateFile("/Presentation.ppt");
            FileSystem.CreateFile("/Budget.xlsx");
            FileSystem.CreateFile("/Text.txt");
            FileSystem.CreateFile("/Working/Text.txt");

            FileSystem.CreateFile("/Temp/Hello/World/Text.txt");
            FileSystem.CreateFile("/Temp/Hello/World/Picture.png");
           
            FileSystem.CreateFile("/Temp/Goodbye/OtherText.txt");
            FileSystem.CreateFile("/Temp/Goodbye/OtherPicture.png");

            if (windows)
            {
                FileSystem.CreateFile("C:/Temp/Hello/World/Text.txt");
            }
        }

        public void SetWorkingDirectory(DirectoryPath path)
        {
            Environment.WorkingDirectory = path;
        }

        public Globber CreateGlobber()
        {
            return new Globber(FileSystem, Environment);
        }
    }
}
