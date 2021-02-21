// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Core.Tooling
{
    /// <summary>
    /// Implementation of the default tool resolution strategy.
    /// </summary>
    public sealed class ToolResolutionStrategy : IToolResolutionStrategy
    {
        private static readonly Regex _windowsExtRegex = new Regex(@"\.(?:bat|cmd|exe)$", RegexOptions.IgnoreCase);

        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly ICakeConfiguration _configuration;
        private readonly ICakeLog _log;
        private readonly object _lock;
        private List<DirectoryPath> _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolResolutionStrategy"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="log">The log.</param>
        public ToolResolutionStrategy(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IGlobber globber,
            ICakeConfiguration configuration,
            ICakeLog log)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            if (globber == null)
            {
                throw new ArgumentNullException(nameof(globber));
            }

            _fileSystem = fileSystem;
            _environment = environment;
            _globber = globber;
            _configuration = configuration;
            _log = log;
            _lock = new object();
        }

        /// <inheritdoc/>
        public FilePath Resolve(IToolRepository repository, string tool)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            if (tool == null)
            {
                throw new ArgumentNullException(nameof(tool));
            }
            if (string.IsNullOrWhiteSpace(tool))
            {
                throw new ArgumentException("Tool name cannot be empty.", nameof(tool));
            }

            // Does this tool already have registrations?
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

        /// <inheritdoc/>
        public FilePath Resolve(IToolRepository repository, IEnumerable<string> toolExeNames)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            if (toolExeNames == null)
            {
                throw new ArgumentNullException(nameof(toolExeNames));
            }

            // Prefer tools with platform affinity
            var toolNames = toolExeNames.OrderByDescending(HasPlatformAffinity).ToArray();
            if (toolNames.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException("Tool names cannot be empty.", nameof(toolExeNames));
            }

            // Does this tool already have registrations?
            var resolve = toolNames.Select(tool => LookInRegistrations(repository, tool)).FirstOrDefault(tool => tool != null);
            if (resolve == null)
            {
                // Look in ./tools/
                resolve = toolNames.Select(LookInToolsDirectory).FirstOrDefault(tool => tool != null);
                if (resolve == null)
                {
                    // Look in the path environment variable.
                    resolve = toolNames.Select(LookInPath).FirstOrDefault(tool => tool != null);
                }
            }

            return resolve;
        }

        private bool HasPlatformAffinity(string tool)
        {
            // Platform affinity matches runtime platform with tool platform determined by file extension.
            return _environment.Platform.IsWindows() == _windowsExtRegex.IsMatch(tool);
        }

        private static FilePath LookInRegistrations(IToolRepository repository, string tool)
        {
            return repository.Resolve(tool).LastOrDefault();
        }

        private FilePath LookInToolsDirectory(string tool)
        {
            var pattern = string.Concat(GetToolsDirectory().FullPath, "/**/", tool);
            var toolPath = _globber.GetFiles(pattern).FirstOrDefault();
            return toolPath?.MakeAbsolute(_environment);
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
                    try
                    {
                        if (_fileSystem.Exist(file))
                        {
                            _log.Debug($"Resolved tool to path {file}");
                            return file.MakeAbsolute(_environment);
                        }
                    }
                    catch
                    {
                    }
                }

                var allPaths = string.Join(",", _path);
                _log.Debug($"Could not resolve path for tool \"{tool}\" using these directories: {allPaths}");
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
                var separator = new[] { _environment.Platform.IsUnix() ? ':' : ';' };
                var paths = path.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in paths)
                {
                    try
                    {
                        result.Add(new DirectoryPath(p.Trim(' ', '"', '\'')));
                    }
                    catch
                    {
                    }
                }
            }

            return result;
        }
    }
}