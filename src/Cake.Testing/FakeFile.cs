// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.IO;
using Cake.Core.IO;
using Path = Cake.Core.IO.Path;

namespace Cake.Testing
{
    /// <summary>
    /// Represents a fake file.
    /// </summary>
    public sealed class FakeFile : IFile
    {
        private readonly FakeFileSystemTree _tree;
        private readonly FilePath _path;
        private readonly object _contentLock = new object();
        private byte[] _content = new byte[4096];

        /// <summary>
        /// Gets the path to the file.
        /// </summary>
        /// <value>The path.</value>
        public FilePath Path
        {
            get { return _path; }
        }

        Path IFileSystemInfo.Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IFileSystemInfo" /> exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the entry exists; otherwise, <c>false</c>.
        /// </value>
        public bool Exists { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IFileSystemInfo" /> is hidden.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the entry is hidden; otherwise, <c>false</c>.
        /// </value>
        public bool Hidden { get; internal set; }

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <value>
        /// The length of the file.
        /// </value>
        public long Length { get; private set; }

        internal object ContentLock
        {
            get { return _contentLock; }
        }

        /// <summary>
        /// Gets the length of the content.
        /// </summary>
        /// <value>
        /// The length of the content.
        /// </value>
        public long ContentLength
        {
            get { return Length; }
            internal set { Length = value; }
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>The content.</value>
        public byte[] Content
        {
            get { return _content; }
            internal set { _content = value; }
        }

        internal FakeFile(FakeFileSystemTree tree, FilePath path)
        {
            _tree = tree;
            _path = path;
            Exists = false;
            Hidden = false;
        }

        /// <summary>
        /// Copies the file to the specified destination path.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        /// <param name="overwrite">Will overwrite existing destination file if set to <c>true</c>.</param>
        public void Copy(FilePath destination, bool overwrite)
        {
            _tree.CopyFile(this, destination, overwrite);
        }

        /// <summary>
        /// Moves the file to the specified destination path.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        public void Move(FilePath destination)
        {
            _tree.MoveFile(this, destination);
        }

        /// <summary>
        /// Opens the file using the specified options.
        /// </summary>
        /// <param name="fileMode">The file mode.</param>
        /// <param name="fileAccess">The file access.</param>
        /// <param name="fileShare">The file share.</param>
        /// <returns>A <see cref="Stream" /> to the file.</returns>
        public Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            bool fileWasCreated;
            var position = GetPosition(fileMode, out fileWasCreated);
            if (fileWasCreated)
            {
                _tree.CreateFile(this);
            }
            return new FakeFileStream(this) { Position = position };
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        public void Delete()
        {
            _tree.DeleteFile(this);
        }

        /// <summary>
        /// Resizes the file.
        /// </summary>
        /// <param name="offset">The offset.</param>
        public void Resize(long offset)
        {
            if (Length < offset)
            {
                Length = offset;
            }
            if (_content.Length >= Length)
            {
                return;
            }
            var buffer = new byte[Length * 2];
            Buffer.BlockCopy(_content, 0, buffer, 0, _content.Length);
            _content = buffer;
        }

        private long GetPosition(FileMode fileMode, out bool fileWasCreated)
        {
            fileWasCreated = false;

            if (Exists)
            {
                switch (fileMode)
                {
                    case FileMode.CreateNew:
                        throw new IOException();
                    case FileMode.Create:
                    case FileMode.Truncate:
                        Length = 0;
                        return 0;
                    case FileMode.Open:
                    case FileMode.OpenOrCreate:
                        return 0;
                    case FileMode.Append:
                        return Length;
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
                        fileWasCreated = true;
                        Exists = true;
                        return Length;
                    case FileMode.Open:
                    case FileMode.Truncate:
                        throw new FileNotFoundException();
                }
            }
            throw new NotSupportedException();
        }
    }
}
