using System;
using System.IO;

namespace Cake.Testing.Fakes
{
    /// <summary>
    /// Implementation of a fake <see cref="Stream"/> used by <see cref="FakeFileSystem"/>.
    /// </summary>
    public sealed class FakeFileStream : Stream
    {
        private readonly FakeFile _file;
        private long _position;

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite
        {
            get { return true; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeFileStream"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public FakeFileStream(FakeFile file)
        {
            _file = file;
            _position = 0;
        }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
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

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        public override long Position
        {
            get { return _position; }
            set { Seek(value, SeekOrigin.Begin); }
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
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

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
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

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        public override void SetLength(long value)
        {
            lock (_file.ContentLock)
            {
                _file.Resize(value);
            }
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
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