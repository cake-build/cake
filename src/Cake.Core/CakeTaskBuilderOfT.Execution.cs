// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cake.Core
{
    public static partial class CakeTaskBuilderOfTExtensions
    {
        /// <summary>
        /// Adds an action to be executed when the task is invoked.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> Does<TData>(
        this CakeTaskBuilder<TData> builder,
        Action<ICakeContext, TData> action)
            where TData : class
            => builder.Process(builder => builder.Does(action));

        /// <summary>
        /// Adds an action to be executed when the task is invoked.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> Does<TData>(
            this CakeTaskBuilder<TData> builder,
            Func<ICakeContext, TData, Task> action)
            where TData : class
            => builder.Process(builder => builder.Does(action));

        /// <summary>
        /// Adds an action to be executed foreach item returned by the items function.
        /// This method will be executed the first time the task is executed.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="itemsFunc">The items.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> DoesForEach<TData, TItem>(
            this CakeTaskBuilder<TData> builder,
            Func<TData, ICakeContext, IEnumerable<TItem>> itemsFunc,
            Action<TData, TItem, ICakeContext> action)
            where TData : class
            => builder.Process(builder => builder.DoesForEach(itemsFunc, action));

        /// <summary>
        /// Adds an action to be executed foreach item returned by the items function.
        /// This method will be executed the first time the task is executed.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="items">The items.</param>
        /// <param name="action">The action.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> DoesForEach<TData, TItem>(
            this CakeTaskBuilder<TData> builder,
            IEnumerable<TItem> items,
            Action<TData, TItem, ICakeContext> action)
            where TData : class
            => builder.Process(builder => builder.DoesForEach(items, action));
    }
}