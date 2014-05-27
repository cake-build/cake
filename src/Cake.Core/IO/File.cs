using System.IO;

namespace Cake.Core.IO
{
    internal sealed class File : IFile
    {
        private readonly FileInfo _file;
        private readonly FilePath _path;

        public FilePath Path
        {
            get { return _path; }
        }

        public bool Exists
        {
            get { return _file.Exists; }
        }

        public long Length
        {
            get { return _file.Length; }
        }

        public File(FilePath path)
        {
            _path = path;
            _file = new FileInfo(path.FullPath);
        }

        public Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            return _file.Open(fileMode, fileAccess, fileShare);
        }        
    }
}