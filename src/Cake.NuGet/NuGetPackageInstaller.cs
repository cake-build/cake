// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Packaging;

namespace Cake.NuGet
{
    /// <summary>
    /// Installer for NuGet URI resources.
    /// </summary>
    public sealed class NuGetPackageInstaller : IPackageInstaller
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IProcessRunner _processRunner;
        private readonly INuGetToolResolver _toolResolver;
        private readonly INuGetPackageContentResolver _contentResolver;
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackageInstaller"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="toolResolver">The NuGet tool resolver.</param>
        /// <param name="contentResolver">The content resolver.</param>
        /// <param name="log">The log.</param>
        public NuGetPackageInstaller(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            INuGetToolResolver toolResolver,
            INuGetPackageContentResolver contentResolver,
            ICakeLog log)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (processRunner == null)
            {
                throw new ArgumentNullException("processRunner");
            }
            if (toolResolver == null)
            {
                throw new ArgumentNullException("toolResolver");
            }
            if (contentResolver == null)
            {
                throw new ArgumentNullException("contentResolver");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            _fileSystem = fileSystem;
            _environment = environment;
            _processRunner = processRunner;
            _toolResolver = toolResolver;
            _contentResolver = contentResolver;
            _log = log;
        }

        /// <summary>
        /// Determines whether this instance can install the specified resource.
        /// </summary>
        /// <param name="package">The package reference.</param>
        /// <param name="type">The package type.</param>
        /// <returns>
        ///   <c>true</c> if this installer can install the
        ///   specified resource; otherwise <c>false</c>.
        /// </returns>
        public bool CanInstall(PackageReference package, PackageType type)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }
            return package.Scheme.Equals("nuget", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Installs the specified resource at the given location.
        /// </summary>
        /// <param name="package">The package reference.</param>
        /// <param name="type">The package type.</param>
        /// <param name="path">The location where to install the package.</param>
        /// <returns>The installed files.</returns>
        public IReadOnlyCollection<IFile> Install(PackageReference package, PackageType type, DirectoryPath path)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            path = path.MakeAbsolute(_environment);

            var root = _fileSystem.GetDirectory(path);
            var packagePath = path.Combine(package.Package);

            // Create the addin directory if it doesn't exist.
            if (!root.Exists)
            {
                _log.Debug("Creating directory {0}", path);
                root.Create();
            }

            // Fetch available content from disc.
            var content = _contentResolver.GetFiles(packagePath, type);
            if (content.Any())
            {
                _log.Debug("Package {0} has already been installed.", package.Package);
                return content;
            }

            // Install the package.
            _log.Debug("Installing NuGet package {0}...", package.Package);
            var nugetPath = GetNuGetPath();
            var process = _processRunner.Start(nugetPath, new ProcessSettings
            {
                Arguments = GetArguments(package, path),
                RedirectStandardOutput = true,
                Silent = true
            });
            process.WaitForExit();

            // Return the files.
            return _contentResolver.GetFiles(packagePath, type);
        }

        private FilePath GetNuGetPath()
        {
            var nugetPath = _toolResolver.ResolvePath();
            if (nugetPath == null)
            {
                throw new CakeException("Failed to find nuget.exe.");
            }
            return nugetPath;
        }

        private static ProcessArgumentBuilder GetArguments(
            PackageReference definition,
            DirectoryPath installationRoot)
        {
            var arguments = new ProcessArgumentBuilder();

            arguments.Append("install");
            arguments.AppendQuoted(definition.Package);

            // Output directory
            arguments.Append("-OutputDirectory");
            arguments.AppendQuoted(installationRoot.FullPath);

            // Source
            if (definition.Address != null)
            {
                arguments.Append("-Source");
                arguments.AppendQuoted(definition.Address.AbsoluteUri);
            }

            // Version
            if (definition.Parameters.ContainsKey("version"))
            {
                arguments.Append("-Version");
                arguments.AppendQuoted(definition.Parameters["version"]);
            }

            // Prerelease
            if (definition.Parameters.ContainsKey("prerelease"))
            {
                arguments.Append("-Prerelease");
            }

            arguments.Append("-ExcludeVersion -NonInteractive -NoCache");
            return arguments;
        }
    }
}
