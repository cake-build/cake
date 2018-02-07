// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Versioning;
using Cake.Core.Polyfill;

namespace Cake.Core
{
    /// <summary>
    /// Represents the runtime that Cake is running in.
    /// </summary>
    public sealed class CakeRuntime : ICakeRuntime
    {
        /// <summary>
        /// Gets the build-time .NET framework version that is being used.
        /// </summary>
        public FrameworkName BuiltFramework { get; }

        /// <summary>
        /// Gets the current executing .NET Runtime.
        /// </summary>
        /// <returns>The target framework.</returns>
        public Runtime Runtime { get; }

        /// <summary>
        /// Gets the version of Cake executing the script.
        /// </summary>
        public Version CakeVersion { get; }

        /// <summary>
        /// Gets a value indicating whether we're running on CoreClr.
        /// </summary>
        /// <value>
        /// <c>true</c> if we're runnning on CoreClr; otherwise, <c>false</c>.
        /// </value>
        public bool IsCoreClr { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeRuntime"/> class.
        /// </summary>
        public CakeRuntime()
        {
            BuiltFramework = EnvironmentHelper.GetBuiltFramework();
            Runtime = EnvironmentHelper.GetRuntime();
            CakeVersion = AssemblyHelper.GetExecutingAssembly().GetName().Version;
            IsCoreClr = EnvironmentHelper.IsCoreClr();
        }
    }
}
