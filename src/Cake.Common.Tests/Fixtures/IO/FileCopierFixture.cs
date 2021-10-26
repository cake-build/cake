using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tests.Fixtures;
using Cake.Testing;

namespace Cake.Common.Tests.Fixtures.IO
{
    internal sealed class FileCopierFixture
    {
        private readonly FakeEnvironment _environment;
        private readonly FakeFileSystem _fileSystem;
        private readonly IGlobber _globber;

        public FileCopierFixture()
        {
            _environment = FakeEnvironment.CreateUnixEnvironment();
            _environment.WorkingDirectory = "/working";
            _fileSystem = new FakeFileSystem(_environment);
            _globber = new Globber(_fileSystem, _environment);
            Context = new CakeContextFixture { Environment = _environment, FileSystem = _fileSystem, Globber = _globber }.CreateContext();
        }

        public CakeContext Context { get; }

        public bool ExistsFile(FilePath path)
        {
            var file = _fileSystem.GetFile(path.MakeAbsolute(Context.Environment));
            return file != null;
        }

        public FilePath MakeAbsolute(FilePath inputPath)
        {
            return inputPath.MakeAbsolute(Context.Environment);
        }

        public void EnsureFileExists(FilePath filePath)
        {
            var fullPath = filePath.MakeAbsolute(Context.Environment);
            _fileSystem.CreateFile(fullPath).SetContent(Guid.NewGuid().ToString("D"));
        }

        public void EnsureDirectoryExists(DirectoryPath directoryPath)
        {
            var fullPath = directoryPath.MakeAbsolute(Context.Environment);
            _fileSystem.CreateDirectory(fullPath);
        }
    }
}