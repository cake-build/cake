using System.IO;
using FileInfo = Pri.LongPath.FileInfo;

namespace Cake.Core.IO.LongPath
{
    internal sealed class File : IFile
    {
        private readonly FileInfo _file;
        private readonly FilePath _path;

        Path IFileSystemInfo.Path
        {
            get { return Path; }
        }
        public FilePath Path { get { return _path; } }
        public bool Exists { get { return _file.Exists; } }
        
        public bool Hidden
        {
            get { return (_file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden; }
        }

        public long Length { get { return _file.Length; } }
        public void Copy(FilePath destination, bool overwrite)
        {
            _file.CopyTo(destination.FullPath, overwrite);
        }

        public void Move(FilePath destination)
        {
            _file.MoveTo(destination.FullPath);
        }

        public void Delete()
        {
            _file.Delete();
        }

        public Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            return _file.Open(fileMode, fileAccess, fileShare);
        }

        internal File(FilePath path)
        {
            _path = path;
            _file = new FileInfo(path.FullPath);
        }
    }
}