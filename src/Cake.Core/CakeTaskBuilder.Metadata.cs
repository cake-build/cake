// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="CakeTaskBuilder"/>.
    /// </summary>
    public static partial class CakeTaskBuilderExtensions
    {
        /// <summary>
        /// Adds a description to the task.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="description">The description.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder Description(this CakeTaskBuilder builder, string description)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Target.Description = description;
            return builder;
        }
    }
}