// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core
{
    /// <summary>
    /// Represents an error that occurred while running an external tool process.
    /// </summary>
    public sealed class CakeProcessException : CakeException
    {
        /// <summary>
        /// Gets the standard output captured from the process, if redirection was enabled.
        /// </summary>
        public IReadOnlyList<string> StandardOutput { get; }

        /// <summary>
        /// Gets the standard error captured from the process, if redirection was enabled.
        /// </summary>
        public IReadOnlyList<string> StandardError { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeProcessException"/> class.
        /// </summary>
        /// <param name="exitCode">The process exit code.</param>
        /// <param name="message">The error message.</param>
        /// <param name="standardOutput">The standard output lines.</param>
        /// <param name="standardError">The standard error lines.</param>
        public CakeProcessException(int exitCode, string message, IEnumerable<string> standardOutput, IEnumerable<string> standardError)
            : base(exitCode, message)
        {
            StandardOutput = standardOutput?.ToArray() ?? Array.Empty<string>();
            StandardError = standardError?.ToArray() ?? Array.Empty<string>();
        }
    }
}
