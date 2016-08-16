﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;

namespace Cake.Testing
{
    /// <summary>
    /// A log message produced by <see cref="FakeLog"/>.
    /// </summary>
    public sealed class FakeLogMessage
    {
        /// <summary>
        /// Gets the verbosity.
        /// </summary>
        /// <value>The verbosity.</value>
        public Verbosity Verbosity { get; }

        /// <summary>
        /// Gets the log level.
        /// </summary>
        /// <value>The log level.</value>
        public LogLevel Level { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeLogMessage"/> class.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The log level.</param>
        /// <param name="message">The message.</param>
        public FakeLogMessage(Verbosity verbosity, LogLevel level, string message)
        {
            Level = level;
            Verbosity = verbosity;
            Message = message;
        }
    }
}