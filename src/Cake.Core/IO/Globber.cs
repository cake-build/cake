// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO.Globbing;

namespace Cake.Core.IO
{
    /// <summary>
    /// The file system globber.
    /// </summary>
    public sealed class Globber : IGlobber
    {
        private readonly GlobParser _parser;
        private readonly GlobVisitor _visitor;
        private readonly PathComparer _comparer;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Globber"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        public Globber(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            _environment = environment;
            _parser = new GlobParser(environment);
            _visitor = new GlobVisitor(fileSystem, environment);
            _comparer = new PathComparer(environment.Platform.IsUnix());
        }

        /// <summary>
        /// Returns <see cref="Path" /> instances matching the specified pattern.
        /// </summary>
        /// <param name="pattern">The pattern to match.</param>
        /// <param name="settings">The globber settings.</param>
        /// <returns>
        ///   <see cref="Path" /> instances matching the specified pattern.
        /// </returns>
        public IEnumerable<Path> Match(GlobPattern pattern, GlobberSettings settings)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }
            if (string.IsNullOrWhiteSpace(pattern?.Pattern))
            {
                return Enumerable.Empty<Path>();
            }

            // Parse the pattern into an AST.
            var root = _parser.Parse(pattern, settings);

            // Visit all nodes in the parsed patterns and filter the result.
            return _visitor.Walk(root, settings)
                .Select(x => x.Path)
                .Distinct(_comparer);
        }
    }
}