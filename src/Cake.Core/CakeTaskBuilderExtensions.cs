// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="CakeTaskBuilder{T}"/>.
    /// </summary>
    public static class CakeTaskBuilderExtensions
    {
        /// <summary>
        /// Creates a dependency between two tasks.
        /// </summary>
        /// <typeparam name="T">The task type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="name">The name of the dependent task.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{T}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<T> IsDependentOn<T>(this CakeTaskBuilder<T> builder, string name)
            where T : CakeTask
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Task.AddDependency(name);
            return builder;
        }

        /// <summary>
        /// Creates a dependency between two tasks.
        /// </summary>
        /// <typeparam name="T">The task type.</typeparam>
        /// <typeparam name="TOther">The task type that this task depends on.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="other">The name of the dependent task.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{T}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<T> IsDependentOn<T, TOther>(this CakeTaskBuilder<T> builder, CakeTaskBuilder<TOther> other)
            where T : CakeTask
            where TOther : CakeTask
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            builder.Task.AddDependency(other.Task.Name);
            return builder;
        }

        /// <summary>
        /// Creates a dependency between two tasks.
        /// </summary>
        /// <typeparam name="T">The task type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="name">The name of the dependent task.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{T}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<T> IsDependentOnIfExists<T>(this CakeTaskBuilder<T> builder, string name)
            where T : CakeTask
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Task.AddDependency(name, true);
            return builder;
        }

        /// <summary>
        /// Adds a criteria that has to be fulfilled for the task to run.
        /// </summary>
        /// <typeparam name="T">The task type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{T}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<T> WithCriteria<T>(this CakeTaskBuilder<T> builder, bool criteria)
            where T : CakeTask
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Task.AddCriteria(_ => criteria);
            return builder;
        }

        /// <summary>
        /// Adds a criteria that has to be fulfilled for the task to run.
        /// The criteria is evaluated when traversal of the graph occurs.
        /// </summary>
        /// <typeparam name="T">The task type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{T}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<T> WithCriteria<T>(this CakeTaskBuilder<T> builder, Func<bool> criteria)
            where T : CakeTask
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Task.AddCriteria(criteria);
            return builder;
        }

        /// <summary>
        /// Adds a criteria that has to be fulfilled for the task to run.
        /// The criteria is evaluated when traversal of the graph occurs.
        /// </summary>
        /// <typeparam name="T">The task type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{T}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<T> WithCriteria<T>(this CakeTaskBuilder<T> builder, Func<ICakeContext, bool> criteria)
            where T : CakeTask
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Task.AddCriteria(criteria);
            return builder;
        }

        /// <summary>
        /// Adds an action to be executed when the task is invoked.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{ActionTask}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<ActionTask> Does(this CakeTaskBuilder<ActionTask> builder, Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            return Does(builder, context => action());
        }

        /// <summary>
        /// Adds an action to be executed when the task is invoked.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{ActionTask}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<ActionTask> Does(this CakeTaskBuilder<ActionTask> builder,
            Action<ICakeContext> action)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Task.AddAction(action);
            return builder;
        }

        /// <summary>
        /// Adds an action to be executed foreach item in the list.
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="items">The items.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{ActionTask}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<ActionTask> DoesForEach<TItem>(this CakeTaskBuilder<ActionTask> builder, IEnumerable<TItem> items, Action<TItem> action)
        {
            return DoesForEach(builder, items, (item, context) => action(item));
        }

        /// <summary>
        /// Adds an action to be executed foreach item in the list.
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="items">The items.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{ActionTask}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<ActionTask> DoesForEach<TItem>(this CakeTaskBuilder<ActionTask> builder, IEnumerable<TItem> items, Action<TItem, ICakeContext> action)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            foreach (var item in items)
            {
                builder.Task.AddAction(context => action(item, context));
            }
            return builder;
        }

        /// <summary>
        /// Adds an action to be executed foreach item returned by the items function.
        /// This method will be executed the first time the task is executed.
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="itemsFunc">The items.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{ActionTask}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<ActionTask> DoesForEach<TItem>(this CakeTaskBuilder<ActionTask> builder, Func<IEnumerable<TItem>> itemsFunc, Action<TItem> action)
        {
            return DoesForEach(builder, itemsFunc, (i, c) => action(i));
        }

        /// <summary>
        /// Adds an action to be executed foreach item returned by the items function.
        /// This method will be executed the first time the task is executed.
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="itemsFunc">The items.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{ActionTask}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<ActionTask> DoesForEach<TItem>(this CakeTaskBuilder<ActionTask> builder, Func<IEnumerable<TItem>> itemsFunc, Action<TItem, ICakeContext> action)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Task.AddDelayedAction(() =>
            {
                foreach (var item in itemsFunc())
                {
                    builder.Task.AddAction(context => action(item, context));
                }
            });
            return builder;
        }

        /// <summary>
        /// Defers all exceptions until after all actions for this task have completed
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{ActionTask}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<ActionTask> DeferOnError(this CakeTaskBuilder<ActionTask> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Task.SetDeferExceptions(true);
            return builder;
        }

        /// <summary>
        /// Adds a description to the task.
        /// </summary>
        /// <typeparam name="T">The task type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="description">The description.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{T}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<T> Description<T>(this CakeTaskBuilder<T> builder, string description)
            where T : CakeTask
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Task.Description = description;
            return builder;
        }

        /// <summary>
        /// Adds an indication to the task that a thrown exception will not halt the script execution.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{ActionTask}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<ActionTask> ContinueOnError(this CakeTaskBuilder<ActionTask> builder)
        {
            return OnError(builder, () => { });
        }

        /// <summary>
        /// Adds an error handler to be executed if an exception occurs in the task.
        /// </summary>
        /// <typeparam name="T">The task type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="errorHandler">The error handler.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{T}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<T> OnError<T>(this CakeTaskBuilder<T> builder, Action errorHandler)
            where T : CakeTask
        {
            return OnError(builder, exception => errorHandler());
        }

        /// <summary>
        /// Adds an error handler to be executed if an exception occurs in the task.
        /// </summary>
        /// <typeparam name="T">The task type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="errorHandler">The error handler.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{T}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<T> OnError<T>(this CakeTaskBuilder<T> builder, Action<Exception> errorHandler)
            where T : CakeTask
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Task.SetErrorHandler(errorHandler);
            return builder;
        }

        /// <summary>
        /// Adds a finally handler to be executed after the task have finished executing.
        /// </summary>
        /// <typeparam name="T">The task type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="finallyHandler">The finally handler.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{T}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<T> Finally<T>(this CakeTaskBuilder<T> builder, Action finallyHandler)
            where T : CakeTask
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.Task.SetFinallyHandler(finallyHandler);
            return builder;
        }

        /// <summary>
        /// Adds an error reporter for the task to be executed when an exception is thrown from the task.
        /// This action is invoked before the error handler, but gives no opportunity to recover from the error.
        /// </summary>
        /// <typeparam name="T">The task type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="errorReporter">The finally handler.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{T}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<T> ReportError<T>(this CakeTaskBuilder<T> builder, Action<Exception> errorReporter)
            where T : CakeTask
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.Task.SetErrorReporter(errorReporter);
            return builder;
        }
    }
}