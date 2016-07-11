// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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
        private readonly DirectoryPath _path;
        private readonly FakeDirectoryContent _content;

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
        /// Gets the path to the directory.
        /// </summary>
        /// <value>The path.</value>
        public DirectoryPath Path
        {
            get { return _path; }
        }

        Path IFileSystemInfo.Path
        {
            get { return _path; }
        }

        internal FakeDirectory Parent { get; set; }

        internal FakeDirectoryContent Content
        {
            get { return _content; }
        }

        internal FakeDirectory(FakeFileSystemTree tree, DirectoryPath path)
        {
            _tree = tree;
            _path = path;
            _content = new FakeDirectoryContent(this, tree.Comparer);
        }

        /// <summary>
        /// Creates the directory.
        /// </summary>
        public void Create()
        {
            _tree.CreateDirectory(this);
        }

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="recursive">Will perform a recursive delete if set to <c>true</c>.</param>
        public void Delete(bool recursive)
        {
            _tree.DeleteDirectory(this, recursive);
        }

        /// <summary>
        /// Gets directories matching the specified filter and scope.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns>Directories matching the filter and scope.</returns>
        public IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope)
        {
            var result = new List<IDirectory>();
            var stack = new Stack<FakeDirectory>();
            foreach (var child in _content.Directories.Values)
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
                    result.Add(current);
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

            return result;
        }

        /// <summary>
        /// Gets files matching the specified filter and scope.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns>Files matching the specified filter and scope.</returns>
        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope)
        {
            var result = new List<IFile>();
            var stack = new Stack<FakeDirectory>();

            // Rewrite the filter to a regex expression.
            var expression = CreateRegex(filter);

            // Just interested in this directory?
            if (scope == SearchScope.Current)
            {
                return GetFiles(this, expression);
            }

            stack.Push(this);
            while (stack.Count > 0)
            {
                // Pop a directory from the stack.
                var current = stack.Pop();

                // Add all files we can find.
                result.AddRange(GetFiles(current, expression));

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
            return result;
        }

        private static IEnumerable<FakeFile> GetFiles(FakeDirectory current, Regex expression)
        {
            var result = new List<FakeFile>();
            foreach (var file in current.Content.Files)
            {
                if (file.Value.Exists)
                {
                    // Is this a match? In that case, add it to the result.
                    if (expression.IsMatch(file.Key.GetFilename().FullPath))
                    {
                        result.Add(current.Content.Files[file.Key]);
                    }
                }
            }
            return result;
        }

        private static Regex CreateRegex(string pattern)
        {
            pattern = pattern.Replace(".", "\\.").Replace("*", ".*").Replace("?", ".{1}");
            return new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled);
        }
    }
}
