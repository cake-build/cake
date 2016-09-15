// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Cake.Core
{
    /// <summary>
    /// Acts as a context providing info about the overall build following its completion
    /// </summary>
    public sealed class TeardownContext : CakeContextAdapter, ITeardownContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeardownContext"/> class.
        /// </summary>
        /// <param name="context">The Cake Context</param>
        /// <param name="throwException">The exception that was thrown by the target.</param>
        public TeardownContext(ICakeContext context, Exception throwException)
            : base(context)
        {
            ThrownException = throwException;
        }

        /// <summary>
        /// Gets a value indicating whether this build was successful
        /// </summary>
        /// <value>
        /// <c>true</c> if successful; otherwise <c>false</c>.
        /// </value>
        public bool Successful => ThrownException == null;

        /// <summary>
        /// Gets the exception that was thrown by the target.
        /// </summary>
        public Exception ThrownException { get; }
    }
}