// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// [deprecated] TeardownEventArgs is obsolete and will be removed in a future release. Use <see cref="BeforeTeardownEventArgs" /> instead.
    /// Event data for the <see cref="ICakeEngine.Teardown"/> event.
    /// </summary>
    [Obsolete("TeardownEventArgs is obsolete and will be removed in a future release. Use BeforeTeardownEventArgs instead.")]
    public sealed class TeardownEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the teardown context.
        /// </summary>
        public ITeardownContext TeardownContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeardownEventArgs"/> class.
        /// </summary>
        /// <param name="teardownContext">The teardown context.</param>
        public TeardownEventArgs(ITeardownContext teardownContext)
        {
            TeardownContext = teardownContext;
        }
    }
}
