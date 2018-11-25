// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Represents Cake task criteria.
    /// </summary>
    public sealed class CakeTaskCriteria
    {
        /// <summary>
        /// Gets the criteria predicate.
        /// </summary>
        /// <value>The criteria predicate.</value>
        public Func<ICakeContext, bool> Predicate { get; }

        /// <summary>
        /// Gets the criteria message.
        /// </summary>
        /// <value>The criteria message.</value>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeTaskCriteria"/> class.
        /// </summary>
        /// <param name="predicate">The criteria predicate.</param>
        /// <param name="message">The criteria message if skipped.</param>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null.</exception>
        public CakeTaskCriteria(Func<ICakeContext, bool> predicate, string message = null)
        {
            Predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            Message = message ?? string.Empty;
        }
    }
}