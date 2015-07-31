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

        public GlobberFixture(bool windows)
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.IsUnix().Returns(!windows);

            if (windows)
            {
                Environment.WorkingDirectory.Returns("C:/Working");
            }
            else
            {
                Environment.WorkingDirectory.Returns("/Working");
            }

            FileSystem = new FakeFileSystem(Environment);

            FileSystem.CreateDirectory("/Working");
            FileSystem.CreateDirectory("/Working/Foo");
            FileSystem.CreateDirectory("/Working/Foo/Bar");
            FileSystem.CreateDirectory("/Working/Bar");

            FileSystem.CreateFile("/Working");
            FileSystem.CreateFile("/Working/Foo/Bar/Qux.c");
            FileSystem.CreateFile("/Working/Foo/Bar/Qex.c");
            FileSystem.CreateFile("/Working/Foo/Bar/Qux.h");
            FileSystem.CreateFile("/Working/Foo/Baz/Qux.c");
            FileSystem.CreateFile("/Working/Foo/Bar/Baz/Qux.c");
            FileSystem.CreateFile("/Working/Bar/Qux.c");
            FileSystem.CreateFile("/Working/Bar/Qux.h");

            FileSystem.CreateFile("C:/Working/Foo/Bar/Qux.c");

            FileSystem.CreateDirectory("/Foo/Bar");
            FileSystem.CreateFile("/Foo/Bar.baz");
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
