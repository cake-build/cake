// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Frosting;

namespace Cake.Frosting
{
    /// <summary>
    /// Base class for setup logic.
    /// </summary>
    public abstract class FrostingSetup : FrostingSetup<ICakeContext>
    {
    }

    /// <summary>
    /// Base class for setup logic.
    /// </summary>
    /// <typeparam name="TContext">The build context type.</typeparam>
    public abstract class FrostingSetup<TContext> : IFrostingSetup
        where TContext : ICakeContext
    {
        /// <summary>
        /// This method is executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="context">The context.</param>
        public abstract void Setup(TContext context);

        void IFrostingSetup.Setup(ICakeContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Setup((TContext)context);
        }
    }
}
