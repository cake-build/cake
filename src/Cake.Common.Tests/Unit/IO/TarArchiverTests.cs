// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Formats.Tar;
using System.IO.Compression;
using System.Text;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tests.Fixtures;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Unit.IO;

public sealed class TarArchiverTests
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
            var result = Record.Exception(() => new TarArchiver(null, environment, log));

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
            var result = Record.Exception(() => new TarArchiver(fileSystem, null, log));

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
            var result = Record.Exception(() => new TarArchiver(fileSystem, environment, null));

            // Then
            AssertEx.IsArgumentNullException(result, "log");
        }
    }

    public sealed class TheTarMethod
    {
        [Fact]
        public void Should_Throw_If_Root_Path_Is_Null()
        {
            // Given
            var archiver = CreateArchiver();

            // When
            var result = Record.Exception(() => archiver.Tar(null, "/file.tar", new FilePath[] { "/Root/file.txt" }));

            // Then
            AssertEx.IsArgumentNullException(result, "rootPath");
        }

        [Fact]
        public void Should_Throw_If_Output_Path_Is_Null()
        {
            // Given
            var archiver = CreateArchiver();

            // When
            var result = Record.Exception(() => archiver.Tar("/Root", null, new FilePath[] { "/Root/file.txt" }));

            // Then
            AssertEx.IsArgumentNullException(result, "outputPath");
        }

        [Fact]
        public void Should_Throw_If_File_Paths_Are_Null()
        {
            // Given
            var archiver = CreateArchiver();

            // When
            var result = Record.Exception(() => archiver.Tar("/Root", "/file.tar", (IEnumerable<FilePath>)null));

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
            var archiver = new TarArchiver(fileSystem, environment, Substitute.For<ICakeLog>());

            // When
            var result = Record.Exception(() => archiver.Tar("/Root", "/file.tar", new FilePath[] { "/NotRoot/file.txt" }));

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("Path '/NotRoot/file.txt' is not relative to root path '/Root'.", result?.Message);
        }

        [Fact]
        public void Should_Tar_Provided_Directory()
        {
            // Given
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            var globber = new Globber(fileSystem, environment);
            var context = new CakeContextFixture { Environment = environment, FileSystem = fileSystem, Globber = globber }.CreateContext();
            fileSystem.CreateDirectory("/Dir0");
            fileSystem.CreateFile("/File1.txt").SetContent("1");
            fileSystem.CreateFile("/Dir1/File2.txt").SetContent("22");
            fileSystem.CreateFile("/Dir2/File3.txt").SetContent("333");
            fileSystem.CreateFile("/Dir2/Dir3/File4.txt").SetContent("4444");
            fileSystem.CreateFile("/Dir2/Dir3/File5.txt").SetContent("55555");
            var archiver = new TarArchiver(fileSystem, environment, Substitute.For<ICakeLog>());

            // When
            archiver.Tar("/", "/Root.tar", context.GetPaths("/**/*"));

            // Then
            var entries = ReadEntries(fileSystem, "/Root.tar");
            Assert.Contains("Dir0", entries.Keys);
            Assert.Contains("Dir1", entries.Keys);
            Assert.Contains("Dir2", entries.Keys);
            Assert.Contains("Dir2/Dir3", entries.Keys);
            Assert.Equal("1", entries["File1.txt"]);
            Assert.Equal("22", entries["Dir1/File2.txt"]);
            Assert.Equal("333", entries["Dir2/File3.txt"]);
            Assert.Equal("4444", entries["Dir2/Dir3/File4.txt"]);
            Assert.Equal("55555", entries["Dir2/Dir3/File5.txt"]);
        }

        [Fact]
        public void Should_Tar_Provided_Files()
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
            var archiver = new TarArchiver(fileSystem, environment, Substitute.For<ICakeLog>());

            // When
            archiver.Tar("/", "/Root.tar", context.GetFiles("/**/*.txt"));

            // Then
            var entries = ReadEntries(fileSystem, "/Root.tar");
            Assert.Contains("Dir1", entries.Keys);
            Assert.Contains("Dir2", entries.Keys);
            Assert.Contains("Dir2/Dir3", entries.Keys);
            Assert.Equal("1", entries["File1.txt"]);
            Assert.Equal("22", entries["Dir1/File2.txt"]);
            Assert.Equal("333", entries["Dir2/File3.txt"]);
            Assert.Equal("4444", entries["Dir2/Dir3/File4.txt"]);
            Assert.Equal("55555", entries["Dir2/Dir3/File5.txt"]);
        }

        [Fact]
        public void Should_Roundtrip_Gzip_File_Content()
        {
            // Given
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateFile("/Root/Stuff/file.txt").SetContent("HelloWorld");
            var archiver = new TarArchiver(fileSystem, environment, Substitute.For<ICakeLog>());

            // When
            archiver.Tar("/Root", "/file.tar.gz", new FilePath[] { "/Root/Stuff/file.txt" }, new TarSettings
            {
                Compression = TarCompression.GZip
            });
            archiver.Untar("/file.tar.gz", "/Out");

            // Then
            Assert.Equal("HelloWorld", fileSystem.GetFile("/Out/Stuff/file.txt").GetTextContent());
        }

        [Fact]
        public void Should_Throw_When_Overwrite_Is_False_And_Destination_Exists()
        {
            // Given
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateFile("/Root/file.txt").SetContent("1");
            fileSystem.CreateFile("/file.tar").SetContent("existing");
            var archiver = new TarArchiver(fileSystem, environment, Substitute.For<ICakeLog>());

            // When
            var result = Record.Exception(() => archiver.Tar("/Root", "/file.tar", new FilePath[] { "/Root/file.txt" }, new TarSettings
            {
                Overwrite = false
            }));

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("The file '/file.tar' already exists.", result?.Message);
        }

        [Fact]
        public void Should_Overwrite_Existing_Archive_When_Overwrite_Is_True()
        {
            // Given
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateFile("/Root/file.txt").SetContent("1");
            fileSystem.CreateFile("/file.tar").SetContent("existing");
            var archiver = new TarArchiver(fileSystem, environment, Substitute.For<ICakeLog>());

            // When
            archiver.Tar("/Root", "/file.tar", new FilePath[] { "/Root/file.txt" }, new TarSettings
            {
                Overwrite = true
            });

            // Then
            Assert.Equal("1", ReadEntries(fileSystem, "/file.tar")["file.txt"]);
        }

        private static TarArchiver CreateArchiver()
        {
            return new TarArchiver(Substitute.For<IFileSystem>(), Substitute.For<ICakeEnvironment>(), Substitute.For<ICakeLog>());
        }
    }

    public sealed class TheUntarMethod
    {
        [Fact]
        public void Should_Throw_If_Archive_Path_Is_Null()
        {
            // Given
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            var archiver = new TarArchiver(fileSystem, environment, Substitute.For<ICakeLog>());

            // When
            var result = Record.Exception(() => archiver.Untar((FilePath)null, "/Out"));

            // Then
            AssertEx.IsArgumentNullException(result, "archivePath");
        }

        [Fact]
        public void Should_Throw_If_Output_Path_Is_Null()
        {
            // Given
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            var archiver = new TarArchiver(fileSystem, environment, Substitute.For<ICakeLog>());

            // When
            var result = Record.Exception(() => archiver.Untar("/file.tar", null));

            // Then
            AssertEx.IsArgumentNullException(result, "outputPath");
        }

        [Fact]
        public void Should_Untar_Plain_Archive()
        {
            // Given
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateFile("/Root/file.txt").SetContent("Hello");
            var archiver = new TarArchiver(fileSystem, environment, Substitute.For<ICakeLog>());
            archiver.Tar("/Root", "/file.tar", new FilePath[] { "/Root/file.txt" });

            // When
            archiver.Untar("/file.tar", "/Out");

            // Then
            Assert.Equal("Hello", fileSystem.GetFile("/Out/file.txt").GetTextContent());
        }

        [Fact]
        public void Should_Skip_Unchanged_Files()
        {
            // Given
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            var source = fileSystem.CreateFile("/Root/file.txt").SetContent("old");
            source.SetLastWriteTimeUtc(new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc));
            var archiver = new TarArchiver(fileSystem, environment, Substitute.For<ICakeLog>());
            archiver.Tar("/Root", "/file.tar", new FilePath[] { "/Root/file.txt" });
            archiver.Untar("/file.tar", "/Out", new UntarSettings { SkipUnchangedFiles = false });
            var destination = fileSystem.GetFile("/Out/file.txt");
            destination.SetContent("new");
            destination.SetLastWriteTimeUtc(new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            // When
            archiver.Untar("/file.tar", "/Out", new UntarSettings { SkipUnchangedFiles = true });

            // Then
            Assert.Equal("new", destination.GetTextContent());
        }

        [Fact]
        public void Should_Throw_When_Destination_Is_Read_Only()
        {
            // Given
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateFile("/Root/file.txt").SetContent("old");
            var archiver = new TarArchiver(fileSystem, environment, Substitute.For<ICakeLog>());
            archiver.Tar("/Root", "/file.tar", new FilePath[] { "/Root/file.txt" });
            archiver.Untar("/file.tar", "/Out", new UntarSettings { SkipUnchangedFiles = false });
            var destination = fileSystem.GetFile("/Out/file.txt");
            destination.Attributes = FileAttributes.ReadOnly;
            destination.SetLastWriteTimeUtc(new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            // When
            var result = Record.Exception(() => archiver.Untar("/file.tar", "/Out", new UntarSettings
            {
                SkipUnchangedFiles = false,
                OverwriteReadOnlyFiles = false
            }));

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("The file '/Out/file.txt' is read-only.", result?.Message);
        }

        [Fact]
        public void Should_Overwrite_Read_Only_Files_When_Enabled()
        {
            // Given
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateFile("/Root/file.txt").SetContent("updated");
            var archiver = new TarArchiver(fileSystem, environment, Substitute.For<ICakeLog>());
            archiver.Tar("/Root", "/file.tar", new FilePath[] { "/Root/file.txt" });
            fileSystem.CreateFile("/Out/file.txt").SetContent("old");
            var destination = fileSystem.GetFile("/Out/file.txt");
            destination.Attributes = FileAttributes.ReadOnly;
            destination.SetLastWriteTimeUtc(new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            // When
            archiver.Untar("/file.tar", "/Out", new UntarSettings
            {
                SkipUnchangedFiles = false,
                OverwriteReadOnlyFiles = true
            });

            // Then
            Assert.Equal("updated", destination.GetTextContent());
        }

        [Fact]
        public void Should_Reject_Parent_Directory_Entry()
        {
            // Given
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            WriteCraftedArchive(fileSystem, "/evil.tar", "../evil.txt");
            var archiver = new TarArchiver(fileSystem, environment, Substitute.For<ICakeLog>());

            // When
            var result = Record.Exception(() => archiver.Untar("/evil.tar", "/Out"));

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("The archive entry '../evil.txt' is not a valid relative path.", result?.Message);
        }
    }

    private static Dictionary<string, string> ReadEntries(FakeFileSystem fileSystem, FilePath archivePath)
    {
        var entries = new Dictionary<string, string>(StringComparer.Ordinal);
        using var stream = fileSystem.GetFile(archivePath).Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        Stream archiveStream = stream;
        if (stream.Length >= 2)
        {
            var isGzip = stream.ReadByte() == 0x1F && stream.ReadByte() == 0x8B;
            stream.Position = 0;
            if (isGzip)
            {
                archiveStream = new GZipStream(stream, CompressionMode.Decompress, leaveOpen: true);
            }
        }

        using (archiveStream == stream ? null : archiveStream)
        using (var reader = new TarReader(archiveStream, leaveOpen: true))
        {
            TarEntry entry;
            while ((entry = reader.GetNextEntry()) != null)
            {
                if (entry.DataStream == null)
                {
                    entries[entry.Name.Trim('/')] = null;
                    continue;
                }

                using var memory = new MemoryStream();
                entry.DataStream.CopyTo(memory);
                entries[entry.Name.Trim('/')] = Encoding.UTF8.GetString(memory.ToArray());
            }
        }

        return entries;
    }

    private static void WriteCraftedArchive(FakeFileSystem fileSystem, FilePath archivePath, string entryName)
    {
        var file = fileSystem.CreateFile(archivePath);
        using var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None);
        using var writer = new TarWriter(stream, TarEntryFormat.Pax, leaveOpen: true);
        using var content = new MemoryStream(Encoding.UTF8.GetBytes("bad"));
        var entry = new PaxTarEntry(TarEntryType.RegularFile, entryName)
        {
            DataStream = content
        };
        writer.WriteEntry(entry);
    }
}
