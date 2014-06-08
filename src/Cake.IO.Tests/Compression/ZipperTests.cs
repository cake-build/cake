using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tests.Fakes;
using Cake.IO.Compression;
using NSubstitute;
using Xunit;

namespace Cake.IO.Tests.Compression
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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("fileSystem", ((ArgumentNullException)result).ParamName);
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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("environment", ((ArgumentNullException)result).ParamName);
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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("log", ((ArgumentNullException)result).ParamName);
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
                var result = Record.Exception(() => zipper.Zip(null, "/file.zip", new FilePath[] {"/Root/file.txt"}));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("rootPath", ((ArgumentNullException)result).ParamName);
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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("outputPath", ((ArgumentNullException)result).ParamName);
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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("filePaths", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_File_Is_Not_Relative_To_Root()
            {                
                // Given
                var fileSystem = new FakeFileSystem(false);
                fileSystem.GetCreatedFile("/NotRoot/file.txt");
                var environment = Substitute.For<ICakeEnvironment>();
                var log = Substitute.For<ICakeLog>();
                var zipper = new Zipper(fileSystem, environment, log);

                // When
                var result = Record.Exception(() => zipper.Zip("/Root", "/file.zip", new FilePath[] { "/NotRoot/file.txt" }));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("File '/NotRoot/file.txt' is not relative to root path '/Root'.", result.Message);
            }

            [Fact]
            public void Should_Zip_Provided_Files()
            {
                // Given
                var fileSystem = new FakeFileSystem(false);
                fileSystem.GetCreatedFile("/Root/file.txt", "HelloWorld");                
                var environment = Substitute.For<ICakeEnvironment>();
                var log = Substitute.For<ICakeLog>();
                var zipper = new Zipper(fileSystem, environment, log);

                // When
                zipper.Zip("/Root", "/file.zip", new FilePath[] {"/Root/file.txt"});

                // Then
                Assert.True(fileSystem.GetFile("/file.zip").Exists);
            }

            [Fact]
            public void Zipped_File_Should_Contain_Correct_Content()
            {
                // Given
                var fileSystem = new FakeFileSystem(false);
                fileSystem.GetCreatedFile("/Root/Stuff/file.txt", "HelloWorld");
                var environment = Substitute.For<ICakeEnvironment>();
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
