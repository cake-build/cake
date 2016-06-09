// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.IO
{
    internal sealed class FileDeleteFixture
    {
        private readonly List<IFile> _files;
        private readonly List<FilePath> _paths;
        private readonly Dictionary<string, IFile> _lookup;

        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public ICakeContext Context { get; set; }
        public IGlobber Globber { get; set; }

        public List<IFile> Files
        {
            get { return _files; }
        }

        public List<FilePath> Paths
        {
            get { return _paths; }
        }

        public FileDeleteFixture()
        {
            // Setup the files in the file system.
            _paths = new List<FilePath>();
            _files = new List<IFile>();
            _lookup = new Dictionary<string, IFile>();
            CreatFile("./file1.txt", "/Working/file1.txt");
            CreatFile("./file2.txt", "/Working/file2.txt");

            // Setup the globber to return all files for wild card.
            Globber = Substitute.For<IGlobber>();
            Globber.Match("*").Returns(c => _paths);

            // Setup the file system to return correct files when asked for.
            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.GetFile(Arg.Is<FilePath>(c => _lookup.ContainsKey(c.FullPath)))
                .Returns(c => _lookup[c.Arg<FilePath>().FullPath]);

            // Set the working directory.
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            // Prepare the context.
            Context = Substitute.For<ICakeContext>();
            Context.FileSystem.Returns(FileSystem);
            Context.Environment.Returns(Environment);
            Context.Globber.Returns(Globber);
        }

        private void CreatFile(FilePath relativePath, FilePath absolutePath)
        {
            // Create the target file.
            var file = Substitute.For<IFile>();
            file.Exists.Returns(true);
            file.Path.Returns(absolutePath);

            // Add to collections.
            _paths.Add(relativePath);
            _files.Add(file);
            _lookup.Add(absolutePath.FullPath, file);
        }
    }
}
