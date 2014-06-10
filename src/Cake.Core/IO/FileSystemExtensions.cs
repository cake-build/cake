namespace Cake.Core.IO
{
    public static class FileSystemExtensions
    {
        public static bool Exist(this IFileSystem fileSystem, FilePath path)
        {
            var file = fileSystem.GetFile(path);
            return file != null && file.Exists;
        }

        public static bool Exist(this IFileSystem fileSystem, DirectoryPath path)
        {
            var directory = fileSystem.GetDirectory(path);
            return directory != null && directory.Exists;
        }
    }
}
