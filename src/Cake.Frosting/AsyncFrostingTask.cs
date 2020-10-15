// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Frosting.Internal;

namespace Cake.Frosting
{
    /// <summary>
    /// Base class for an asynchronous Frosting task using the standard context.
    /// </summary>
    /// <seealso cref="ICakeContext" />
    public abstract class AsyncFrostingTask : AsyncFrostingTask<ICakeContext>
    {
    }

    /// <summary>
    /// Base class for an asynchronous Frosting task using a custom context.
    /// </summary>
    /// <typeparam name="T">The context type.</typeparam>
    /// <seealso cref="IFrostingTask" />
    public abstract class AsyncFrostingTask<T> : IFrostingTask
        where T : ICakeContext
    {
        /// <summary>
        /// Runs the task using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual Task RunAsync(T context)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets whether or not the task should be run.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if the task should run; otherwise <c>false</c>.
        /// </returns>
        public virtual bool ShouldRun(T context)
        {
            return true;
        }

        /// <summary>
        /// The error handler to be executed using the specified context if an exception occurs in the task.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="context">The context.</param>
        public virtual void OnError(Exception exception, T context)
        {
        }

        /// <summary>
        /// The finally handler to be executed using the specified context after the task has finished executing.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual void Finally(T context)
        {
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Explicit implementation.")]
        Task IFrostingTask.RunAsync(ICakeContext context)
        {
            Guard.ArgumentNotNull(context, nameof(context));

            return RunAsync((T)context);
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Explicit implementation.")]
        bool IFrostingTask.ShouldRun(ICakeContext context)
        {
            Guard.ArgumentNotNull(context, nameof(context));

            return ShouldRun((T)context);
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Explicit implementation.")]
        void IFrostingTask.OnError(Exception exception, ICakeContext context)
        {
            Guard.ArgumentNotNull(exception, nameof(exception));
            Guard.ArgumentNotNull(context, nameof(context));

            OnError(exception, (T)context);
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Explicit implementation.")]
        void IFrostingTask.Finally(ICakeContext context)
        {
            Guard.ArgumentNotNull(context, nameof(context));

            Finally((T)context);
        }
    }
}