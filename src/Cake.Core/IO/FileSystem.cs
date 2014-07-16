namespace Cake.Core.IO
{
    /// <summary>
    /// A physical file system implementation.
    /// </summary>
    public sealed class FileSystem : IFileSystem
    {
        /// <summary>
        /// Gets a <see cref="IFile" /> instance representing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="IFile" /> instance representing the specified path.</returns>
        public IFile GetFile(FilePath path)
        {
            return new File(path);
        }

        /// <summary>
        /// Gets a <see cref="IDirectory" /> instance representing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="IDirectory" /> instance representing the specified path.</returns>
        public IDirectory GetDirectory(DirectoryPath path)
        {
            return new Directory(path);
        }
    }
}
