using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.IO.Tests.Fixtures
{
    public sealed class FileCopyFixture
    {
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public ICakeContext Context { get; set; }
        public IGlobber Globber { get; set; }
        public IDirectory TargetDirectory { get; set; }

        public List<FilePath> SourceFilePaths { get; set; }
        public List<IFile> TargetFiles { get; set; }
        public List<FilePath> TargetFilePaths { get; set; }

        public FileCopyFixture()
        {
            TargetDirectory = Substitute.For<IDirectory>();
            TargetDirectory.Exists.Returns(true);

            SourceFilePaths = new List<FilePath>();
            TargetFiles = new List<IFile>();
            TargetFilePaths = new List<FilePath>();
            CreateTargetFile("./file1.txt", "/Working/file1.txt");
            CreateTargetFile("./file2.txt", "/Working/file2.txt");

            Globber = Substitute.For<IGlobber>();
            Globber.Match("*").Returns(c => TargetFilePaths);

            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.GetDirectory(Arg.Is<DirectoryPath>(p => p.FullPath == "/Working/target")).Returns(c => TargetDirectory);
            FileSystem.GetFile(Arg.Is<FilePath>(p => p.FullPath == TargetFilePaths[0].FullPath)).Returns(c => TargetFiles[0]);
            FileSystem.GetFile(Arg.Is<FilePath>(p => p.FullPath == TargetFilePaths[1].FullPath)).Returns(c => TargetFiles[1]);

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            Context = Substitute.For<ICakeContext>();
            Context.FileSystem.Returns(FileSystem);
            Context.Environment.Returns(Environment);
            Context.Globber.Returns(Globber);
        }

        private void CreateTargetFile(FilePath sourcePath, FilePath targetPath)
        {
            var targetFile = Substitute.For<IFile>();
            targetFile.Exists.Returns(true);
            targetFile.Path.Returns(targetPath);

            SourceFilePaths.Add(sourcePath);
            TargetFiles.Add(targetFile);
            TargetFilePaths.Add(targetPath);
        }
    }
}