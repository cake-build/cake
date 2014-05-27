namespace Cake.Core.IO
{
    internal sealed class FileSystem : IFileSystem
    {
        private readonly bool _isUnix;

        public bool IsUnix
        {
            get { return _isUnix; }
        }

        public DirectoryPath WorkingDirectory { get; set; }

        public FileSystem()
        {
            _isUnix = Machine.IsUnix();
        }

        public IFile GetFile(FilePath path)
        {
            return new File(path);
        }

        public IDirectory GetDirectory(DirectoryPath path)
        {
            return new Directory(path);
        }
    }
}
