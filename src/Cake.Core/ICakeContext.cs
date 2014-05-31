using Cake.Core.IO;

namespace Cake.Core
{
    public interface ICakeContext
    {
        IFileSystem FileSystem { get; }
        ICakeEnvironment Environment { get; }
    }
}