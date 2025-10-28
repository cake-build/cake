// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Cake.Core.IO;

namespace Cake.Testing
{
    /// <summary>
    /// Represents a fake directory.
    /// </summary>
    [DebuggerDisplay("{Path,nq}")]
    public sealed class FakeDirectory : IDirectory
    {
        private readonly FakeFileSystemTree _tree;

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
        /// Gets the date and time, in Coordinated Universal Time (UTC), that the directory was last written to.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that represents the last write time in UTC, or <c>null</c> if not available.
        /// </value>
        public DateTime? LastWriteTimeUtc { get; private set; }

        /// <summary>
        /// Gets the date and time, in Coordinated Universal Time (UTC), that the directory was created.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that represents the creation time in UTC, or <c>null</c> if not available.
        /// </value>
        public DateTime? CreationTimeUtc { get; private set; }

        /// <summary>
        /// Gets the date and time, in Coordinated Universal Time (UTC), that the directory was last accessed.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that represents the last access time in UTC, or <c>null</c> if not available.
        /// </value>
        public DateTime? LastAccessTimeUtc { get; private set; }

        /// <summary>
        /// Gets the Unix file mode of the entry.
        /// </summary>
        /// <value>
        /// A <see cref="System.IO.UnixFileMode"/> value that represents the Unix file mode of the entry.
        /// </value>
        public System.IO.UnixFileMode? UnixFileMode { get; internal set; }

        /// <inheritdoc/>
        public DirectoryPath Path { get; }

        /// <inheritdoc/>
        Path IFileSystemInfo.Path => Path;

        internal FakeDirectory Parent { get; set; }

        internal FakeDirectoryContent Content { get; }

        internal FakeDirectory(FakeFileSystemTree tree, DirectoryPath path)
        {
            _tree = tree;
            Path = path;
            Content = new FakeDirectoryContent(this, tree.Comparer);
            CreationTimeUtc = null;
            LastAccessTimeUtc = null;
            LastWriteTimeUtc = null;
            UnixFileMode = null;
        }

        /// <inheritdoc/>
        public void Create()
        {
            _tree.CreateDirectory(this);
        }

        /// <inheritdoc/>
        public void Move(DirectoryPath destination)
        {
            _tree.MoveDirectory(this, destination);
        }

        /// <inheritdoc/>
        public void Delete(bool recursive)
        {
            _tree.DeleteDirectory(this, recursive);
        }

        /// <inheritdoc/>
        public IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope)
        {
            var stack = new Stack<FakeDirectory>();
            foreach (var child in Content.Directories.Values)
            {
                if (child.Exists)
                {
                    stack.Push(child);
                }
            }

            // Rewrite the filter to a regex expression.
            var expression = CreateRegex(filter);

            while (stack.Count > 0)
            {
                // Pop a directory from the stack.
                var current = stack.Pop();

                // Is this a match? In that case, add it to the result.
                if (expression.IsMatch(current.Path.GetDirectoryName()))
                {
                    yield return current;
                }

                // Recurse?
                if (scope == SearchScope.Recursive)
                {
                    foreach (var child in current.Content.Directories.Values)
                    {
                        if (child.Exists)
                        {
                            stack.Push(child);
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope)
        {
            var stack = new Stack<FakeDirectory>();

            // Rewrite the filter to a regex expression.
            var expression = CreateRegex(filter);

            // Just interested in this directory?
            if (scope == SearchScope.Current)
            {
                foreach (var file in GetFiles(this, expression))
                {
                    yield return file;
                }
                yield break;
            }

            stack.Push(this);
            while (stack.Count > 0)
            {
                // Pop a directory from the stack.
                var current = stack.Pop();

                // Add all files we can find.
                foreach (var file in GetFiles(current, expression))
                {
                    yield return file;
                }

                // Recurse?
                if (scope == SearchScope.Recursive)
                {
                    foreach (var child in current.Content.Directories.Values)
                    {
                        if (child.Exists)
                        {
                            stack.Push(child);
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public IDirectory SetCreationTime(DateTime creationTime)
        {
            FakeFileSystemTree.ThrowIfNotFound(this);
            CreationTimeUtc = creationTime.ToUniversalTime();
            return this;
        }

        /// <inheritdoc/>
        public IDirectory SetCreationTimeUtc(DateTime creationTimeUtc)
        {
            FakeFileSystemTree.ThrowIfNotFound(this);
            CreationTimeUtc = creationTimeUtc;
            return this;
        }

        /// <inheritdoc/>
        public IDirectory SetLastAccessTime(DateTime lastAccessTime)
        {
            FakeFileSystemTree.ThrowIfNotFound(this);
            LastAccessTimeUtc = lastAccessTime.ToUniversalTime();
            return this;
        }

        /// <inheritdoc/>
        public IDirectory SetLastAccessTimeUtc(DateTime lastAccessTimeUtc)
        {
            FakeFileSystemTree.ThrowIfNotFound(this);
            LastAccessTimeUtc = lastAccessTimeUtc;
            return this;
        }

        /// <inheritdoc/>
        public IDirectory SetLastWriteTime(DateTime lastWriteTime)
        {
            FakeFileSystemTree.ThrowIfNotFound(this);
            LastWriteTime = lastWriteTime;
            LastWriteTimeUtc = lastWriteTime.ToUniversalTime();
            return this;
        }

        /// <inheritdoc/>
        public IDirectory SetLastWriteTimeUtc(DateTime lastWriteTimeUtc)
        {
            FakeFileSystemTree.ThrowIfNotFound(this);
            LastWriteTimeUtc = lastWriteTimeUtc;
            LastWriteTime = lastWriteTimeUtc.ToLocalTime();
            return this;
        }

        /// <inheritdoc/>
        public IDirectory SetUnixFileMode(System.IO.UnixFileMode unixFileMode)
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
        public IDirectory SetLastWriteNow()
        {
            return SetLastWriteTimeUtc(_tree.GetUtcNow());
        }

        private static IEnumerable<FakeFile> GetFiles(FakeDirectory current, Regex expression)
        {
            foreach (var file in current.Content.Files)
            {
                if (file.Value.Exists)
                {
                    // Is this a match? In that case, add it to the result.
                    if (expression.IsMatch(file.Key.GetFilename().FullPath))
                    {
                        yield return current.Content.Files[file.Key];
                    }
                }
            }
        }

        private static Regex CreateRegex(string pattern)
        {
            pattern = pattern.Replace(".", "\\.").Replace("*", ".*").Replace("?", ".{1}");
            return new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled);
        }
    }
}