// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Frosting
{
    /// <summary>
    /// Base class for the lifetime for a task.
    /// </summary>
    /// <seealso cref="ICakeContext" />
    public abstract class FrostingTaskLifetime : FrostingTaskLifetime<ICakeContext>
    {
    }

    /// <summary>
    /// Base class for the lifetime for a task.
    /// </summary>
    /// <typeparam name="TContext">The build context type.</typeparam>
    /// <seealso cref="ICakeContext" />
    public abstract class FrostingTaskLifetime<TContext> : IFrostingTaskLifetime
        where TContext : ICakeContext
    {
        /// <summary>
        /// This method is executed before each task is run.
        /// If the task setup fails, the task will not be executed but the task's teardown will be performed.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="info">The setup information.</param>
        public abstract void Setup(TContext context, ITaskSetupContext info);

        /// <summary>
        /// This method is executed after each task have been run.
        /// If a task setup action or a task fails with or without recovery, the specified task teardown action will still be executed.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="info">The teardown information.</param>
        public abstract void Teardown(TContext context, ITaskTeardownContext info);

        /// <inheritdoc/>
        void IFrostingTaskSetup.Setup(ICakeContext context, ITaskSetupContext info)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            if (info is null)
            {
                throw new System.ArgumentNullException(nameof(info));
            }

            Setup((TContext)context, info);
        }

        /// <inheritdoc/>
        void IFrostingTaskTeardown.Teardown(ICakeContext context, ITaskTeardownContext info)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            if (info is null)
            {
                throw new System.ArgumentNullException(nameof(info));
            }

            Teardown((TContext)context, info);
        }
    }
}