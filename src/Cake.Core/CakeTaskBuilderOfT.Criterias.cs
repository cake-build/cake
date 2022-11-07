// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    public static partial class CakeTaskBuilderOfTExtensions
    {
        /// <summary>
        /// Adds a criteria that has to be fulfilled for the task to run.
        /// The criteria is evaluated when traversal of the graph occurs.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> WithCriteria<TData>(
            this CakeTaskBuilder<TData> builder,
            Func<ICakeContext, TData, bool> criteria)
            where TData : class
            => builder.Process(builder => builder.WithCriteria(criteria));

        /// <summary>
        /// Adds a criteria that has to be fulfilled for the task to run.
        /// The criteria is evaluated when traversal of the graph occurs.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="message">The message to display if the task was skipped due to the provided criteria.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> WithCriteria<TData>(
            this CakeTaskBuilder<TData> builder,
            Func<ICakeContext, TData, bool> criteria,
            string message)
            where TData : class
            => builder.Process(builder => builder.WithCriteria(criteria, message));
    }
}