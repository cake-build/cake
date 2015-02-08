using System;
using System.Collections.Generic;
using System.IO;
using Cake.Core.IO;

namespace Cake.Testing.Fakes
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

        public FakeFileSystem(bool isUnix)
        {
            _directories = new Dictionary<DirectoryPath, FakeDirectory>(new PathComparer(isUnix));
            _files = new Dictionary<FilePath, FakeFile>(new PathComparer(isUnix));
        }

        public IFile GetFile(FilePath path)
        {
            return GetFile(path, false);
        }

        public IFile GetFile(FilePath path, bool hidden)
        {
            if (!Files.ContainsKey(path))
            {
                Files.Add(path, new FakeFile(this, path){Hidden = hidden});
            }
            return Files[path];
        }

        public IFile GetCreatedFile(FilePath path)
        {
            return GetCreatedFile(path, false);
        }

        public IFile GetCreatedFile(FilePath path, bool hidden)
        {
            var file = GetFile(path,hidden);
            file.Open(FileMode.Create, FileAccess.Write, FileShare.None).Close();
            return file;
        }

        public IFile GetCreatedFile(FilePath path, string content)
        {
            var file = GetFile(path);
            var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None);
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Close();
            stream.Close();
            return file;
        }

        public IDirectory GetDirectory(DirectoryPath path)
        {
            return GetDirectory(path, creatable: true, hidden:false);
        }

        public IDirectory GetCreatedDirectory(DirectoryPath path)
        {
            return GetCreatedDirectory(path, false);
        }

        public IDirectory GetCreatedDirectory(DirectoryPath path, bool hidden)
        {
            var directory = GetDirectory(path, creatable: true, hidden:hidden);
            directory.Create();
            return directory;
        }

        private IDirectory GetDirectory(DirectoryPath path, bool creatable, bool hidden)
        {
            if (!Directories.ContainsKey(path))
            {
                Directories.Add(path, new FakeDirectory(this, path, creatable, hidden));
            }
            return Directories[path];
        }

        public string GetTextContent(FilePath path)
        {
            var file = GetFile(path) as FakeFile;
            if (file == null)
            {
                throw new InvalidOperationException();
            }

            try
            {
                if (file.Deleted)
                {
                    Files[path].Exists = true;
                }

                using (var stream = file.OpenRead())
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            finally
            {
                if (file.Deleted)
                {
                    Files[path].Exists = false;
                }
            }
        }
    }
}