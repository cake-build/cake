﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Represent errors that occur during script execution.
    /// </summary>
    public sealed class CakeException : Exception
    {
        /// <summary>
        /// Gets custom exit code.
        /// </summary>
        public int ExitCode { get; } = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeException"/> class.
        /// </summary>
        public CakeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CakeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public CakeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeException"/> class.
        /// </summary>
        /// <param name="exitCode">Exit code.</param>
        public CakeException(int exitCode)
        {
            ExitCode = exitCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeException"/> class.
        /// </summary>
        /// <param name="exitCode">Custom exit code.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public CakeException(int exitCode, string message)
            : base(message)
        {
            ExitCode = exitCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeException"/> class.
        /// </summary>
        /// <param name="exitCode">Custom exit code.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public CakeException(int exitCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ExitCode = exitCode;
        }
    }
}