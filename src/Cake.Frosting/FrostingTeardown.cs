// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Frosting;

namespace Cake.Frosting
{
    /// <summary>
    /// Base class for teardown logic.
    /// </summary>
    public abstract class FrostingTeardown : FrostingTeardown<ICakeContext>
    {
    }

    /// <summary>
    /// Base class for teardown logic.
    /// </summary>
    /// <typeparam name="TContext">The build context type.</typeparam>
    public abstract class FrostingTeardown<TContext> : IFrostingTeardown
        where TContext : ICakeContext
    {
        /// <summary>
        /// This method is executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="info">The teardown information.</param>
        public abstract void Teardown(TContext context, ITeardownContext info);

        void IFrostingTeardown.Teardown(ICakeContext context, ITeardownContext info)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Teardown((TContext)context, info);
        }
    }
}
