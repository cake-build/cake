// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Event data for the <see cref="ICakeEngine.BeforeTeardown"/> event.
    /// </summary>
    public sealed class BeforeTeardownEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the teardown context.
        /// </summary>
        public ITeardownContext TeardownContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeTeardownEventArgs"/> class.
        /// </summary>
        /// <param name="teardownContext">The teardown context.</param>
        public BeforeTeardownEventArgs(ITeardownContext teardownContext)
        {
            TeardownContext = teardownContext;
        }
    }
}