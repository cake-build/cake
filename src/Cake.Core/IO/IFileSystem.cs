namespace Cake.Core.IO
{
    public interface IFileSystem
    {
        IFile GetFile(FilePath path);
        IDirectory GetDirectory(DirectoryPath path);
    }
}