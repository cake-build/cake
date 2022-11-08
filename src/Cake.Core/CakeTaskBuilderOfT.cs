// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Allows configuration to be performed for a registered <see cref="CakeTask"/>.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    public sealed class CakeTaskBuilder<TData>
        where TData : class
    {
        /// <summary>
        /// Gets a read-only representation of the task being configured.
        /// </summary>
        public ICakeTaskInfo Task => Builder.Task;

        /// <summary>
        /// Gets the cake task builder.
        /// </summary>
        public CakeTaskBuilder Builder { get; }

        internal CakeTask Target => Builder.Target;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeTaskBuilder{TData}"/> class.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        public CakeTaskBuilder(CakeTaskBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            Builder = builder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeTaskBuilder{TData}"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is null.</exception>
        public CakeTaskBuilder(CakeTask task)
        {
            ArgumentNullException.ThrowIfNull(task);

            Builder = new CakeTaskBuilder(task);
        }
    }
}