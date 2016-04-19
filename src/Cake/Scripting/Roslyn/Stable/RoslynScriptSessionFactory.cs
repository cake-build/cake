using System;
using System.Reflection;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;
using NuGet;

namespace Cake.Scripting.Roslyn.Stable
{
    using Core.IO;

    internal sealed class RoslynScriptSessionFactory
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeConfiguration _configuration;
        private readonly ICakeLog _log;
        private readonly FilePath[] _paths;

        public RoslynScriptSessionFactory(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            ICakeConfiguration configuration,
            ICakeLog log)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _configuration = configuration;
            _log = log;

            _paths = new FilePath[]
            {
                @"Roslyn.Compilers.CSharp.1.2.20906.2\lib\net45\Roslyn.Compilers.CSharp.dll",
                @"Roslyn.Compilers.Common.1.2.20906.2\lib\net45\Roslyn.Compilers.dll"
            };
        }

        public IScriptSession CreateSession(IScriptHost host)
        {
            var root = _environment.GetApplicationRoot();

            // Is Roslyn installed?
            if (!IsInstalled(root))
            {
                _log.Information("Downloading and installing Roslyn...");
                Install(root);
            }

            // Load Roslyn assemblies dynamically.
            foreach (var filePath in _paths)
            {
                Assembly.LoadFrom(_environment
                    .GetApplicationRoot()
                    .CombineWithFilePath(filePath.GetFilename())
                    .FullPath);
            }

            // Create a new session.
            return new RoslynScriptSession(host, _log);
        }

        private bool IsInstalled(DirectoryPath root)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            foreach (var path in _paths)
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

        private void Install(DirectoryPath root)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }

            var installRoot = root.Combine(Guid.NewGuid().ToString().Replace("-", string.Empty));

            // Install package.
            var nugetSource = _configuration.GetValue("Roslyn_NuGetSource") ?? "https://packages.nuget.org/api/v2";
            var repository = PackageRepositoryFactory.Default.CreateRepository(nugetSource);
            var packageManager = new PackageManager(repository, installRoot.FullPath);
            _log.Verbose("Installing packages (using {0})...", nugetSource);
            packageManager.InstallPackage("Roslyn.Compilers.CSharp", new SemanticVersion(new Version(1, 2, 20906, 2)), false, true);

            // Copy files
            _log.Verbose("Copying files...");
            foreach (var path in _paths)
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
    }
}