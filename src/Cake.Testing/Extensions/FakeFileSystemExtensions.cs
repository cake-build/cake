// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.IO;
using Cake.Core.IO;

// ReSharper disable once CheckNamespace
namespace Cake.Testing
{
    /// <summary>
    /// Contains extensions for <see cref="FakeFileSystem"/>.
    /// </summary>
    public static class FakeFileSystemExtensions
    {
        /// <summary>
        /// Ensures that the specified file do not exist.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path.</param>
        public static void EnsureFileDoNotExist(this FakeFileSystem fileSystem, FilePath path)
        {
            var file = fileSystem.GetFile(path);
            if (file != null && file.Exists)
            {
                file.Delete();
            }
        }

        /// <summary>
        /// Creates a file at the specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path.</param>
        /// <returns>The same <see cref="FakeFile"/> instance so that multiple calls can be chained.</returns>
        public static FakeFile CreateFile(this FakeFileSystem fileSystem, FilePath path)
        {
            CreateDirectory(fileSystem, path.GetDirectory());

            var file = fileSystem.GetFile(path);
            if (!file.Exists)
            {
                file.OpenWrite().Close();
            }

            return file;
        }

        /// <summary>
        /// Creates a file at the specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path.</param>
        /// <param name="contentsBytes">The file contents.</param>
        /// <returns>The same <see cref="FakeFile"/> instance so that multiple calls can be chained.</returns>
        public static FakeFile CreateFile(this FakeFileSystem fileSystem, FilePath path, byte[] contentsBytes)
        {
            CreateDirectory(fileSystem, path.GetDirectory());

            var file = fileSystem.GetFile(path);
            if (!file.Exists)
            {
                using (var stream = file.OpenWrite())
                {
                    using (var ms = new MemoryStream(contentsBytes))
                    {
                        ms.CopyTo(stream);
                    }
                }
            }

            return file;
        }

        /// <summary>
        /// Creates a directory at the specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path.</param>
        /// <returns>The same <see cref="FakeDirectory"/> instance so that multiple calls can be chained.</returns>
        public static FakeDirectory CreateDirectory(this FakeFileSystem fileSystem, DirectoryPath path)
        {
            var directory = fileSystem.GetDirectory(path);
            if (!directory.Exists)
            {
                directory.Create();
            }
            return directory;
        }
    }
}
