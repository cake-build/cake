using System.Collections.Generic;

namespace Cake.Core.IO
{
    public interface IDirectory
    {
        DirectoryPath Path { get; }
        bool Exists { get; }
        void Create();
        void Delete(bool recursive);

        IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope);
        IEnumerable<IFile> GetFiles(string filter, SearchScope scope);
    }
}