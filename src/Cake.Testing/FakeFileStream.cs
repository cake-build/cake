// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.IO;

namespace Cake.Testing
{
    internal sealed class FakeFileStream : Stream
    {
        private readonly FakeFile _file;
        private long _position;

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public FakeFileStream(FakeFile file)
        {
            _file = file;
            _position = 0;
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get
            {
                lock (_file.ContentLock)
                {
                    return _file.ContentLength;
                }
            }
        }

        public override long Position
        {
            get { return _position; }
            set { Seek(value, SeekOrigin.Begin); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (_file.ContentLock)
            {
                var end = _position + count;
                var fileSize = _file.ContentLength;
                var maxLengthToRead = end > fileSize ? fileSize - _position : count;
                Buffer.BlockCopy(_file.Content, (int)_position, buffer, offset, (int)maxLengthToRead);
                _position += maxLengthToRead;
                return (int)maxLengthToRead;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
            {
                return MoveTo(offset);
            }
            if (origin == SeekOrigin.Current)
            {
                return MoveTo(_position + offset);
            }
            if (origin == SeekOrigin.End)
            {
                return MoveTo(_file.ContentLength - offset);
            }
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            lock (_file.ContentLock)
            {
                _file.Resize(value);
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            lock (_file.ContentLock)
            {
                var fileSize = _file.ContentLength;
                var endOfWrite = _position + count;
                if (endOfWrite > fileSize)
                {
                    _file.Resize(endOfWrite);
                }
                Buffer.BlockCopy(buffer, offset, _file.Content, (int)_position, count);
                _position = _position + count;
            }
        }

        private long MoveTo(long offset)
        {
            lock (_file.ContentLock)
            {
                if (offset < 0)
                {
                    throw new InvalidOperationException();
                }
                if (offset > _file.ContentLength)
                {
                    _file.Resize(offset);
                }
                _position = offset;
                return offset;
            }
        }
    }
}
