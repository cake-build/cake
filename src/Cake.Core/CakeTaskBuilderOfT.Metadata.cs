// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="CakeTaskBuilder{TData}"/>.
    /// </summary>
    public static partial class CakeTaskBuilderOfTExtensions
    {
        internal static CakeTaskBuilder<TData> Process<TData>(
            this CakeTaskBuilder<TData> builder,
            Action<CakeTaskBuilder> action)
            where TData : class
        {
            ArgumentNullException.ThrowIfNull(builder);
            action(builder.Builder);
            return builder;
        }

        /// <summary>
        /// Gives a <see cref="CakeTaskBuilder{TData}"/> bound to specific data context type.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <returns>A <see cref="CakeTaskBuilder{TData}"/>.</returns>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        public static CakeTaskBuilder<TData> Of<TData>(this CakeTaskBuilder builder)
            where TData : class
            => new (builder);

        /// <summary>
        /// Adds a description to the task.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="description">The description.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> Description<TData>(
            this CakeTaskBuilder<TData> builder,
            string description)
            where TData : class
            => builder.Process(builder => builder.Description(description));
    }
}