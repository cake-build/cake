// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Testing.Xunit
{
    /// <summary>
    /// Represents the runtime environment for tests.
    /// </summary>
    [Flags]
    public enum TestRuntime
    {
        /// <summary>
        /// The .NET Framework Common Language Runtime.
        /// </summary>
        Clr = 1,
        /// <summary>
        /// The .NET Core Common Language Runtime.
        /// </summary>
        CoreClr = 2
    }
}