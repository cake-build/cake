// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;

namespace Cake.Core.Tooling
{
    /// <summary>
    /// The tool repository.
    /// </summary>
    public sealed class ToolRepository : IToolRepository
    {
        private readonly ICakeEnvironment _environment;
        private readonly Dictionary<string, List<FilePath>> _paths;
        private readonly PathComparer _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolRepository"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public ToolRepository(ICakeEnvironment environment)
        {
            _environment = environment;
            _paths = new Dictionary<string, List<FilePath>>(StringComparer.OrdinalIgnoreCase);
            _comparer = new PathComparer(environment);
        }

        /// <summary>
        /// Registers the specified path with the repository.
        /// </summary>
        /// <param name="path">The path to register.</param>
        public void Register(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            path = path.MakeAbsolute(_environment);

            var filename = path.GetFilename();

            if (!_paths.ContainsKey(filename.FullPath))
            {
                _paths.Add(filename.FullPath, new List<FilePath>());
            }

            if (!_paths[filename.FullPath].Contains(path, _comparer))
            {
                _paths[filename.FullPath].Add(path);
            }
        }

        /// <summary>
        /// Resolves the specified tool.
        /// </summary>
        /// <param name="tool">The tool to resolve.</param>
        /// <returns>
        /// The tool's file paths if any; otherwise <c>null</c>.
        /// </returns>
        public IEnumerable<FilePath> Resolve(string tool)
        {
            if (_paths.ContainsKey(tool))
            {
                return _paths[tool];
            }

            return Enumerable.Empty<FilePath>();
        }
    }
}
