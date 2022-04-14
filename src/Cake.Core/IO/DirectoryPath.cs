// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Cake.Core.Polyfill;

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a directory path.
    /// </summary>
    [TypeConverter(typeof(DirectoryPathConverter))]
    public sealed class DirectoryPath : Path, IEquatable<DirectoryPath>, IComparer<DirectoryPath>, IPath<DirectoryPath>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryPath"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public DirectoryPath(string path)
            : base(path)
        {
        }

        /// <summary>
        /// Gets the name of the directory.
        /// </summary>
        /// <returns>The directory name.</returns>
        /// <remarks>
        ///    If this is passed a file path, it will return the file name.
        ///    This is by-and-large equivalent to how DirectoryInfo handles this scenario.
        ///    If we wanted to return the *actual* directory name, we'd need to pull in IFileSystem,
        ///    and do various checks to make sure things exists.
        /// </remarks>
        public string GetDirectoryName()
        {
            return Segments.Last();
        }

        /// <summary>
        /// Combines the current path with the file name of a <see cref="FilePath"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A combination of the current path and the file name of the provided <see cref="FilePath"/>.</returns>
        public FilePath GetFilePath(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return new FilePath(PathHelper.Combine(FullPath, path.GetFilename().FullPath));
        }

        /// <summary>
        /// Gets the directory path of a <see cref="DirectoryPath"/>.
        /// </summary>
        /// <returns>A <see cref="DirectoryPath"/> to the parent directory of the given <see cref="DirectoryPath"/>.</returns>
        public DirectoryPath GetParent()
        {
            var collapsed = this.Collapse();

            if (collapsed.Segments.Length == 0)
            {
                return null;
            }

            if (collapsed.IsUNC && collapsed.Segments.Length < 4)
            {
                // UNC is special: \\server\share makes 3 (!) Segments
                // Also, \\server\share simply has no parent
                return null;
            }

            if (collapsed.Segments.Length == 1)
            {
                if (collapsed.IsRelative)
                {
                    // something like "relativeFolder/", whose parent is simply "."
                    return new DirectoryPath(".");
                }

                // one segment on Windows is e.g. "C:/"
                // on all other systems one segment is e.g "/home"
                if (EnvironmentHelper.GetPlatformFamily() == PlatformFamily.Windows)
                {
                    // no more parents
                    return null;
                }

                // root ("/") is not really a segment for Cake,
                // so we return that directly.
                return new DirectoryPath("/");
            }

            var segments = collapsed.Segments.Take(collapsed.Segments.Length - 1);
            var fullPath = collapsed.IsUNC
                ? @"\\" + string.Join(Separator.ToString(), segments.Skip(1))
                : string.Join(Separator.ToString(), segments);

            return new DirectoryPath(fullPath);
        }

        /// <summary>
        /// Combines the current path with a <see cref="FilePath"/>.
        /// The provided <see cref="FilePath"/> must be relative.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A combination of the current path and the provided <see cref="FilePath"/>.</returns>
        public FilePath CombineWithFilePath(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (!path.IsRelative)
            {
                throw new InvalidOperationException("Cannot combine a directory path with an absolute file path.");
            }

            return new FilePath(PathHelper.Combine(FullPath, path.FullPath));
        }

        /// <summary>
        /// Combines the current path with another <see cref="DirectoryPath"/>.
        /// The provided <see cref="DirectoryPath"/> must be relative.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A combination of the current path and the provided <see cref="DirectoryPath"/>.</returns>
        public DirectoryPath Combine(DirectoryPath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (!path.IsRelative)
            {
                throw new InvalidOperationException("Cannot combine a directory path with an absolute directory path.");
            }

            return new DirectoryPath(PathHelper.Combine(FullPath, path.FullPath));
        }

        /// <summary>
        /// Makes the path absolute to another (absolute) path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>An absolute path.</returns>
        public DirectoryPath MakeAbsolute(DirectoryPath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (path.IsRelative)
            {
                throw new CakeException("The provided path cannot be relative.");
            }
            return IsRelative
                ? path.Combine(this).Collapse()
                : new DirectoryPath(FullPath);
        }

        /// <summary>
        /// Makes the path absolute (if relative) using the current working directory.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>An absolute path.</returns>
        public DirectoryPath MakeAbsolute(ICakeEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            return IsRelative
                ? environment.WorkingDirectory.Combine(this).Collapse()
                : new DirectoryPath(FullPath);
        }

        /// <summary>
        /// Collapses a <see cref="DirectoryPath"/> containing ellipses.
        /// </summary>
        /// <returns>A collapsed <see cref="DirectoryPath"/>.</returns>
        public DirectoryPath Collapse()
        {
            return new DirectoryPath(PathCollapser.Collapse(this));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="DirectoryPath"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="DirectoryPath"/>.</returns>
        public static implicit operator DirectoryPath(string path)
        {
            return path is null ? null : FromString(path);
        }

        /// <summary>
        /// Performs a conversion from <see cref="System.String"/> to <see cref="DirectoryPath"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="DirectoryPath"/>.</returns>
        public static DirectoryPath FromString(string path)
        {
            return new DirectoryPath(path);
        }

        /// <summary>
        /// Get the relative path to another directory.
        /// </summary>
        /// <param name="to">The target directory path.</param>
        /// <returns>A <see cref="DirectoryPath"/>.</returns>
        public DirectoryPath GetRelativePath(DirectoryPath to)
        {
            return RelativePathResolver.Resolve(this, to);
        }

        /// <summary>
        /// Get the relative path to another file.
        /// </summary>
        /// <param name="to">The target file path.</param>
        /// <returns>A <see cref="FilePath"/>.</returns>
        public FilePath GetRelativePath(FilePath to)
        {
            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            return GetRelativePath(to.GetDirectory()).GetFilePath(to.GetFilename());
        }

        /// <summary>
        /// Determines whether two <see cref="DirectoryPath"/> instances are equal.
        /// </summary>
        /// <param name="other">the <see cref="DirectoryPath"/> to compare.</param>
        /// <returns>True if other is equal to current object, False otherwise.</returns>
        public bool Equals(DirectoryPath other)
            => PathComparer.Default.Equals(this, other);

        /// <summary>
        /// Determines whether two <see cref="DirectoryPath"/> instances are equal.
        /// </summary>
        /// <param name="other">the <see cref="DirectoryPath"/> to compare.</param>
        /// <returns>True if other is equal to current object, False otherwise.</returns>
        public override bool Equals(object other)
            => Equals(other as DirectoryPath);

        /// <summary>
        /// Determines whether two <see cref="DirectoryPath"/> instances are equal.
        /// </summary>
        /// <param name="directoryPath">left side <see cref="DirectoryPath"/>.</param>
        /// <param name="otherDirectoryPath">right side <see cref="DirectoryPath"/>.</param>
        /// <returns>True if other is equal to current object, False otherwise.</returns>
        public static bool operator ==(DirectoryPath directoryPath, DirectoryPath otherDirectoryPath)
            => ReferenceEquals(directoryPath, otherDirectoryPath)
                || directoryPath?.Equals(otherDirectoryPath) == true;

        /// <summary>
        /// Determines whether two <see cref="DirectoryPath"/> instances are different.
        /// </summary>
        /// <param name="directoryPath">left side <see cref="DirectoryPath"/>.</param>
        /// <param name="otherDirectoryPath">right side <see cref="DirectoryPath"/>.</param>
        /// <returns>True if other is equal to current object, False otherwise.</returns>
        public static bool operator !=(DirectoryPath directoryPath, DirectoryPath otherDirectoryPath) => !(directoryPath == otherDirectoryPath);

        /// <summary>
        /// Returns the hash code for the <see cref="FilePath"/>.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode()
            => PathComparer.Default.GetHashCode(this);

        /// <summary>
        /// Compares two DirectoryPath and returns an indication of their relative sort order.
        /// </summary>
        /// <param name="x">The first <see cref="Path"/> to compare.</param>
        /// <param name="y">The second <see cref="Path"/> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of x and y.
        /// <list type="table">
        /// <item>
        /// <term>Less than zero</term>
        /// <description>x precedes y in the sort order, or x is null and y is not null.</description>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <description>x is equal to y, or x and y are both null.</description>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <description>
        /// x follows y in the sort order, or y is null and x is not null.
        /// </description>
        /// </item>
        /// </list>
        /// </returns>
        public int Compare([AllowNull] DirectoryPath x, [AllowNull] DirectoryPath y)
            => PathComparer.Default.Compare(x, y);
    }
}