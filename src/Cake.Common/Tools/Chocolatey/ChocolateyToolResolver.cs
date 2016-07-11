// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.Chocolatey
{
    /// <summary>
    /// Contains Chocolatey path resolver functionality
    /// </summary>
    public sealed class ChocolateyToolResolver : IChocolateyToolResolver
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private IFile _cachedPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyToolResolver" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        public ChocolateyToolResolver(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            _fileSystem = fileSystem;
            _environment = environment;

            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
        }

        /// <summary>
        /// Resolves the path to choco.exe.
        /// </summary>
        /// <returns>The path to choco.exe.</returns>
        public FilePath ResolvePath()
        {
            // Check if path allready resolved
            if (_cachedPath != null && _cachedPath.Exists)
            {
                return _cachedPath.Path;
            }

            // Check if path set to environment variable
            var chocolateyInstallationFolder = _environment.GetEnvironmentVariable("ChocolateyInstall");
            if (!string.IsNullOrWhiteSpace(chocolateyInstallationFolder))
            {
                var envFile = _fileSystem.GetFile(System.IO.Path.Combine(chocolateyInstallationFolder, "choco.exe"));
                if (envFile.Exists)
                {
                    _cachedPath = envFile;
                    return _cachedPath.Path;
                }
            }

            // Last resort try path
            var envPath = _environment.GetEnvironmentVariable("path");
            if (!string.IsNullOrWhiteSpace(envPath))
            {
                var pathFile = envPath
                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(path => _fileSystem.GetDirectory(path))
                    .Where(path => path.Exists)
                    .Select(path => path.Path.CombineWithFilePath("choco.exe"))
                    .Select(_fileSystem.GetFile)
                    .FirstOrDefault(file => file.Exists);

                if (pathFile != null)
                {
                    _cachedPath = pathFile;
                    return _cachedPath.Path;
                }
            }

            throw new CakeException("Could not locate choco.exe.");
        }
    }
}
