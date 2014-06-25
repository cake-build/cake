using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Core
{
    public interface ICakeContext
    {
        IFileSystem FileSystem { get; }
        ICakeEnvironment Environment { get; }
        IGlobber Globber { get; }
        ICakeLog Log { get; }
        ICakeArguments Arguments { get; }
        IProcessRunner ProcessRunner { get; }
    }
}