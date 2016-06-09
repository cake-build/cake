// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core.Tooling;

namespace Cake.Core.IO.NuGet
{
    /// <summary>
    /// Contains NuGet path resolver functionality
    /// </summary>
    public sealed class NuGetToolResolver : INuGetToolResolver
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IToolLocator _tools;
        private IFile _cachedPath;

        private static readonly FilePath[] _unixSystemPaths;

        static NuGetToolResolver()
        {
            _unixSystemPaths = new[]
            {
                new FilePath("/usr/local/bin/nuget"),
                new FilePath("/usr/bin/nuget")
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetToolResolver" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="tools">The tool locator.</param>
        public NuGetToolResolver(IFileSystem fileSystem, ICakeEnvironment environment, IToolLocator tools)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (tools == null)
            {
                throw new ArgumentNullException("tools");
            }
            _fileSystem = fileSystem;
            _environment = environment;
            _tools = tools;
        }

        /// <summary>
        /// Resolves the path to nuget.exe.
        /// </summary>
        /// <returns>The path to nuget.exe.</returns>
        public FilePath ResolvePath()
        {
            // Check if path already resolved
            if (_cachedPath != null && _cachedPath.Exists)
            {
                return _cachedPath.Path;
            }

            // Try to resolve it with the regular tool resolver.
            var toolsExe = _tools.Resolve("nuget.exe");
            if (toolsExe != null)
            {
                var toolsFile = _fileSystem.GetFile(toolsExe);
                if (toolsFile.Exists)
                {
                    _cachedPath = toolsFile;
                    return _cachedPath.Path;
                }
            }

            // Check if path set to environment variable
            var environmentExe = _environment.GetEnvironmentVariable("NUGET_EXE");
            if (!string.IsNullOrWhiteSpace(environmentExe))
            {
                var envFile = _fileSystem.GetFile(environmentExe);
                if (envFile.Exists)
                {
                    _cachedPath = envFile;
                    return _cachedPath.Path;
                }
            }

            // On Unix /usr/bin/nuget or /usr/local/bin/nuget are viable options
            if (_environment.IsUnix())
            {
                foreach (var systemPath in _unixSystemPaths)
                {
                    if (_fileSystem.Exist(systemPath))
                    {
                        _cachedPath = _fileSystem.GetFile(systemPath);
                        return _cachedPath.Path;
                    }
                }
            }

            throw new CakeException("Could not locate nuget.exe.");
        }
    }
}
