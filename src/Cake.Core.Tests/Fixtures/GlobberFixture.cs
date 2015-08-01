using System;
using System.Linq;
using Cake.Core.IO;
using Cake.Testing.Fakes;

namespace Cake.Core.Tests.Fixtures
{
    internal sealed class GlobberFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }

        public GlobberFixture(bool windows = false)
        {
            if (windows)
            {
                PrepareWindowsFixture();
            }
            else
            {
                PrepareUnixFixture();
            }
        }

        private void PrepareWindowsFixture()
        {
            Environment = FakeEnvironment.CreateWindowsEnvironment();
            FileSystem = new FakeFileSystem(Environment);

            // Directories
            FileSystem.CreateDirectory("C://Working");
            FileSystem.CreateDirectory("C://Working/Foo");
            FileSystem.CreateDirectory("C://Working/Foo/Bar");

            // Files
            FileSystem.CreateFile("C:/Working/Foo/Bar/Qux.c");
        }

        private void PrepareUnixFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);

            // Directories
            FileSystem.CreateDirectory("/Working");
            FileSystem.CreateDirectory("/Working/Foo");
            FileSystem.CreateDirectory("/Working/Foo/Bar");
            FileSystem.CreateDirectory("/Working/Bar");
            FileSystem.CreateDirectory("/Foo/Bar");

            // Files
            FileSystem.CreateFile("/Working/Foo/Bar/Qux.c");
            FileSystem.CreateFile("/Working/Foo/Bar/Qex.c");
            FileSystem.CreateFile("/Working/Foo/Bar/Qux.h");
            FileSystem.CreateFile("/Working/Foo/Baz/Qux.c");
            FileSystem.CreateFile("/Working/Foo/Bar/Baz/Qux.c");
            FileSystem.CreateFile("/Working/Bar/Qux.c");
            FileSystem.CreateFile("/Working/Bar/Qux.h");
            FileSystem.CreateFile("/Foo/Bar.baz");
        }

        public void SetWorkingDirectory(DirectoryPath path)
        {
            Environment.WorkingDirectory = path;
        }

        public Path[] Match(string pattern)
        {
            return Match(pattern, null);
        }

        public Path[] Match(string pattern, Func<IFileSystemInfo, bool> predicate)
        {
            return new Globber(FileSystem, Environment)
                .Match(pattern, predicate)
                .ToArray();
        }
    }
}
