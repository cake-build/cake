using Cake.Core.IO;

namespace Cake.Bootstrapping
{
    public interface INuGetInstaller
    {
        void Install(DirectoryPath root);
    }
}
