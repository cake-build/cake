// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;

namespace Cake.Testing.Fixtures
{
    /// <summary>
    /// Represents a tool fixture result.
    /// </summary>
    public class ToolFixtureResult
    {
        private readonly FilePath _path;
        private readonly ProcessSettings _process;
        private readonly string _args;

        /// <summary>
        /// Gets the tool path.
        /// </summary>
        /// <value>The tool path.</value>
        public FilePath Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets the process settings.
        /// </summary>
        /// <value>The process settings.</value>
        public ProcessSettings Process
        {
            get { return _process; }
        }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public string Args
        {
            get { return _args; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolFixtureResult"/> class.
        /// </summary>
        /// <param name="path">The tool path.</param>
        /// <param name="process">The process settings.</param>
        public ToolFixtureResult(FilePath path, ProcessSettings process)
        {
            _path = path;
            _args = process.Arguments.Render();
            _process = process;
        }
    }
}
