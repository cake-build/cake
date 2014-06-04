using System;
using System.IO;
using Cake.Core.IO;

namespace Cake.Core.Tests.Fakes
{
    public sealed class FakeFile : IFile
    {
        private readonly FilePath _path;
        private bool _exists;
        private byte[] _content = new byte[4096];
        private long _contentLength;
        private readonly object _contentLock = new object();

        public FilePath Path
        {
            get { return _path; }
        }

        public bool Exists
        {
            get { return _exists; }
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

        public FakeFile(FilePath path)
        {
            _path = path;
            _exists = false;
        }

        public void Copy(FilePath destination)
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
    }
}