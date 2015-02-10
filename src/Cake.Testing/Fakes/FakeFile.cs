using System;
using System.IO;
using Cake.Core.IO;

namespace Cake.Testing.Fakes
{
    /// <summary>
    /// Implementation of a fake <see cref="IFile"/>.
    /// </summary>
    public sealed class FakeFile : IFile
    {
        private readonly FakeFileSystem _fileSystem;
        private readonly FilePath _path;
        private readonly object _contentLock = new object();
        private bool _exists;
        private byte[] _content = new byte[4096];
        private long _contentLength;        
        private bool _deleted;
        private bool _hidden;

        /// <summary>
        /// Gets the path to the file.
        /// </summary>
        /// <value>The path.</value>
        public FilePath Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets the path to the file.
        /// </summary>
        /// <value>The path.</value>
        Core.IO.Path IFileSystemInfo.Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IFileSystemInfo" /> exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the entry exists; otherwise, <c>false</c>.
        /// </value>
        public bool Exists
        {
            get { return _exists; }
            set { _exists = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IFileSystemInfo" /> is hidden.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the entry is hidden; otherwise, <c>false</c>.
        /// </value>
        public bool Hidden
        {
            get { return _hidden; }
            set { _hidden = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="FakeFile"/> is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if deleted; otherwise, <c>false</c>.
        /// </value>
        public bool Deleted
        {
            get { return _deleted; }
        }

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <value>
        /// The length of the file.
        /// </value>
        public long Length
        {
            get { return _contentLength; }
        }

        /// <summary>
        /// Gets the content lock.
        /// </summary>
        /// <value>
        /// The content lock.
        /// </value>
        public object ContentLock
        {
            get { return _contentLock; }
        }

        /// <summary>
        /// Gets or sets the length of the content.
        /// </summary>
        /// <value>The length of the content.</value>
        public long ContentLength
        {
            get { return _contentLength; }
            set { _contentLength = value; }
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public byte[] Content
        {
            get { return _content; }
            set { _content = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeFile"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path.</param>
        public FakeFile(FakeFileSystem fileSystem, FilePath path)
        {
            _fileSystem = fileSystem;
            _path = path;
            _exists = false;
            _hidden = false;
        }

        /// <summary>
        /// Copies the file to the specified destination path.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        /// <param name="overwrite">Will overwrite existing destination file if set to <c>true</c>.</param>
        public void Copy(FilePath destination, bool overwrite)
        {
            var file = _fileSystem.GetCreatedFile(destination) as FakeFile;
            if (file != null)
            {
                file.Content = Content;
                file.ContentLength = ContentLength;
            }
        }

        /// <summary>
        /// Moves the file to the specified destination path.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        public void Move(FilePath destination)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opens the file using the specified options.
        /// </summary>
        /// <param name="fileMode">The file mode.</param>
        /// <param name="fileAccess">The file access.</param>
        /// <param name="fileShare">The file share.</param>
        /// <returns>
        /// A <see cref="Stream" /> to the file.
        /// </returns>
        public Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            var position = GetPosition(fileMode);
            var stream = new FakeFileStream(this) { Position = position };
            return stream;
        }

        /// <summary>
        /// Resizes the specified offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
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

            var buffer = new byte[_contentLength * 2];
            Buffer.BlockCopy(_content, 0, buffer, 0, _content.Length);
            _content = buffer;
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        public void Delete()
        {
            _exists = false;
            _deleted = true;
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