using Cake.Core.Diagnostics;
using Cake.Core.IO;

// TODO: This is one big hack. Should refactor and add better tests.

namespace Cake.Bootstrapping
{
    internal sealed class CakeBootstrapper : ICakeBootstrapper
    {
        private readonly IFileSystem _fileSystem;
        private readonly INuGetInstaller _installer;
        private readonly ICakeLog _log;

        public readonly FilePath[] _roslynAssemblies =
        {
            @"Roslyn.Compilers.CSharp.dll",
            @"Roslyn.Compilers.dll"
        };

        public CakeBootstrapper(IFileSystem fileSystem, ICakeLog log, INuGetInstaller installer = null)
        {
            _fileSystem = fileSystem;            
            _log = log;
            _installer = installer ?? new NuGetInstaller(_fileSystem, _log);
        }

        public void Bootstrap(DirectoryPath root)
        {
            if (!IsInstalled(root))
            {
                _log.Information("Downloading and installing Roslyn...");
                _installer.Install(root);
            }
        }

        private bool IsInstalled(DirectoryPath root)
        {
            foreach (var roslynAssembly in _roslynAssemblies)
            {
                var file = _fileSystem.GetFile(root.GetFilePath(roslynAssembly));
                if (!file.Exists)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
