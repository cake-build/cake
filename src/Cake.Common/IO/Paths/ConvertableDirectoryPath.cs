// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core.IO;

namespace Cake.Common.IO.Paths
{
    /// <summary>
    /// Represents a <see cref="DirectoryPath" /> that can be easily converted.
    /// <example>
    /// To get the underlying <see cref="DirectoryPath"/>:
    /// <code>
    /// ConvertableDirectoryPath convertable = Directory("./root");
    /// DirectoryPath path = (DirectoryPath)convertable;
    /// </code>
    /// To combine two directories:
    /// <code>
    /// ConvertableDirectoryPath path = Directory("./root") + Directory("other");
    /// </code>
    /// To combine a directory with a file:
    /// <code>
    /// ConvertableFilePath path = Directory("./root") + File("other.txt");
    /// </code>
    /// </example>
    /// </summary>
    public sealed class ConvertableDirectoryPath
    {
        private readonly DirectoryPath _path;

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public DirectoryPath Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertableDirectoryPath"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        internal ConvertableDirectoryPath(DirectoryPath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            _path = path;
        }

        /// <summary>
        /// Operator that combines A <see cref="ConvertableDirectoryPath"/> instance
        /// with another <see cref="ConvertableDirectoryPath"/> instance.
        /// </summary>
        /// <param name="left">The left directory path operand.</param>
        /// <param name="right">The right directory path operand.</param>
        /// <returns>A new directory path representing a combination of the two provided paths.</returns>
        public static ConvertableDirectoryPath operator +(ConvertableDirectoryPath left, ConvertableDirectoryPath right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            return new ConvertableDirectoryPath(left.Path.Combine(right.Path));
        }

        /// <summary>
        /// Operator that combines A <see cref="ConvertableDirectoryPath"/> instance
        /// with a <see cref="DirectoryPath"/> instance.
        /// </summary>
        /// <param name="left">The left directory path operand.</param>
        /// <param name="right">The right directory path operand.</param>
        /// <returns>A new directory path representing a combination of the two provided paths.</returns>
        public static ConvertableDirectoryPath operator +(ConvertableDirectoryPath left, DirectoryPath right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            return new ConvertableDirectoryPath(left.Path.Combine(right));
        }

        /// <summary>
        /// Operator that combines A <see cref="ConvertableDirectoryPath"/> instance
        /// with a <see cref="ConvertableFilePath"/> instance.
        /// </summary>
        /// <param name="directory">The left directory path operand.</param>
        /// <param name="file">The right file path operand.</param>
        /// <returns>A new file path representing a combination of the two provided paths.</returns>
        public static ConvertableFilePath operator +(ConvertableDirectoryPath directory, ConvertableFilePath file)
        {
            if (directory == null)
            {
                throw new ArgumentNullException("directory");
            }
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            return new ConvertableFilePath(directory.Path.CombineWithFilePath(file.Path));
        }

        /// <summary>
        /// Operator that combines A <see cref="ConvertableDirectoryPath"/> instance
        /// with a <see cref="FilePath"/> instance.
        /// </summary>
        /// <param name="directory">The left directory path operand.</param>
        /// <param name="file">The right file path operand.</param>
        /// <returns>A new file path representing a combination of the two provided paths.</returns>
        public static ConvertableFilePath operator +(ConvertableDirectoryPath directory, FilePath file)
        {
            if (directory == null)
            {
                throw new ArgumentNullException("directory");
            }
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            return new ConvertableFilePath(directory.Path.CombineWithFilePath(file));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ConvertableDirectoryPath"/> to <see cref="DirectoryPath"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator DirectoryPath(ConvertableDirectoryPath path)
        {
            if (path == null)
            {
                return null;
            }
            return path.Path;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ConvertableDirectoryPath"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(ConvertableDirectoryPath path)
        {
            if (path == null)
            {
                return null;
            }
            return path.Path.FullPath;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _path.FullPath;
        }
    }
}
