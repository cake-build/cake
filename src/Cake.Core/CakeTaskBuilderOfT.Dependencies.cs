// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core
{
    public static partial class CakeTaskBuilderOfTExtensions
    {
        /// <summary>
        /// Makes the task a dependee of another task.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="name">The name of the task the current task will be a dependency of.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> IsDependeeOf<TData>(
            this CakeTaskBuilder<TData> builder,
            string name)
            where TData : class
            => builder.Process(builder => builder.IsDependeeOf(name));

        /// <summary>
        /// Makes the task a dependee of another task.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="other">The dependent task.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> IsDependeeOf<TData>(
            this CakeTaskBuilder<TData> builder,
            CakeTaskBuilder<TData> other)
            where TData : class
            => builder.Process(builder => builder.IsDependeeOf(other.Builder));

        /// <summary>
        /// Creates a dependency between two tasks.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="name">The name of the dependent task.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> IsDependentOn<TData>(
            this CakeTaskBuilder<TData> builder,
            string name)
            where TData : class
            => builder.Process(builder => builder.IsDependentOn(name));

        /// <summary>
        /// Creates a dependency between two tasks.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="other">The dependent task.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> IsDependentOn<TData>(
            this CakeTaskBuilder<TData> builder,
            CakeTaskBuilder<TData> other)
            where TData : class
            => builder.Process(builder => builder.IsDependentOn(other?.Builder));

        /// <summary>
        /// Creates a dependency between two tasks.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="other">The dependent task.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> IsDependentOn<TData>(
            this CakeTaskBuilder<TData> builder,
            CakeTaskBuilder other)
            where TData : class
            => builder.Process(builder => builder.IsDependentOn(other));
    }
}