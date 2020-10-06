// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Frosting
{
    /// <summary>
    /// Default implementation of the build context.
    /// </summary>
    /// <seealso cref="CakeContextAdapter" />
    /// <seealso cref="IFrostingContext" />
    public class FrostingContext : CakeContextAdapter, IFrostingContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrostingContext"/> class.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        public FrostingContext(ICakeContext context)
            : base(context)
        {
        }
    }
}
