using System.IO;

namespace Cake.Core.IO
{
    public static class FileExtensions
    {
        public static Stream Open(this IFile file, FileMode mode)
        {
            return file.Open(mode, 
                mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, 
                FileShare.None);
        }

        public static Stream Open(this IFile file, FileMode mode, FileAccess access)
        {
            return file.Open(mode, access, FileShare.None);
        }

        public static Stream OpenRead(this IFile file)
        {
            return file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public static Stream OpenWrite(this IFile file)
        {
            return file.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        }
    }
}
