// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Cake.Core
{
    /// <inheritdoc/>
    public sealed class TeardownContext : CakeContextAdapter, ITeardownContext
    {
        /// <inheritdoc/>
        public bool Successful => ThrownException == null;

        /// <inheritdoc/>
        public Exception ThrownException { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeardownContext"/> class.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <param name="throwException">The exception that was thrown by the target.</param>
        public TeardownContext(ICakeContext context, Exception throwException)
            : base(context)
        {
            ThrownException = throwException;
        }
    }
}