// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Acts as a context providing info about the overall build following its completion.
    /// </summary>
    public interface ITeardownContext : ICakeContext
    {
        /// <summary>
        /// Gets a value indicating whether this build was successful.
        /// </summary>
        /// <value>
        /// <c>true</c> if successful; otherwise <c>false</c>.
        /// </value>
        bool Successful { get; }

        /// <summary>
        /// Gets the exception that was thrown by the target.
        /// </summary>
        Exception ThrownException { get; }
    }
}