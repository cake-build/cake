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
            FileSystem.CreateDirectory("C:");
            FileSystem.CreateDirectory("C:/Program Files (x86)");

            // Files
            FileSystem.CreateFile("C:/Working/Foo/Bar/Qux.c");
            FileSystem.CreateFile("C:/Program Files (x86)/Foo.c");
            FileSystem.CreateFile("C:/Working/Project.A.Test.dll");
            FileSystem.CreateFile("C:/Working/Project.B.Test.dll");
            FileSystem.CreateFile("C:/Working/Project.IntegrationTest.dll");
            FileSystem.CreateFile("C:/Tools & Services/MyTool.dll");
            FileSystem.CreateFile("C:/Tools + Services/MyTool.dll");
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
            FileSystem.CreateDirectory("/Foo (Bar)");

            // Files
            FileSystem.CreateFile("/Working/Foo/Bar/Qux.c");
            FileSystem.CreateFile("/Working/Foo/Bar/Qex.c");
            FileSystem.CreateFile("/Working/Foo/Bar/Qux.h");
            FileSystem.CreateFile("/Working/Foo/Baz/Qux.c");
            FileSystem.CreateFile("/Working/Foo/Bar/Baz/Qux.c");
            FileSystem.CreateFile("/Working/Bar/Qux.c");
            FileSystem.CreateFile("/Working/Bar/Qux.h");
            FileSystem.CreateFile("/Working/Foo.Bar.Test.dll");
            FileSystem.CreateFile("/Working/Bar.Qux.Test.dll");
            FileSystem.CreateFile("/Working/Quz.FooTest.dll");
            FileSystem.CreateFile("/Foo/Bar.baz");
            FileSystem.CreateFile("/Foo (Bar)/Baz.c");
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
