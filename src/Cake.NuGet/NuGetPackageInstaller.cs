// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Configuration;
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
        private readonly INuGetContentResolver _contentResolver;
        private readonly ICakeLog _log;

        private readonly ICakeConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackageInstaller"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="toolResolver">The NuGet tool resolver.</param>
        /// <param name="contentResolver">The content resolver.</param>
        /// <param name="log">The log.</param>
        /// <param name="config">the configuration</param>
        public NuGetPackageInstaller(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            INuGetToolResolver toolResolver,
            INuGetContentResolver contentResolver,
            ICakeLog log,
            ICakeConfiguration config)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            if (processRunner == null)
            {
                throw new ArgumentNullException(nameof(processRunner));
            }
            if (toolResolver == null)
            {
                throw new ArgumentNullException(nameof(toolResolver));
            }
            if (contentResolver == null)
            {
                throw new ArgumentNullException(nameof(contentResolver));
            }
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            _fileSystem = fileSystem;
            _environment = environment;
            _processRunner = processRunner;
            _toolResolver = toolResolver;
            _contentResolver = contentResolver;
            _log = log;
            _config = config;
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
                throw new ArgumentNullException(nameof(package));
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
                throw new ArgumentNullException(nameof(package));
            }
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
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
                Arguments = GetArguments(package, path, _config),
                RedirectStandardOutput = true,
                Silent = _log.Verbosity < Verbosity.Diagnostic
            });
            process.WaitForExit();

            var exitCode = process.GetExitCode();
            if (exitCode != 0)
            {
                _log.Warning("NuGet exited with {0}", exitCode);
                var output = string.Join(Environment.NewLine, process.GetStandardOutput());
                _log.Verbose(Verbosity.Diagnostic, "Output:\r\n{0}", output);
            }

            // Get the files.
            var result = _contentResolver.GetFiles(packagePath, type);
            if (result.Count == 0)
            {
                var framework = _environment.Runtime.TargetFramework;
                _log.Warning("Could not find any assemblies compatible with {0}.", framework.FullName);
            }

            return result;
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
            DirectoryPath installationRoot, ICakeConfiguration config)
        {
            var arguments = new ProcessArgumentBuilder();

            arguments.Append("install");
            arguments.AppendQuoted(definition.Package);

            // Output directory
            arguments.Append("-OutputDirectory");
            arguments.AppendQuoted(installationRoot.FullPath);

            // if an absolute uri is specified for source, use this
            // otherwise check config for customise package source/s
            if (definition.Address != null)
            {
                arguments.Append("-Source");
                arguments.AppendQuoted(definition.Address.AbsoluteUri);
            }
            else
            {
                var nugetSource = config.GetValue(Constants.NuGet.Source);
                if (!string.IsNullOrWhiteSpace(nugetSource))
                {
                    arguments.Append("-Source");
                    arguments.AppendQuoted(nugetSource);
                }
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