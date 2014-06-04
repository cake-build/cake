using Cake.Core.IO;

namespace Cake.Bootstrapping
{
    public interface ICakeBootstrapper
    {
        void Bootstrap(DirectoryPath root);
    }
}
