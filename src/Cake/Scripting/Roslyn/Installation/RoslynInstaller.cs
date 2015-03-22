using System;
using System.Linq;
using Cake.Core.Diagnostics;
using NuGet;

namespace Cake.Scripting.Roslyn.Installation
{
    using Core.IO;

    internal sealed class RoslynInstaller : IRoslynInstaller
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeLog _log;

        public RoslynInstaller(IFileSystem fileSystem, ICakeLog log)
        {
            _fileSystem = fileSystem;
            _log = log;
        }

        public bool IsInstalled(DirectoryPath root, RoslynInstallerInstructions instructions)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            if (instructions == null)
            {
                throw new ArgumentNullException("instructions");
            }
            foreach (var path in instructions.Paths)
            {
                var filename = path.GetFilename();
                var file = _fileSystem.GetFile(root.CombineWithFilePath(filename));
                if (!file.Exists)
                {
                    return false;
                }
            }
            return true;
        }

        public void Install(DirectoryPath root, RoslynInstallerInstructions instructions)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            if (instructions == null)
            {
                throw new ArgumentNullException("instructions");
            }

            var installRoot = root.Combine(Guid.NewGuid().ToString().Replace("-", string.Empty));

            // Install package.
            var packageManager = CreatePackageManager(installRoot, instructions);
            foreach (var package in instructions.Packages)
            {
                _log.Verbose("Installing {0} ({1})...", package.Key, package.Value);
                packageManager.InstallPackage(package.Key, package.Value, false, true);
            }

            // Copy files
            _log.Verbose("Copying files...");
            foreach (var path in instructions.Paths)
            {
                var source = _fileSystem.GetFile(installRoot.CombineWithFilePath(path));
                var destination = _fileSystem.GetFile(root.CombineWithFilePath(path.GetFilename()));

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

        private static IPackageManager CreatePackageManager(DirectoryPath path, RoslynInstallerInstructions instructions)
        {
            var repo = AggregateRepository.Create(PackageRepositoryFactory.Default, instructions.PackageSources.ToArray(), false);
            return new PackageManager(repo, path.FullPath);
        }
    }
}