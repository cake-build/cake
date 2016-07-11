// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a file path.
    /// </summary>
    public sealed class FilePath : Path
    {
        /// <summary>
        /// Gets a value indicating whether this path has a file extension.
        /// </summary>
        /// <value>
        /// <c>true</c> if this file path has a file extension; otherwise, <c>false</c>.
        /// </value>
        public bool HasExtension
        {
            get { return System.IO.Path.HasExtension(FullPath); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePath"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public FilePath(string path)
            : base(path)
        {
        }

        /// <summary>
        /// Gets the directory part of the path.
        /// </summary>
        /// <returns>The directory part of the path.</returns>
        public DirectoryPath GetDirectory()
        {
            var directory = System.IO.Path.GetDirectoryName(FullPath);
            if (string.IsNullOrWhiteSpace(directory))
            {
                directory = "./";
            }
            return new DirectoryPath(directory);
        }

        /// <summary>
        /// Gets the filename.
        /// </summary>
        /// <returns>The filename.</returns>
        public FilePath GetFilename()
        {
            var filename = System.IO.Path.GetFileName(FullPath);
            return new FilePath(filename);
        }

        /// <summary>
        /// Gets the filename without it's extension.
        /// </summary>
        /// <returns>The filename without it's extension.</returns>
        public FilePath GetFilenameWithoutExtension()
        {
            var filename = System.IO.Path.GetFileNameWithoutExtension(FullPath);
            return new FilePath(filename);
        }

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        /// <returns>The file extension.</returns>
        public string GetExtension()
        {
            var extension = System.IO.Path.GetExtension(FullPath);
            return string.IsNullOrWhiteSpace(extension) ? null : extension;
        }

        /// <summary>
        /// Changes the file extension of the path.
        /// </summary>
        /// <param name="extension">The new extension.</param>
        /// <returns>A new <see cref="FilePath"/> with a new extension.</returns>
        public FilePath ChangeExtension(string extension)
        {
            return new FilePath(System.IO.Path.ChangeExtension(FullPath, extension));
        }

        /// <summary>
        /// Appends a file extension to the path.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>A new <see cref="FilePath"/> with an appended extension.</returns>
        public FilePath AppendExtension(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }
            if (!extension.StartsWith(".", StringComparison.OrdinalIgnoreCase))
            {
                extension = string.Concat(".", extension);
            }
            return new FilePath(string.Concat(FullPath, extension));
        }

        /// <summary>
        /// Makes the path absolute (if relative) using the current working directory.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>An absolute path.</returns>
        public FilePath MakeAbsolute(ICakeEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            return IsRelative
                ? environment.WorkingDirectory.CombineWithFilePath(this).Collapse()
                : new FilePath(FullPath);
        }

        /// <summary>
        /// Makes the path absolute (if relative) using the specified directory path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>An absolute path.</returns>
        public FilePath MakeAbsolute(DirectoryPath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (path.IsRelative)
            {
                throw new InvalidOperationException("Cannot make a file path absolute with a relative directory path.");
            }

            return IsRelative
                ? path.CombineWithFilePath(this).Collapse()
                : new FilePath(FullPath);
        }

        /// <summary>
        /// Collapses a <see cref="FilePath"/> containing ellipses.
        /// </summary>
        /// <returns>A collapsed <see cref="FilePath"/>.</returns>
        public FilePath Collapse()
        {
            return new FilePath(PathCollapser.Collapse(this));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="FilePath"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="FilePath"/>.</returns>
        public static implicit operator FilePath(string path)
        {
            return FromString(path);
        }

        /// <summary>
        /// Performs a conversion from <see cref="System.String"/> to <see cref="FilePath"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="FilePath"/>.</returns>
        public static FilePath FromString(string path)
        {
            return new FilePath(path);
        }

        /// <summary>
        /// Get the relative path to another directory.
        /// </summary>
        /// <param name="to">The target directory path.</param>
        /// <returns>A <see cref="DirectoryPath"/>.</returns>
        public DirectoryPath GetRelativePath(DirectoryPath to)
        {
            return GetDirectory().GetRelativePath(to);
        }

        /// <summary>
        /// Get the relative path to another file.
        /// </summary>
        /// <param name="to">The target file path.</param>
        /// <returns>A <see cref="FilePath"/>.</returns>
        public FilePath GetRelativePath(FilePath to)
        {
            return GetDirectory().GetRelativePath(to);
        }
    }
}
