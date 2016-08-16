// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Versioning;
using Cake.Core;

namespace Cake.Testing
{
    /// <summary>
    /// An implementation of a fake <see cref="ICakeRuntime"/>
    /// </summary>
    public sealed class FakeRuntime : ICakeRuntime
    {
        /// <summary>
        /// Gets or sets the target .NET framework version that the current AppDomain is targeting.
        /// </summary>
        public FrameworkName TargetFramework { get; set; }

        /// <summary>
        /// Gets or sets the version of Cake executing the script.
        /// </summary>
        public Version CakeVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we're running on CoreClr.
        /// </summary>
        /// <value>
        /// <c>true</c> if we're runnning on CoreClr; otherwise, <c>false</c>.
        /// </value>
        public bool IsCoreClr { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeRuntime"/> class.
        /// </summary>
        public FakeRuntime()
        {
            TargetFramework = new FrameworkName(".NETFramework,Version=v4.5");
            CakeVersion = new Version(0, 1, 2);
            IsCoreClr = false;
        }
    }
}