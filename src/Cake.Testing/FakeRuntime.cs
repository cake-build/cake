// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Versioning;
using Cake.Core;
using Cake.Core.Polyfill;

namespace Cake.Testing
{
    /// <summary>
    /// An implementation of a fake <see cref="ICakeRuntime"/>.
    /// </summary>
    public sealed class FakeRuntime : ICakeRuntime
    {
        /// <summary>
        /// Gets or sets the build-time .NET framework version that is being used.
        /// </summary>
        public FrameworkName BuiltFramework { get; set; }

        /// <summary>
        /// Gets or sets the current executing .NET Runtime.
        /// </summary>
        /// <returns>The target framework.</returns>
        public Runtime Runtime { get; set; }

        /// <summary>
        /// Gets or sets the version of Cake executing the script.
        /// </summary>
        public Version CakeVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we're running on CoreClr.
        /// </summary>
        /// <value>
        /// <c>true</c> if we're running on CoreClr; otherwise, <c>false</c>.
        /// </value>
        public bool IsCoreClr { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeRuntime"/> class.
        /// </summary>
        public FakeRuntime()
        {
            BuiltFramework = new FrameworkName(".NETFramework,Version=v4.6.2");
            Runtime = Runtime.Clr;
            CakeVersion = new Version(0, 1, 2);
            IsCoreClr = false;
        }
    }
}