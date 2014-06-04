using System;

namespace Cake.Core.IO
{
    public sealed class DirectoryPath : Path
    {
        public DirectoryPath(string path)
            : base(path)
        {
        }

        public FilePath GetFilePath(FilePath path)
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

        public static implicit operator DirectoryPath(string path)
        {
            return FromString(path);
        }

        public static DirectoryPath FromString(string path)
        {
            return new DirectoryPath(path);
        }
    }
}