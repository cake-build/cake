// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Core.Tooling
{
    /// <summary>
    /// Implementation of the tool locator.
    /// </summary>
    public sealed class ToolLocator : IToolLocator
    {
        private readonly ICakeEnvironment _environment;
        private readonly IToolRepository _repository;
        private readonly IToolResolutionStrategy _strategy;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolLocator"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="repository">The tool repository.</param>
        /// <param name="strategy">The tool resolution strategy.</param>
        public ToolLocator(ICakeEnvironment environment, IToolRepository repository, IToolResolutionStrategy strategy)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        /// <inheritdoc/>
        public void RegisterFile(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            _repository.Register(path.MakeAbsolute(_environment));
        }

        /// <inheritdoc/>
        public FilePath Resolve(string tool)
        {
            if (tool == null)
            {
                throw new ArgumentNullException(nameof(tool));
            }
            if (string.IsNullOrWhiteSpace(tool))
            {
                throw new ArgumentException("Tool name cannot be empty.", nameof(tool));
            }

            return _strategy.Resolve(_repository, tool);
        }

        /// <inheritdoc/>
        public FilePath Resolve(IEnumerable<string> toolExeNames)
        {
            if (toolExeNames == null)
            {
                throw new ArgumentNullException(nameof(toolExeNames));
            }

            return _strategy.Resolve(_repository, toolExeNames);
        }
    }
}