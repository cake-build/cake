// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.IO.Compression;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tests.Fixtures;
using Cake.Testing;
using Cake.Testing.Xunit;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.IO
{
    public sealed class ZipperTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                var log = Substitute.For<ICakeLog>();

                // When
                var result = Record.Exception(() => new Zipper(null, environment, log));

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var log = Substitute.For<ICakeLog>();

                // When
                var result = Record.Exception(() => new Zipper(fileSystem, null, log));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var environment = Substitute.For<ICakeEnvironment>();

                // When
                var result = Record.Exception(() => new Zipper(fileSystem, environment, null));

                // Then
                AssertEx.IsArgumentNullException(result, "log");
            }
        }

        public sealed class TheZipMethod
        {
            [Fact]
            public void Should_Throw_If_Root_Path_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var environment = Substitute.For<ICakeEnvironment>();
                var log = Substitute.For<ICakeLog>();
                var zipper = new Zipper(fileSystem, environment, log);

                // When
                var result = Record.Exception(() => zipper.Zip(null, "/file.zip", new FilePath[] { "/Root/file.txt" }));

                // Then
                AssertEx.IsArgumentNullException(result, "rootPath");
            }

            [Fact]
            public void Should_Throw_If_Output_Path_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var environment = Substitute.For<ICakeEnvironment>();
                var log = Substitute.For<ICakeLog>();
                var zipper = new Zipper(fileSystem, environment, log);

                // When
                var result = Record.Exception(() => zipper.Zip("/Root", null, new FilePath[] { "/Root/file.txt" }));

                // Then
                AssertEx.IsArgumentNullException(result, "outputPath");
            }

            [Fact]
            public void Should_Throw_If_File_Paths_Are_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var environment = Substitute.For<ICakeEnvironment>();
                var log = Substitute.For<ICakeLog>();
                var zipper = new Zipper(fileSystem, environment, log);

                // When
                var result = Record.Exception(() => zipper.Zip("/Root", "/file.txt", null));

                // Then
                AssertEx.IsArgumentNullException(result, "filePaths");
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Not_Relative_To_Root()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile("/NotRoot/file.txt");
                var log = Substitute.For<ICakeLog>();
                var zipper = new Zipper(fileSystem, environment, log);

                // When
                var result = Record.Exception(() => zipper.Zip("/Root", "/file.zip", new FilePath[] { "/NotRoot/file.txt" }));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Path '/NotRoot/file.txt' is not relative to root path '/Root'.", result?.Message);
            }

            [Fact]
            public void Should_Zip_Provided_Directory()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var globber = new Globber(fileSystem, environment);
                var context = new CakeContextFixture { Environment = environment, FileSystem = fileSystem, Globber = globber }.CreateContext();
                fileSystem.CreateDirectory("/Dir0"); // empty directory
                fileSystem.CreateFile("/File1.txt").SetContent("1");
                fileSystem.CreateFile("/Dir1/File2.txt").SetContent("22");
                fileSystem.CreateFile("/Dir2/File3.txt").SetContent("333");
                fileSystem.CreateFile("/Dir2/Dir3/File4.txt").SetContent("4444");
                fileSystem.CreateFile("/Dir2/Dir3/File5.txt").SetContent("55555");
                var log = Substitute.For<ICakeLog>();
                var zipper = new Zipper(fileSystem, environment, log);

                // When
                zipper.Zip("/", "/Root.zip", context.GetPaths("/**/*"));

                // Then
                var archive = new ZipArchive(fileSystem.GetFile("/Root.zip").Open(FileMode.Open, FileAccess.Read, FileShare.Read));
                Assert.True(archive.Entries.Count == 9);
                Assert.True(archive.GetEntry("Dir0/")?.Length == 0); // directory entries; includes empty directories
                Assert.True(archive.GetEntry("Dir1/")?.Length == 0);
                Assert.True(archive.GetEntry("Dir2/")?.Length == 0);
                Assert.True(archive.GetEntry("Dir2/Dir3/")?.Length == 0);
                Assert.True(archive.GetEntry("File1.txt")?.Length == 1); // file entries
                Assert.True(archive.GetEntry("Dir1/File2.txt")?.Length == 2);
                Assert.True(archive.GetEntry("Dir2/File3.txt")?.Length == 3);
                Assert.True(archive.GetEntry("Dir2/Dir3/File4.txt")?.Length == 4);
                Assert.True(archive.GetEntry("Dir2/Dir3/File5.txt")?.Length == 5);
            }

            [Fact]
            public void Should_Zip_Provided_Files()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var globber = new Globber(fileSystem, environment);
                var context = new CakeContextFixture { Environment = environment, FileSystem = fileSystem, Globber = globber }.CreateContext();
                fileSystem.CreateFile("/File1.txt").SetContent("1");
                fileSystem.CreateFile("/Dir1/File2.txt").SetContent("22");
                fileSystem.CreateFile("/Dir2/File3.txt").SetContent("333");
                fileSystem.CreateFile("/Dir2/Dir3/File4.txt").SetContent("4444");
                fileSystem.CreateFile("/Dir2/Dir3/File5.txt").SetContent("55555");
                var log = Substitute.For<ICakeLog>();
                var zipper = new Zipper(fileSystem, environment, log);

                // When
                zipper.Zip("/", "/Root.zip", context.GetFiles("/**/*.txt"));

                // Then
                var archive = new ZipArchive(fileSystem.GetFile("/Root.zip").Open(FileMode.Open, FileAccess.Read, FileShare.Read));
                Assert.True(archive.Entries.Count == 8);
                Assert.True(archive.GetEntry("Dir1/")?.Length == 0); // directory entries
                Assert.True(archive.GetEntry("Dir2/")?.Length == 0);
                Assert.True(archive.GetEntry("Dir2/Dir3/")?.Length == 0);
                Assert.True(archive.GetEntry("File1.txt")?.Length == 1); // file entries
                Assert.True(archive.GetEntry("Dir1/File2.txt")?.Length == 2);
                Assert.True(archive.GetEntry("Dir2/File3.txt")?.Length == 3);
                Assert.True(archive.GetEntry("Dir2/Dir3/File4.txt")?.Length == 4);
                Assert.True(archive.GetEntry("Dir2/Dir3/File5.txt")?.Length == 5);
            }

            [WindowsFact("Investigate why this fail on Mono 4.2.1.")]
            public void Zipped_File_Should_Contain_Correct_Content()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile("/Root/Stuff/file.txt").SetContent("HelloWorld");
                var log = Substitute.For<ICakeLog>();
                var zipper = new Zipper(fileSystem, environment, log);
                zipper.Zip("/Root", "/file.zip", new FilePath[] { "/Root/Stuff/file.txt" });

                // When
                var archive = new ZipArchive(fileSystem.GetFile("/file.zip").Open(FileMode.Open, FileAccess.Read, FileShare.Read));
                var entry = archive.GetEntry("Stuff/file.txt");

                // Then
                Assert.NotNull(entry);
                Assert.True(entry.Length == 10);
            }
        }
    }
}