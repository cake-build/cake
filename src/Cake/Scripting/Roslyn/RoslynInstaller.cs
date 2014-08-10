using System;
using Cake.Core.Diagnostics;
using NuGet;

namespace Cake.Scripting.Roslyn
{
    using Core.IO;

    internal sealed class RoslynInstaller : IRoslynInstaller
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeLog _log;

        public readonly FilePath[] _nugetPaths =
        {
            @"Roslyn.Compilers.CSharp.1.2.20906.2\lib\net45\Roslyn.Compilers.CSharp.dll",
            @"Roslyn.Compilers.Common.1.2.20906.2\lib\net45\Roslyn.Compilers.dll"
        };

        public readonly FilePath[] _roslynAssemblies =
        {
            @"Roslyn.Compilers.CSharp.dll",
            @"Roslyn.Compilers.dll"
        };

        public RoslynInstaller(IFileSystem fileSystem, ICakeLog log)
        {
            _fileSystem = fileSystem;
            _log = log;
        }

        public bool IsInstalled(DirectoryPath root)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }

            foreach (var roslynAssembly in _roslynAssemblies)
            {
                var file = _fileSystem.GetFile(root.CombineWithFilePath(roslynAssembly));
                if (!file.Exists)
                {
                    return false;
                }
            }
            return true;
        }

        public void Install(DirectoryPath root)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }

            var installRoot = root.Combine(Guid.NewGuid().ToString().Replace("-", ""));

            // Install package.
            _log.Verbose("Installing package...");
            var repo = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
            var packageManager = new PackageManager(repo, installRoot.FullPath);
            packageManager.InstallPackage("Roslyn.Compilers.CSharp", new SemanticVersion(new Version(1, 2, 20906, 2)));

            // Copy files
            _log.Verbose("Copying files...");
            foreach (var nugetPath in _nugetPaths)
            {
                var source = _fileSystem.GetFile(installRoot.CombineWithFilePath(nugetPath));
                var destination = _fileSystem.GetFile(root.CombineWithFilePath(nugetPath.GetFilename()));

                _log.Information("Copying {0}...", source.Path.GetFilename());

                if (!destination.Exists)
                {
                    source.Copy(destination.Path, true);
                }
            }

            // Delete the install directory.
            _log.Verbose("Deleting installation directory...");
            _fileSystem.GetDirectory(installRoot).Delete(true);
        }
    }
}