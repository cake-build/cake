namespace Cake.Core.IO
{
    public interface IFileSystem
    {
        bool IsUnix { get; }
        DirectoryPath WorkingDirectory { get; set; }

        IFile GetFile(FilePath path);
        IDirectory GetDirectory(DirectoryPath path);
    }
}