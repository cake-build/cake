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

        /// <inheritdoc/>
        public FilePath Path { get; }

        /// <inheritdoc/>
        Path IFileSystemInfo.Path => Path;

        /// <inheritdoc/>
        public bool Exists { get; internal set; }

        /// <inheritdoc/>
        public bool Hidden { get; internal set; }

        /// <summary>
        /// Gets the time when this <see cref="IFileSystemInfo"/> was last written to.
        /// </summary>
        /// <value>The last write time.</value>
        public DateTime LastWriteTime { get; private set; }

        /// <summary>
        /// Gets the date and time, in Coordinated Universal Time (UTC), that the file was last written to.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that represents the last write time in UTC, or <c>null</c> if not available.
        /// </value>
        public DateTime? LastWriteTimeUtc { get; private set; }

        /// <summary>
        /// Gets the date and time, in Coordinated Universal Time (UTC), that the file was created.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that represents the creation time in UTC, or <c>null</c> if not available.
        /// </value>
        public DateTime? CreationTimeUtc { get; private set; }

        /// <summary>
        /// Gets the date and time, in Coordinated Universal Time (UTC), that the file was last accessed.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that represents the last access time in UTC, or <c>null</c> if not available.
        /// </value>
        public DateTime? LastAccessTimeUtc { get; private set; }

        /// <summary>
        /// Gets the Unix file mode of the entry.
        /// </summary>
        /// <value>
        /// A <see cref="UnixFileMode"/> value that represents the Unix file mode of the entry.
        /// </value>
        public UnixFileMode? UnixFileMode { get; private set; }

        /// <inheritdoc/>
        public long Length { get; private set; }

        /// <inheritdoc/>
        public FileAttributes Attributes { get; set; }

        internal object ContentLock { get; } = new object();

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
        public byte[] Content { get; internal set; }

        internal FakeFile(FakeFileSystemTree tree, FilePath path)
        {
            _tree = tree;
            Path = path;
            Reset();
        }

        /// <inheritdoc/>
        public void Copy(FilePath destination, bool overwrite)
        {
            _tree.CopyFile(this, destination, overwrite);
        }

        /// <inheritdoc/>
        public void Move(FilePath destination)
        {
            _tree.MoveFile(this, destination);
        }

        /// <inheritdoc/>
        public Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            var position = GetPosition(fileMode, out bool fileWasCreated);
            if (fileWasCreated)
            {
                _tree.CreateFile(this);
            }
            return new FakeFileStream(this) { Position = position };
        }

        /// <inheritdoc/>
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
            if (Content.Length >= Length)
            {
                return;
            }
            var buffer = new byte[Length * 2];
            Buffer.BlockCopy(Content, 0, buffer, 0, Content.Length);
            Content = buffer;
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
                        var now = _tree.GetUtcNow();
                        SetLastWriteTimeUtc(now);
                        SetLastAccessTimeUtc(now);
                        SetCreationTimeUtc(now);
                        UnixFileMode = _tree.DefaultUnixCreateFileMode;
                        return Length;
                    case FileMode.Open:
                    case FileMode.Truncate:
                        throw FakeFileSystemTree.ThrowIfNotFound(this);
                }
            }
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public IFile SetCreationTime(DateTime creationTime)
        {
            FakeFileSystemTree.ThrowIfNotFound(this);
            CreationTimeUtc = creationTime.ToUniversalTime();
            return this;
        }

        /// <inheritdoc/>
        public IFile SetCreationTimeUtc(DateTime creationTimeUtc)
        {
            FakeFileSystemTree.ThrowIfNotFound(this);
            CreationTimeUtc = creationTimeUtc;
            return this;
        }

        /// <inheritdoc/>
        public IFile SetLastAccessTime(DateTime lastAccessTime)
        {
            FakeFileSystemTree.ThrowIfNotFound(this);
            LastAccessTimeUtc = lastAccessTime.ToUniversalTime();
            return this;
        }

        /// <inheritdoc/>
        public IFile SetLastAccessTimeUtc(DateTime lastAccessTimeUtc)
        {
            FakeFileSystemTree.ThrowIfNotFound(this);
            LastAccessTimeUtc = lastAccessTimeUtc;
            return this;
        }

        /// <inheritdoc/>
        public IFile SetLastWriteTime(DateTime lastWriteTime)
        {
            FakeFileSystemTree.ThrowIfNotFound(this);
            LastWriteTime = lastWriteTime;
            LastWriteTimeUtc = lastWriteTime.ToUniversalTime();
            return this;
        }

        /// <inheritdoc/>
        public IFile SetLastWriteTimeUtc(DateTime lastWriteTimeUtc)
        {
            FakeFileSystemTree.ThrowIfNotFound(this);
            LastWriteTimeUtc = lastWriteTimeUtc;
            LastWriteTime = lastWriteTimeUtc.ToLocalTime();
            return this;
        }

        /// <inheritdoc/>
        public IFile SetUnixFileMode(UnixFileMode unixFileMode)
        {
            if (!_tree.IsUnix)
            {
                throw new PlatformNotSupportedException("Setting Unix file mode is not supported on Windows platforms.");
            }
            FakeFileSystemTree.ThrowIfNotFound(this);
            UnixFileMode = unixFileMode;
            return this;
        }

        /// <summary>
        /// Sets the last write time of the file to the current UTC time.
        /// </summary>
        /// <returns>The current <see cref="IFile"/> instance.</returns>
        public IFile SetLastWriteNow()
        {
            return SetLastWriteTimeUtc(_tree.GetUtcNow());
        }

        internal void Reset()
        {
            Exists = false;
            Hidden = false;
            LastWriteTime = default;
            CreationTimeUtc = null;
            LastAccessTimeUtc = null;
            LastWriteTimeUtc = null;
            UnixFileMode = null;
            ContentLength = 0;
            Attributes = default;
            Content = new byte[4096];
        }
    }
}
