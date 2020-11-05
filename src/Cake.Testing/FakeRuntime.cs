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
        /// <inheritdoc/>
        public FrameworkName BuiltFramework { get; set; }

        /// <inheritdoc/>
        public Runtime Runtime { get; set; }

        /// <inheritdoc/>
        public Version CakeVersion { get; set; }

        /// <inheritdoc/>
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