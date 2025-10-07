﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    public static partial class CakeTaskBuilderExtensions
    {
        /// <summary>
        /// Creates a dependency between two tasks.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="name">The name of the dependent task.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder IsDependentOn(this CakeTaskBuilder builder, string name)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Target.AddDependency(name);
            return builder;
        }

        /// <summary>
        /// Creates a dependency between two tasks.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="other">The name of the dependent task.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder IsDependentOn(this CakeTaskBuilder builder, CakeTaskBuilder other)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(other);

            builder.Target.AddDependency(other.Target.Name);
            return builder;
        }

        /// <summary>
        /// Makes the task a dependency of another task.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="name">The name of the task the current task will be a dependency of.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder IsDependeeOf(this CakeTaskBuilder builder, string name)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Target.AddDependee(name);
            return builder;
        }

        /// <summary>
        /// Makes the task a dependency of another task.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="other">The name of the dependent task.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder IsDependeeOf(this CakeTaskBuilder builder, CakeTaskBuilder other)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(other);

            builder.Target.AddDependee(other.Target.Name);
            return builder;
        }
    }
}
