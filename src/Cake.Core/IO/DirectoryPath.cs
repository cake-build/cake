using System;
using System.Linq;

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a directory path.
    /// </summary>
    public sealed class DirectoryPath : Path
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
                throw new ArgumentNullException("path");
            }
            var combinedPath = System.IO.Path.Combine(FullPath, path.GetFilename().FullPath);
            return new FilePath(combinedPath);
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
                throw new ArgumentNullException("path");
            }
            if (!path.IsRelative)
            {
                throw new InvalidOperationException("Cannot combine a directory path with an absolute file path.");
            }
            var combinedPath = System.IO.Path.Combine(FullPath, path.FullPath);
            return new FilePath(combinedPath);
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
                throw new ArgumentNullException("path");
            }
            if (!path.IsRelative)
            {
                throw new InvalidOperationException("Cannot combine a directory path with an absolute directory path.");
            }
            var combinedPath = System.IO.Path.Combine(FullPath, path.FullPath);
            return new DirectoryPath(combinedPath);
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
                throw new ArgumentNullException("path");
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
                throw new ArgumentNullException("environment");
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
            return FromString(path);
        }

        /// <summary>
        /// Operator that combines A <see cref="DirectoryPath"/> instance 
        /// with a <see cref="DirectoryPath"/> instance.
        /// </summary>
        /// <param name="left">The left directory path operand.</param>
        /// <param name="right">The right directory path operand.</param>
        /// <returns>
        /// A new directory path representing a combination of the two provided paths.
        /// </returns>
        public static DirectoryPath operator +(DirectoryPath left, DirectoryPath right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            return left.Combine(right);
        }

        /// <summary>
        /// Operator that combines A <see cref="DirectoryPath"/> instance 
        /// with a <see cref="FilePath"/> instance.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="file">The file.</param>
        /// <returns>
        /// A new file path representing a combination of the two provided paths.
        /// </returns>
        public static FilePath operator +(DirectoryPath directory, FilePath file)
        {
            if (directory == null)
            {
                throw new ArgumentNullException("directory");
            }
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            return directory.CombineWithFilePath(file);
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
    }
}