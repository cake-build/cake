using System;
using System.IO;
using Cake.Core.IO;

namespace Cake.Testing.Fakes
{
    public sealed class FakeFile : IFile
    {
        private readonly FakeFileSystem _fileSystem;
        private readonly FilePath _path;
        private bool _exists;
        private byte[] _content = new byte[4096];
        private long _contentLength;
        private readonly object _contentLock = new object();
        private bool _deleted;
        private bool _hidden;

        public FilePath Path
        {
            get { return _path; }
        }

        Core.IO.Path IFileSystemInfo.Path
        {
            get { return _path; }
        }

        public bool Exists
        {
            get { return _exists; }
            set { _exists = value; } 
        }

         public bool Hidden
        {
            get { return _hidden; }
            set { _hidden = value; } 
        }

        public bool Deleted
        {
            get { return _deleted; }
        }

        public long Length
        {
            get { return _contentLength; }
        }

        public object ContentLock
        {
            get { return _contentLock; }
        }

        public long ContentLength
        {
            get { return _contentLength; }
            set { _contentLength = value; }
        }

        public byte[] Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public FakeFile(FakeFileSystem fileSystem, FilePath path)
        {
            _fileSystem = fileSystem;
            _path = path;
            _exists = false;
            _hidden = false;
        }

        public void Copy(FilePath destination, bool overwrite)
        {
            var file = _fileSystem.GetCreatedFile(destination) as FakeFile;
            if (file != null)
            {
                file.Content = Content;
                file.ContentLength = ContentLength;
            }
        }

        public void Move(FilePath destination)
        {
            throw new NotImplementedException();
        }

        public Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            var position = GetPosition(fileMode);
            var stream = new FakeFileStream(this) {Position = position};
            return stream;
        }

        public void Resize(long offset)
        {
            if (_contentLength < offset)
            {
                _contentLength = offset;
            }
            if (_content.Length >= _contentLength)
            {
                return;
            }

            var buffer = new byte[_contentLength*2];
            Buffer.BlockCopy(_content, 0, buffer, 0, _content.Length);
            _content = buffer;
        }

        private long GetPosition(FileMode fileMode)
        {
            if (Exists)
            {
                switch (fileMode)
                {
                    case FileMode.CreateNew:
                        throw new IOException();
                    case FileMode.Create:
                    case FileMode.Truncate:
                        _contentLength = 0;
                        return 0;
                    case FileMode.Open:
                    case FileMode.OpenOrCreate:
                        return 0;
                    case FileMode.Append:
                        return _contentLength;
                }
            }
            else
            {
                switch (fileMode)
                {
                    case FileMode.Create:
                    case FileMode.Append:
                    case FileMode.CreateNew:
                    case FileMode.OpenOrCreate:
                        _exists = true;
                        return _contentLength;
                    case FileMode.Open:
                    case FileMode.Truncate:
                        throw new FileNotFoundException();
                }
            }
            throw new NotSupportedException();
        }

        public void Delete()
        {
            _exists = false;
            _deleted = true;
        }
    }
}