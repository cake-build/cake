// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Frosting
{
    /// <summary>
    /// The options that tells how Cake should be executed.
    /// </summary>
    public sealed class CakeHostOptions
    {
        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public IDictionary<string, string> Arguments { get; }

        /// <summary>
        /// Gets or sets the working directory.
        /// </summary>
        /// <value>The working directory.</value>
        public DirectoryPath WorkingDirectory { get; set; }

        /// <summary>
        /// Gets or sets the target to run.
        /// </summary>
        /// <value>The target to run.</value>
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the output verbosity.
        /// </summary>
        /// <value>The output verbosity.</value>
        public Verbosity Verbosity { get; set; }

        /// <summary>
        /// Gets or sets the command to execute.
        /// </summary>
        /// <value>The command to execute.</value>
        public CakeHostCommand Command { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeHostOptions"/> class.
        /// </summary>
        public CakeHostOptions()
        {
            Arguments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            WorkingDirectory = null;
            Target = "Default";
            Verbosity = Verbosity.Normal;
            Command = CakeHostCommand.Run;
        }
    }
}
