// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.IO.Compression;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
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
            public void Should_Throw_If_File_Is_Not_Relative_To_Root()
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
                Assert.Equal("File '/NotRoot/file.txt' is not relative to root path '/Root'.", result?.Message);
            }

            [Fact]
            public void Should_Zip_Provided_Files()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile("/Root/file.txt").SetContent("HelloWorld");
                var log = Substitute.For<ICakeLog>();
                var zipper = new Zipper(fileSystem, environment, log);

                // When
                zipper.Zip("/Root", "/file.zip", new FilePath[] { "/Root/file.txt" });

                // Then
                Assert.True(fileSystem.GetFile("/file.zip").Exists);
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