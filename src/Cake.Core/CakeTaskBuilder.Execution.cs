// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cake.Core
{
    public static partial class CakeTaskBuilderExtensions
    {
        /// <summary>
        /// Adds an action to be executed when the task is invoked.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder Does(this CakeTaskBuilder builder, Action action)
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
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder Does<TData>(this CakeTaskBuilder builder, Action<TData> action)
            where TData : class
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return Does<TData>(builder, (context, data) => action(data));
        }

        /// <summary>
        /// Adds an action to be executed when the task is invoked.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="func">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder Does(this CakeTaskBuilder builder, Func<Task> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return Does(builder, context => func());
        }

        /// <summary>
        /// Adds an action to be executed when the task is invoked.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder Does(this CakeTaskBuilder builder, Action<ICakeContext> action)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            builder.Target.AddAction(x =>
            {
                action(x);
                return Task.CompletedTask;
            });

            return builder;
        }

        /// <summary>
        /// Adds an action to be executed when the task is invoked.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="func">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder Does(this CakeTaskBuilder builder, Func<ICakeContext, Task> func)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            builder.Target.AddAction(func);

            return builder;
        }

        /// <summary>
        /// Adds an action to be executed when the task is invoked.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="func">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder Does<TData>(this CakeTaskBuilder builder, Func<TData, Task> func)
            where TData : class
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return Does<TData>(builder, (context, data) => func(data));
        }

        /// <summary>
        /// Adds an action to be executed when the task is invoked.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder Does<TData>(this CakeTaskBuilder builder, Action<ICakeContext, TData> action)
            where TData : class
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return Does(builder, context =>
            {
                var data = context.Data.Get<TData>();
                action(context, data);
            });
        }

        /// <summary>
        /// Adds an action to be executed when the task is invoked.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="func">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder Does<TData>(this CakeTaskBuilder builder, Func<ICakeContext, TData, Task> func)
            where TData : class
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return Does(builder, context =>
            {
                var data = context.Data.Get<TData>();
                return func(context, data);
            });
        }

        /// <summary>
        /// Adds an action to be executed foreach item in the list.
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="items">The items.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, IEnumerable<TItem> items, Action<TItem> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return DoesForEach(builder, items, (item, context) => action(item));
        }

        /// <summary>
        /// Adds an action to be executed foreach item in the list.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="items">The items.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, IEnumerable<TItem> items, Action<TData, TItem> action)
            where TData : class
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return DoesForEach<TData, TItem>(builder, items, (data, item, context) => action(data, item));
        }

        /// <summary>
        /// Adds an action to be executed foreach item in the list.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="items">The items.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, IEnumerable<TItem> items, Action<TData, TItem, ICakeContext> action)
            where TData : class
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return DoesForEach(builder, items, (item, context) =>
            {
                var data = context.Data.Get<TData>();
                action(data, item, context);
            });
        }

        /// <summary>
        /// Adds an action to be executed foreach item in the list.
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="items">The items.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, IEnumerable<TItem> items, Action<TItem, ICakeContext> action)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in items)
            {
                builder.Target.AddAction(context =>
                {
                    action(item, context);
                    return Task.CompletedTask;
                });
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
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, Func<IEnumerable<TItem>> itemsFunc, Action<TItem> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return DoesForEach(builder, itemsFunc,
                (item, context) => action(item));
        }

        /// <summary>
        /// Adds an action to be executed foreach item returned by the items function.
        /// This method will be executed the first time the task is executed.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="itemsFunc">The items.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<IEnumerable<TItem>> itemsFunc, Action<TData, TItem> action)
            where TData : class
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return DoesForEach<TData, TItem>(builder, itemsFunc,
                (data, item, context) => action(data, item));
        }

        /// <summary>
        /// Adds an action to be executed foreach item returned by the items function.
        /// This method will be executed the first time the task is executed.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="itemsFunc">The items.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<IEnumerable<TItem>> itemsFunc, Action<TData, TItem, ICakeContext> action)
            where TData : class
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return DoesForEach(builder, itemsFunc, (item, context) =>
            {
                var data = context.Data.Get<TData>();
                action(data, item, context);
            });
        }

        /// <summary>
        /// Adds an action to be executed foreach item returned by the items function.
        /// This method will be executed the first time the task is executed.
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="itemsFunc">The items.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, Func<IEnumerable<TItem>> itemsFunc, Action<TItem, ICakeContext> action)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            builder.Target.AddDelayedAction(() =>
            {
                foreach (var item in itemsFunc())
                {
                    builder.Target.AddAction(context =>
                    {
                        action(item, context);
                        return Task.CompletedTask;
                    });
                }
            });

            return builder;
        }
    }
}
