using System;
using System.Collections.Generic;
using System.Reflection;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;
using NuGet;

namespace Cake.Scripting.Mono
{
    using Core.IO;

    internal sealed class MonoScriptEngine : IScriptEngine
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;

        public MonoScriptEngine(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;
        }

        public IScriptSession CreateSession(IScriptHost host, IDictionary<string, string> arguments)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            // Is Mono.CSharp installed?
            var root = _environment.GetApplicationRoot();
            if (!IsInstalled(root))
            {
                _log.Information("Downloading and installing Mono compiler...");
                Install(root);
            }

            // Load Mono.CSharp assembly dynamically.
            _log.Debug("Loading Mono.CSharp.dll...");
            Assembly.LoadFrom(_environment
                .GetApplicationRoot()
                .CombineWithFilePath("Mono.CSharp.dll")
                .FullPath);

            // Create the script session.
            _log.Debug("Creating script session...");
            return new MonoScriptSession(host, _log);
        }

        private bool IsInstalled(DirectoryPath root)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            var path = root.CombineWithFilePath("Mono.CSharp.dll");
            return _fileSystem.GetFile(path).Exists;
        }

        private void Install(DirectoryPath root)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }

            var installRoot = root.Combine(Guid.NewGuid().ToString().Replace("-", string.Empty));

            // Install package.
            _log.Verbose("Installing package...");
            var repository = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
            var packageManager = new PackageManager(repository, installRoot.FullPath);
            packageManager.InstallPackage("Mono.CSharp", new SemanticVersion(4, 0, 0, 143));

            // Get the install root.
            var path = new FilePath("Mono.CSharp.4.0.0.143/lib/4.5/Mono.CSharp.dll");

            // Copy files
            _log.Verbose("Copying files...");
            var source = _fileSystem.GetFile(installRoot.CombineWithFilePath(path));
            var destination = _fileSystem.GetFile(root.CombineWithFilePath(path.GetFilename()));

            if (!destination.Exists)
            {
                _log.Information("Copying {0}...", source.Path.GetFilename());
                source.Copy(destination.Path, true);
            }

            // Delete the install directory.
            _log.Verbose("Deleting installation directory...");
            _fileSystem.GetDirectory(installRoot).Delete(true);
        }
    }
}
