using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;

namespace Cake.Testing.Fakes
{
    /// <summary>
    /// Implementation of a fake <see cref="IDirectory"/>.
    /// </summary>
    public sealed class FakeDirectory : IDirectory
    {
        private readonly FakeFileSystem _fileSystem;
        private readonly DirectoryPath _path;
        private readonly bool _creatable;
        private readonly bool _hidden;
        private bool _exist;        

        /// <summary>
        /// Gets the path to the directory.
        /// </summary>
        /// <value>The path.</value>
        public DirectoryPath Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets the path to the directory.
        /// </summary>
        /// <value>The path.</value>
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
        public bool Exists
        {
            get { return _exist; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IFileSystemInfo" /> is hidden.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the entry is hidden; otherwise, <c>false</c>.
        /// </value>
        public bool Hidden
        {
            get { return _exist && _hidden; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeDirectory"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path.</param>
        /// <param name="creatable">if set to <c>true</c> the directory is creatable.</param>
        /// <param name="hidden">if set to <c>true</c> the directory is hidden.</param>
        public FakeDirectory(FakeFileSystem fileSystem, DirectoryPath path, bool creatable, bool hidden)
        {
            _fileSystem = fileSystem;
            _path = path;
            _exist = false;
            _creatable = creatable;
            _hidden = hidden;
        }

        /// <summary>
        /// Creates the directory.
        /// </summary>
        public void Create()
        {
            if (_creatable)
            {
                _exist = true;
            }
        }

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="recursive">Will perform a recursive delete if set to <c>true</c>.</param>
        public void Delete(bool recursive)
        {
            if (recursive)
            {
                foreach (var directory in GetDirectories("*", SearchScope.Current))
                {
                    directory.Delete(true);
                }
                foreach (var file in GetFiles("*", SearchScope.Current))
                {
                    file.Delete();
                }
            }
            _exist = false;
        }

        /// <summary>
        /// Gets directories matching the specified filter and scope.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns>
        /// Directories matching the filter and scope.
        /// </returns>
        public IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope)
        {
            var result = new List<IDirectory>();
            var children = _fileSystem.Directories.Where(x => x.Key.FullPath.StartsWith(_path.FullPath + "/", StringComparison.OrdinalIgnoreCase));
            foreach (var child in children.Where(c => c.Value.Exists))
            {
                var relative = child.Key.FullPath.Substring(_path.FullPath.Length + 1);
                if (!relative.Contains("/"))
                {
                    result.Add(child.Value);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets files matching the specified filter and scope.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns>
        /// Files matching the specified filter and scope.
        /// </returns>
        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope)
        {
            var result = new List<IFile>();
            var children = _fileSystem.Files.Where(x => x.Key.FullPath.StartsWith(_path.FullPath + "/", StringComparison.OrdinalIgnoreCase));
            foreach (var child in children.Where(c => c.Value.Exists))
            {
                var relative = child.Key.FullPath.Substring(_path.FullPath.Length + 1);
                if (!relative.Contains("/"))
                {
                    result.Add(child.Value);
                }
            }
            return result;
        }
    }
}