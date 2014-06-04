using System.IO;

namespace Cake.Core.IO
{
    public interface IFile
    {
        FilePath Path { get; }
        bool Exists { get; }
        long Length { get; }

        void Copy(FilePath destination);

        Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare);
    }
}