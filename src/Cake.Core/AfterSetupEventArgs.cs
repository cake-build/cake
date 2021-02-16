// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Event data for the <see cref="ICakeEngine.AfterSetup"/> event.
    /// </summary>
    public sealed class AfterSetupEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the Cake context.
        /// </summary>
        public ICakeContext Context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AfterSetupEventArgs"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public AfterSetupEventArgs(ICakeContext context)
        {
            Context = context;
        }
    }
}