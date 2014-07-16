namespace Cake.Core.IO
{
    /// <summary>
    /// Contains extensions for <see cref="IFileSystem"/>.
    /// </summary>
    public static class FileSystemExtensions
    {
        /// <summary>
        /// Determines if a specified <see cref="FilePath"/> exist.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path.</param>
        /// <returns>Whether or not the specified file exist.</returns>
        public static bool Exist(this IFileSystem fileSystem, FilePath path)
        {
            var file = fileSystem.GetFile(path);
            return file != null && file.Exists;
        }

        /// <summary>
        /// Determines if a specified <see cref="DirectoryPath"/> exist.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path.</param>
        /// <returns>Whether or not the specified directory exist.</returns>
        public static bool Exist(this IFileSystem fileSystem, DirectoryPath path)
        {
            var directory = fileSystem.GetDirectory(path);
            return directory != null && directory.Exists;
        }
    }
}
