using System;
using System.Collections.Generic;
using System.IO;
using Cake.Core.IO;

namespace Cake.Core.Tests.Fakes
{
    public sealed class FakeFileSystem : IFileSystem
    {
        private readonly Dictionary<DirectoryPath, FakeDirectory> _directories;
        private readonly Dictionary<FilePath, FakeFile> _files;

        public Dictionary<DirectoryPath, FakeDirectory> Directories
        {
            get { return _directories; }
        }

        public Dictionary<FilePath, FakeFile> Files
        {
            get { return _files; }
        }

        public DirectoryPath WorkingDirectory { get; set; }

        public FakeFileSystem(bool isUnix)
        {
            _directories = new Dictionary<DirectoryPath, FakeDirectory>(new PathComparer(isUnix));
            _files = new Dictionary<FilePath, FakeFile>(new PathComparer(isUnix));
            WorkingDirectory = "/Working";
        }

        public IFile GetFile(FilePath path)
        {
            if (!Files.ContainsKey(path))
            {
                Files.Add(path, new FakeFile(path));
            }
            return Files[path];
        }

        public IFile GetCreatedFile(FilePath path)
        {
            var file = GetFile(path);
            file.Open(FileMode.Create, FileAccess.Write, FileShare.None).Close();
            return file;
        }

        public void DeleteDirectory(DirectoryPath path)
        {
            if (Directories.ContainsKey(path))
            {
                Directories[path].Exists = false;
            }
        }

        public IDirectory GetDirectory(DirectoryPath path)
        {
            return GetDirectory(path, creatable: true);
        }

        public DirectoryPath GetSpecialFolderPath(Environment.SpecialFolder folder)
        {
            throw new NotImplementedException();
        }

        public IDirectory GetCreatedDirectory(DirectoryPath path)
        {
            var directory = GetDirectory(path, creatable: true);
            directory.Create();
            return directory;
        }

        public IDirectory GetNonCreatableDirectory(DirectoryPath path)
        {
            return GetDirectory(path, creatable: false);
        }

        private IDirectory GetDirectory(DirectoryPath path, bool creatable)
        {
            if (!Directories.ContainsKey(path))
            {
                Directories.Add(path, new FakeDirectory(this, path, creatable));
            }
            return Directories[path];
        }
    }
}