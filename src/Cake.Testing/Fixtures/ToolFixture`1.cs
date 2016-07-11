// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Testing.Fixtures
{
    /// <summary>
    /// Base class for tool fixtures.
    /// </summary>
    /// <typeparam name="TToolSettings">The type of the tool settings.</typeparam>
    public abstract class ToolFixture<TToolSettings> : ToolFixture<TToolSettings, ToolFixtureResult>
        where TToolSettings : ToolSettings, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolFixture{TToolSettings}"/> class.
        /// </summary>
        /// <param name="toolFilename">The tool filename.</param>
        protected ToolFixture(string toolFilename)
            : base(toolFilename)
        {
        }

        /// <summary>
        /// Creates a <see cref="ToolFixtureResult"/> from a tool path and process settings.
        /// </summary>
        /// <param name="path">The tool path.</param>
        /// <param name="process">The process settings.</param>
        /// <returns>A tool fixture result.</returns>
        protected sealed override ToolFixtureResult CreateResult(FilePath path, ProcessSettings process)
        {
            return new ToolFixtureResult(path, process);
        }
    }
}
