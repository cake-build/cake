// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.IO
{
    internal sealed class FileCopyFixture
    {
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public ICakeContext Context { get; set; }
        public IGlobber Globber { get; set; }
        public IDirectory TargetDirectory { get; set; }
        public FakeLog Log { get; set; }

        public List<FilePath> SourceFilePaths { get; set; }
        public List<IFile> TargetFiles { get; set; }
        public List<FilePath> TargetFilePaths { get; set; }

        public FileCopyFixture()
        {
            // Setup the target directory.
            TargetDirectory = Substitute.For<IDirectory>();
            TargetDirectory.Exists.Returns(true);

            // Setup the files in the file system.
            SourceFilePaths = new List<FilePath>();
            TargetFiles = new List<IFile>();
            TargetFilePaths = new List<FilePath>();
            CreateTargetFile("./file1.txt", "/Working/file1.txt");
            CreateTargetFile("./file2.txt", "/Working/file2.txt");

            // Setup the globber to return all files for wild card.
            Globber = Substitute.For<IGlobber>();
            Globber.Match("*").Returns(c => TargetFilePaths);

            // Setup the file system to return correct directories when asked for.
            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.GetDirectory(Arg.Is<DirectoryPath>(p => p.FullPath == "/Working/target")).Returns(c => TargetDirectory);
            FileSystem.GetFile(Arg.Is<FilePath>(p => p.FullPath == TargetFilePaths[0].FullPath)).Returns(c => TargetFiles[0]);
            FileSystem.GetFile(Arg.Is<FilePath>(p => p.FullPath == TargetFilePaths[1].FullPath)).Returns(c => TargetFiles[1]);

            // Set the working directory.
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            // Setup the logger
            Log = new FakeLog();

            // Prepare the context.
            Context = Substitute.For<ICakeContext>();
            Context.FileSystem.Returns(FileSystem);
            Context.Environment.Returns(Environment);
            Context.Globber.Returns(Globber);
            Context.Log.Returns(Log);
        }

        private void CreateTargetFile(FilePath sourcePath, FilePath targetPath)
        {
            // Create the target file.
            var targetFile = Substitute.For<IFile>();
            targetFile.Exists.Returns(true);
            targetFile.Path.Returns(targetPath);

            // Add the file to lookup data structures.
            SourceFilePaths.Add(sourcePath);
            TargetFiles.Add(targetFile);
            TargetFilePaths.Add(targetPath);
        }
    }
}
