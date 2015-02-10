using System;
using System.Collections.Generic;
using System.IO;
using Cake.Core.IO;

namespace Cake.Testing.Fakes
{
    /// <summary>
    /// Implementation of a fake <see cref="IFileSystem"/>.
    /// </summary>
    public sealed class FakeFileSystem : IFileSystem
    {
        private readonly Dictionary<DirectoryPath, FakeDirectory> _directories;
        private readonly Dictionary<FilePath, FakeFile> _files;

        /// <summary>
        /// Gets the directories.
        /// </summary>
        /// <value>The directories.</value>
        public Dictionary<DirectoryPath, FakeDirectory> Directories
        {
            get { return _directories; }
        }

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <value>The files.</value>
        public Dictionary<FilePath, FakeFile> Files
        {
            get { return _files; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeFileSystem"/> class.
        /// </summary>
        /// <param name="isCaseSensitive">If set to <c>true</c> the file system is case sensitive.</param>
        public FakeFileSystem(bool isCaseSensitive)
        {
            _directories = new Dictionary<DirectoryPath, FakeDirectory>(new PathComparer(isCaseSensitive));
            _files = new Dictionary<FilePath, FakeFile>(new PathComparer(isCaseSensitive));
        }

        /// <summary>
        /// Gets a <see cref="IFile" /> instance representing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A <see cref="IFile" /> instance representing the specified path.
        /// </returns>
        public IFile GetFile(FilePath path)
        {
            return GetFile(path, false);
        }

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="hidden">if set to <c>true</c> [hidden].</param>
        /// <returns>A <see cref="IFile"/> instance.</returns>
        public IFile GetFile(FilePath path, bool hidden)
        {
            if (!Files.ContainsKey(path))
            {
                Files.Add(path, new FakeFile(this, path) { Hidden = hidden });
            }
            return Files[path];
        }

        /// <summary>
        /// Gets the created file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="IFile"/> instance.</returns>
        public IFile GetCreatedFile(FilePath path)
        {
            return GetCreatedFile(path, false);
        }

        /// <summary>
        /// Gets the created file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="hidden">if set to <c>true</c> [hidden].</param>
        /// <returns>A <see cref="IFile"/> instance.</returns>
        public IFile GetCreatedFile(FilePath path, bool hidden)
        {
            var file = GetFile(path, hidden);
            file.Open(FileMode.Create, FileAccess.Write, FileShare.None).Close();
            return file;
        }

        /// <summary>
        /// Gets the created file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="content">The content.</param>
        /// <returns>A <see cref="IFile"/> instance.</returns>
        public IFile GetCreatedFile(FilePath path, string content)
        {
            var file = GetFile(path);
            var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None);
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Close();
            stream.Close();
            return file;
        }

        /// <summary>
        /// Gets a <see cref="IDirectory" /> instance representing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A <see cref="IDirectory" /> instance representing the specified path.
        /// </returns>
        public IDirectory GetDirectory(DirectoryPath path)
        {
            return GetDirectory(path, creatable: true, hidden: false);
        }

        /// <summary>
        /// Gets the created directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="IDirectory"/> instance.</returns>
        public IDirectory GetCreatedDirectory(DirectoryPath path)
        {
            return GetCreatedDirectory(path, false);
        }

        /// <summary>
        /// Gets the created directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="hidden">if set to <c>true</c> [hidden].</param>
        /// <returns>A <see cref="IDirectory"/> instance.</returns>
        public IDirectory GetCreatedDirectory(DirectoryPath path, bool hidden)
        {
            var directory = GetDirectory(path, creatable: true, hidden: hidden);
            directory.Create();
            return directory;
        }

        /// <summary>
        /// Gets the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="creatable">if set to <c>true</c> [creatable].</param>
        /// <param name="hidden">if set to <c>true</c> [hidden].</param>
        /// <returns>A <see cref="IDirectory"/> instance.</returns>
        private IDirectory GetDirectory(DirectoryPath path, bool creatable, bool hidden)
        {
            if (!Directories.ContainsKey(path))
            {
                Directories.Add(path, new FakeDirectory(this, path, creatable, hidden));
            }
            return Directories[path];
        }

        /// <summary>
        /// Gets the content of the text.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The text content of the file.</returns>
        public string GetTextContent(FilePath path)
        {
            var file = GetFile(path) as FakeFile;
            if (file == null)
            {
                throw new InvalidOperationException();
            }

            try
            {
                if (file.Deleted)
                {
                    Files[path].Exists = true;
                }

                using (var stream = file.OpenRead())
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            finally
            {
                if (file.Deleted)
                {
                    Files[path].Exists = false;
                }
            }
        }
    }
}