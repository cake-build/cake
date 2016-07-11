// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Tools.VSTest
{
    /// <summary>
    /// Target platform architecture to be used for test execution.
    /// </summary>
    public enum VSTestPlatform
    {
        /// <summary>
        /// Use default platform architecture.
        /// </summary>
        Default,

        /// <summary>
        /// Platform architecture: <c>x86</c>.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        x86,

        /// <summary>
        /// Platform architecture: <c>x64</c>.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        x64,

        /// <summary>
        /// Platform architecture: <c>ARM</c>.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ARM
    }
}
