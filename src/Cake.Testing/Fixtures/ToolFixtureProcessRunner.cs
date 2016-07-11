// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Testing.Fixtures
{
    /// <summary>
    /// The tool fixture process runner.
    /// </summary>
    /// <typeparam name="TFixtureResult">The type of the fixture result.</typeparam>
    public sealed class ToolFixtureProcessRunner<TFixtureResult> : IProcessRunner
        where TFixtureResult : ToolFixtureResult
    {
        private readonly Func<FilePath, ProcessSettings, TFixtureResult> _factory;
        private readonly List<TFixtureResult> _results;

        /// <summary>
        /// Gets or sets the process that will be returned
        /// when starting a process.
        /// </summary>
        /// <value>The process.</value>
        public FakeProcess Process { get; set; }

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <value>The results.</value>
        public IReadOnlyList<TFixtureResult> Results
        {
            get { return _results; }
        }

        internal ToolFixtureProcessRunner(Func<FilePath, ProcessSettings, TFixtureResult> factory)
        {
            _factory = factory;
            _results = new List<TFixtureResult>();

            Process = new FakeProcess();
        }

        /// <summary>
        /// Starts the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The (fake) tool fixture process.</returns>
        public IProcess Start(FilePath filePath, ProcessSettings settings)
        {
            // Invoke the intercept action.
            _results.Add(_factory(filePath, settings));

            // Return a dummy result.
            return Process;
        }
    }
}
