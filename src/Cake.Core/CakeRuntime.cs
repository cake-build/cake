// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Runtime.Versioning;

namespace Cake.Core
{
    /// <summary>
    /// Represents the runtime that Cake is running in.
    /// </summary>
    public sealed class CakeRuntime : ICakeRuntime
    {
        /// <summary>
        /// Gets the target .NET framework version that the current AppDomain is targeting.
        /// </summary>
        public FrameworkName TargetFramework { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeRuntime"/> class.
        /// </summary>
        public CakeRuntime()
        {
            // Try to get the current framework name from the current application domain,
            // but if that is null, we default to .NET 4.5. The reason for doing this is
            // that this actually is what happens on Mono.
            var frameworkName = AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
            TargetFramework = new FrameworkName(frameworkName ?? ".NETFramework,Version=v4.5");
        }
    }
}
