// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.Configuration;
using Cake.Core.IO;

namespace Cake.Core.Tooling
{
    /// <summary>
    /// Implementation of the default tool resolution strategy.
    /// </summary>
    public sealed class ToolResolutionStrategy : IToolResolutionStrategy
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly ICakeConfiguration _configuration;
        private readonly object _lock;
        private List<DirectoryPath> _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolResolutionStrategy"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="configuration">The configuration.</param>
        public ToolResolutionStrategy(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IGlobber globber,
            ICakeConfiguration configuration)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (globber == null)
            {
                throw new ArgumentNullException("globber");
            }

            _fileSystem = fileSystem;
            _environment = environment;
            _globber = globber;
            _configuration = configuration;
            _lock = new object();
        }

        /// <summary>
        /// Resolves the specified tool using the specified tool repository.
        /// </summary>
        /// <param name="repository">The tool repository.</param>
        /// <param name="tool">The tool.</param>
        /// <returns>
        /// The path to the tool; otherwise <c>null</c>.
        /// </returns>
        public FilePath Resolve(IToolRepository repository, string tool)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (tool == null)
            {
                throw new ArgumentNullException("tool");
            }
            if (string.IsNullOrWhiteSpace(tool))
            {
                throw new ArgumentException("Tool name cannot be empty.", "tool");
            }

            // Does this file already have registrations?
            var resolve = LookInRegistrations(repository, tool);
            if (resolve == null)
            {
                // Look in ./tools/
                resolve = LookInToolsDirectory(tool);
                if (resolve == null)
                {
                    // Look in the path environment variable.
                    resolve = LookInPath(tool);
                }
            }

            return resolve;
        }

        private static FilePath LookInRegistrations(IToolRepository repository, string tool)
        {
            return repository.Resolve(tool).LastOrDefault();
        }

        private FilePath LookInToolsDirectory(string tool)
        {
            var pattern = string.Concat(GetToolsDirectory().FullPath, "/**/", tool);
            var toolPath = _globber.GetFiles(pattern).FirstOrDefault();
            return toolPath != null ? toolPath.MakeAbsolute(_environment) : null;
        }

        private FilePath LookInPath(string tool)
        {
            lock (_lock)
            {
                if (_path == null)
                {
                    _path = GetPathDirectories();
                }

                foreach (var pathDir in _path)
                {
                    var file = pathDir.CombineWithFilePath(tool);
                    if (_fileSystem.Exist(file))
                    {
                        return file.MakeAbsolute(_environment);
                    }
                }

                return null;
            }
        }

        private DirectoryPath GetToolsDirectory()
        {
            var toolPath = _configuration.GetValue(Constants.Paths.Tools);
            if (!string.IsNullOrWhiteSpace(toolPath))
            {
                return new DirectoryPath(toolPath);
            }

            return new DirectoryPath("./tools");
        }

        private List<DirectoryPath> GetPathDirectories()
        {
            var result = new List<DirectoryPath>();
            var path = _environment.GetEnvironmentVariable("PATH");
            if (!string.IsNullOrEmpty(path))
            {
                var separator = new[] { _environment.IsUnix() ? ':' : ';' };
                var paths = path.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                result.AddRange(paths.Select(p => new DirectoryPath(p)));
            }

            return result;
        }
    }
}
