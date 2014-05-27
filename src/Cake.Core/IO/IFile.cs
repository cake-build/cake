using System.IO;

namespace Cake.Core.IO
{
    public interface IFile
    {
        FilePath Path { get; }
        bool Exists { get; }
        long Length { get; }

        Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare);
    }
}