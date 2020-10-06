// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Cake.Core;
using Cake.Frosting.Internal;

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
    /// <typeparam name="TContext">The build script context type.</typeparam>
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
        public virtual void Setup(TContext context, ITaskSetupContext info)
        {
        }

        /// <summary>
        /// This method is executed after each task have been run.
        /// If a task setup action or a task fails with or without recovery, the specified task teardown action will still be executed.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="info">The teardown information.</param>
        public virtual void Teardown(TContext context, ITaskTeardownContext info)
        {
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Explicit implementation.")]
        void IFrostingTaskLifetime.Setup(ICakeContext context, ITaskSetupContext info)
        {
            Guard.ArgumentNotNull(context, nameof(context));
            Guard.ArgumentNotNull(info, nameof(info));

            Setup((TContext)context, info);
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Explicit implementation.")]
        void IFrostingTaskLifetime.Teardown(ICakeContext context, ITaskTeardownContext info)
        {
            Guard.ArgumentNotNull(context, nameof(context));
            Guard.ArgumentNotNull(info, nameof(info));

            Teardown((TContext)context, info);
        }
    }
}