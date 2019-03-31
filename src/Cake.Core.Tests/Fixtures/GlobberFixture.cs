// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Core.IO;
using Cake.Testing;

namespace Cake.Core.Tests.Fixtures
{
    internal sealed class GlobberFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }

        private GlobberFixture(FakeFileSystem filesystem, ICakeEnvironment environment)
        {
            FileSystem = filesystem;
            Environment = environment;
        }

        public static GlobberFixture Windows()
        {
            var environment = FakeEnvironment.CreateWindowsEnvironment();
            var filesystem = new FakeFileSystem(environment);

            // Directories
            filesystem.CreateDirectory("C://Working");
            filesystem.CreateDirectory("C://Working/Foo");
            filesystem.CreateDirectory("C://Working/Foo/Bar");
            filesystem.CreateDirectory("C:");
            filesystem.CreateDirectory("C:/Program Files (x86)");

            // UNC directories
            filesystem.CreateDirectory(@"\\Server");
            filesystem.CreateDirectory(@"\\Server\Foo");
            filesystem.CreateDirectory(@"\\Server\Foo\Bar");
            filesystem.CreateDirectory(@"\\Server\Bar");
            filesystem.CreateDirectory(@"\\Foo\Bar");
            filesystem.CreateDirectory(@"\\Foo (Bar)");
            filesystem.CreateDirectory(@"\\Foo@Bar\");
            filesystem.CreateDirectory(@"\\嵌套");
            filesystem.CreateDirectory(@"\\嵌套\目录");

            // Files
            filesystem.CreateFile("C:/Working/Foo/Bar/Qux.c");
            filesystem.CreateFile("C:/Program Files (x86)/Foo.c");
            filesystem.CreateFile("C:/Working/Project.A.Test.dll");
            filesystem.CreateFile("C:/Working/Project.B.Test.dll");
            filesystem.CreateFile("C:/Working/Project.IntegrationTest.dll");
            filesystem.CreateFile("C:/Tools & Services/MyTool.dll");
            filesystem.CreateFile("C:/Tools + Services/MyTool.dll");
            filesystem.CreateFile("C:/Some %2F Directory/MyTool.dll");
            filesystem.CreateFile("C:/Some ! Directory/MyTool.dll");
            filesystem.CreateFile("C:/Some@Directory/MyTool.dll");
            filesystem.CreateFile("C:/Working/foobar.rs");
            filesystem.CreateFile("C:/Working/foobaz.rs");
            filesystem.CreateFile("C:/Working/foobax.rs");

            // UNC files
            filesystem.CreateFile(@"\\Server\Foo/Bar/Qux.c");
            filesystem.CreateFile(@"\\Server\Foo/Bar/Qex.c");
            filesystem.CreateFile(@"\\Server\Foo/Bar/Qux.h");
            filesystem.CreateFile(@"\\Server\Foo/Baz/Qux.c");
            filesystem.CreateFile(@"\\Server\Foo/Bar/Baz/Qux.c");
            filesystem.CreateFile(@"\\Server\Bar/Qux.c");
            filesystem.CreateFile(@"\\Server\Bar/Qux.h");
            filesystem.CreateFile(@"\\Server\Foo.Bar.Test.dll");
            filesystem.CreateFile(@"\\Server\Bar.Qux.Test.dll");
            filesystem.CreateFile(@"\\Server\Quz.FooTest.dll");
            filesystem.CreateFile(@"\\Foo\Bar.baz");
            filesystem.CreateFile(@"\\Foo (Bar)\Baz.c");
            filesystem.CreateFile(@"\\Foo@Bar\Baz.c");
            filesystem.CreateFile(@"\\嵌套/目录/文件.延期");

            return new GlobberFixture(filesystem, environment);
        }

        public static GlobberFixture UnixLike()
        {
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var filesystem = new FakeFileSystem(environment);

            // Directories
            filesystem.CreateDirectory("/RootDir");
            filesystem.CreateDirectory("/Working");
            filesystem.CreateDirectory("/Working/Foo");
            filesystem.CreateDirectory("/Working/Foo/Bar");
            filesystem.CreateDirectory("/Working/Bar");
            filesystem.CreateDirectory("/Foo/Bar");
            filesystem.CreateDirectory("/Foo (Bar)");
            filesystem.CreateDirectory("/Foo@Bar/");
            filesystem.CreateDirectory("/嵌套");
            filesystem.CreateDirectory("/嵌套/目录");

            // Files
            filesystem.CreateFile("/RootFile.sh");
            filesystem.CreateFile("/Working/Foo/Bar/Qux.c");
            filesystem.CreateFile("/Working/Foo/Bar/Qex.c");
            filesystem.CreateFile("/Working/Foo/Bar/Qux.h");
            filesystem.CreateFile("/Working/Foo/Baz/Qux.c");
            filesystem.CreateFile("/Working/Foo/Bar/Baz/Qux.c");
            filesystem.CreateFile("/Working/Bar/Qux.c");
            filesystem.CreateFile("/Working/Bar/Qux.h");
            filesystem.CreateFile("/Working/Foo.Bar.Test.dll");
            filesystem.CreateFile("/Working/Bar.Qux.Test.dll");
            filesystem.CreateFile("/Working/Quz.FooTest.dll");
            filesystem.CreateFile("/Foo/Bar.baz");
            filesystem.CreateFile("/Foo (Bar)/Baz.c");
            filesystem.CreateFile("/Foo@Bar/Baz.c");
            filesystem.CreateFile("/嵌套/目录/文件.延期");
            filesystem.CreateFile("/Working/foobar.rs");
            filesystem.CreateFile("/Working/foobaz.rs");
            filesystem.CreateFile("/Working/foobax.rs");

            return new GlobberFixture(filesystem, environment);
        }

        public void SetWorkingDirectory(DirectoryPath path)
        {
            Environment.WorkingDirectory = path;
        }

        public Path[] Match(string pattern)
        {
            return Match(pattern, null);
        }

        public Path[] Match(string pattern, Func<IFileSystemInfo, bool> directoryPredicate)
        {
            return Match(pattern, directoryPredicate, null);
        }

        public Path[] Match(string pattern, Func<IFileSystemInfo, bool> directoryPredicate, Func<IFile, bool> filePredicate)
        {
            return new Globber(FileSystem, Environment)
                .Match(pattern, new GlobberSettings { Predicate = directoryPredicate, FilePredicate = filePredicate })
                .ToArray();
        }
    }
}