// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Frosting.Example
{
    public class Settings : FrostingContext
    {
        public bool Magic { get; set; }

        public Settings(ICakeContext context)
            : base(context)
        {
            // You could also use a CakeLifeTime<Settings>
            // to provide a Setup method to setup the context.
            Magic = context.Arguments.HasArgument("magic");
        }
    }
}