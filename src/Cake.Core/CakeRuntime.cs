// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Versioning;
using Cake.Core.Polyfill;

namespace Cake.Core
{
    /// <inheritdoc/>
    public sealed class CakeRuntime : ICakeRuntime
    {
        /// <inheritdoc/>
        public FrameworkName BuiltFramework { get; }

        /// <inheritdoc/>
        public Runtime Runtime { get; }

        /// <inheritdoc/>
        public Version CakeVersion { get; }

        /// <inheritdoc/>
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
