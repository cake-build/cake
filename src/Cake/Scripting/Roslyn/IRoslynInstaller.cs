using Cake.Core.IO;

namespace Cake.Scripting.Roslyn
{
    public interface IRoslynInstaller
    {
        bool IsInstalled(DirectoryPath root);
        void Install(DirectoryPath root);
    }
}
