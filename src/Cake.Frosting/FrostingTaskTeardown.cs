// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Frosting;

namespace Cake.Frosting
{
    /// <summary>
    /// Base class for the teardown logic of a task.
    /// </summary>
    public abstract class FrostingTaskTeardown : FrostingTaskTeardown<ICakeContext>
    {
    }

    /// <summary>
    /// Base class for the teardown logic of a task.
    /// </summary>
    /// <typeparam name="TContext">The build context type.</typeparam>
    public abstract class FrostingTaskTeardown<TContext> : IFrostingTaskTeardown
        where TContext : ICakeContext
    {
        /// <summary>
        /// This method is executed after each task have been run.
        /// If a task setup action or a task fails with or without recovery, the specified task teardown action will still be executed.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="info">The teardown information.</param>
        public abstract void Teardown(TContext context, ITaskTeardownContext info);

        void IFrostingTaskTeardown.Teardown(ICakeContext context, ITaskTeardownContext info)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Teardown((TContext)context, info);
        }
    }
}
