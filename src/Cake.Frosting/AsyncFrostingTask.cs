// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Cake.Core;

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
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }
        }

        /// <summary>
        /// The finally handler to be executed using the specified context after the task has finished executing.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual void Finally(T context)
        {
        }

        /// <inheritdoc/>
        Task IFrostingTask.RunAsync(ICakeContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return RunAsync((T)context);
        }

        /// <inheritdoc/>
        bool IFrostingTask.ShouldRun(ICakeContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return ShouldRun((T)context);
        }

        /// <inheritdoc/>
        void IFrostingTask.OnError(Exception exception, ICakeContext context)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            OnError(exception, (T)context);
        }

        /// <inheritdoc/>
        void IFrostingTask.Finally(ICakeContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Finally((T)context);
        }
    }
}