// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Frosting
{
    /// <summary>
    /// Represent errors that occur during execution of Cake.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public sealed class FrostingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrostingException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public FrostingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrostingException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public FrostingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
