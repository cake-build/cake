// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
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
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (strategy == null)
            {
                throw new ArgumentNullException("strategy");
            }

            _environment = environment;
            _repository = repository;
            _strategy = strategy;
        }

        /// <summary>
        /// Registers the specified tool file path.
        /// </summary>
        /// <param name="path">The tool path.</param>
        public void RegisterFile(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            _repository.Register(path.MakeAbsolute(_environment));
        }

        /// <summary>
        /// Resolves the path to the specified tool.
        /// </summary>
        /// <param name="tool">The tool.</param>
        /// <returns>A path if the tool was found; otherwise <c>null</c>.</returns>
        public FilePath Resolve(string tool)
        {
            if (tool == null)
            {
                throw new ArgumentNullException("tool");
            }
            if (string.IsNullOrWhiteSpace(tool))
            {
                throw new ArgumentException("Tool name cannot be empty.", "tool");
            }

            return _strategy.Resolve(_repository, tool);
        }
    }
}
