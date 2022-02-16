// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Frosting
{
    /// <summary>
    /// Base class for the Setup/Teardown logic of a Cake run.
    /// </summary>
    /// <seealso cref="ICakeContext" />
    public abstract class FrostingLifetime : FrostingLifetime<ICakeContext>
    {
    }

    /// <summary>
    /// Base class for the Setup/Teardown logic of a Cake run.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="ICakeContext" />
    public abstract class FrostingLifetime<TContext> : IFrostingLifetime
        where TContext : ICakeContext
    {
        /// <summary>
        /// This method is executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="context">The context.</param>
        public abstract void Setup(TContext context);

        /// <summary>
        /// This method is executed after all tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="info">The teardown information.</param>
        public abstract void Teardown(TContext context, ITeardownContext info);

        /// <inheritdoc/>
        void IFrostingSetup.Setup(ICakeContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Setup((TContext)context);
        }

        /// <inheritdoc/>
        void IFrostingTeardown.Teardown(ICakeContext context, ITeardownContext info)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (info is null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            Teardown((TContext)context, info);
        }
    }
}
