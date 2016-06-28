// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;

namespace Cake.Testing.Fixtures
{
    /// <summary>
    /// Represents a tool fixture result.
    /// </summary>
    public class ToolFixtureResult
    {
        /// <summary>
        /// Gets the tool path.
        /// </summary>
        /// <value>The tool path.</value>
        public FilePath Path { get; }

        /// <summary>
        /// Gets the process settings.
        /// </summary>
        /// <value>The process settings.</value>
        public ProcessSettings Process { get; }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public string Args { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolFixtureResult"/> class.
        /// </summary>
        /// <param name="path">The tool path.</param>
        /// <param name="process">The process settings.</param>
        public ToolFixtureResult(FilePath path, ProcessSettings process)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (process == null)
            {
                throw new ArgumentNullException(nameof(process));
            }
            Path = path;
            Args = process.Arguments.Render();
            Process = process;
        }
    }
}