using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;
using NuGet;

namespace Cake.Scripting.Roslyn.Nightly
{
    using Core.IO;

    internal sealed class RoslynNightlyScriptSessionFactory
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly ICakeLog _log;
        private readonly FilePath[] _paths;

        public RoslynNightlyScriptSessionFactory(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IGlobber globber,
            ICakeLog log)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _globber = globber;
            _log = log;

            _paths = new FilePath[]
            {
                @"net45/Microsoft.CodeAnalysis.dll",
                @"net45/Microsoft.CodeAnalysis.Scripting.CSharp.dll",
                @"net45/Microsoft.CodeAnalysis.Scripting.dll",
                @"net45/Microsoft.CodeAnalysis.Desktop.dll",
                @"net45/Microsoft.CodeAnalysis.CSharp.dll",
                @"net45/Microsoft.CodeAnalysis.CSharp.Desktop.dll",
                @"portable-net45+win8+wp8+wpa81/System.Collections.Immutable.dll",
                @"portable-net45+win8/System.Reflection.Metadata.dll",
            };
        }

        public IScriptSession CreateSession(IScriptHost host)
        {
            // Is Roslyn installed?
            if (!IsInstalled())
            {
                _log.Information("Downloading and installing Roslyn (experimental)...");
                Install(new SemanticVersion(1, 0, 0, "rc2"));
            }

            // Load Roslyn assemblies dynamically.
            foreach (var filePath in _paths)
            {
                Assembly.LoadFrom(_environment
                    .GetApplicationRoot()
                    .CombineWithFilePath(filePath.GetFilename())
                    .FullPath);
            }

            // Create the session.
            return new RoslynNightlyScriptSession(host, _log);
        }

        private bool IsInstalled()
        {
            var root = _environment.GetApplicationRoot();
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

        private void Install(SemanticVersion version)
        {
            var root = _environment.GetApplicationRoot().MakeAbsolute(_environment);
            var installRoot = root.Combine(Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            var packages = new Dictionary<string, SemanticVersion>
            {
                { "Microsoft.CodeAnalysis.Scripting", version },
                { "Microsoft.CodeAnalysis.CSharp", version }
            };

            // Install package.
            _log.Verbose("Installing packages...");
            var packageManager = CreatePackageManager(installRoot);
            foreach (var package in packages)
            {
                _log.Information("Downloading package {0} ({1})...", package.Key, package.Value);
                packageManager.InstallPackage(package.Key, package.Value, false, true);
            }

            // Copy files.
            _log.Verbose("Copying files...");
            foreach (var path in _paths)
            {
                // Find the file within the temporary directory.
                var exp = string.Concat(installRoot.FullPath, "/**/", path);
                var foundFile = _globber.Match(exp).FirstOrDefault();
                if (foundFile == null)
                {
                    var format = "Could not find file {0}.";
                    var message = string.Format(CultureInfo.InvariantCulture, format, path);
                    throw new CakeException(message);
                }

                var source = _fileSystem.GetFile((FilePath)foundFile);
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

        private static IPackageManager CreatePackageManager(DirectoryPath path)
        {
            var sources = new[]
            {
                new PackageSource("https://packages.nuget.org/api/v2"),
            };
            var repo = AggregateRepository.Create(PackageRepositoryFactory.Default, sources, false);
            return new PackageManager(repo, path.FullPath);
        }
    }
}
