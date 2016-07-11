// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Tools.VSTest
{
    /// <summary>
    /// Target .NET Framework version to be used for test execution.
    /// </summary>
    public enum VSTestFrameworkVersion
    {
        /// <summary>
        /// Use default .NET Framework version.
        /// </summary>
        Default,

        /// <summary>
        /// .NET Framework version: <c>3.5</c>.
        /// </summary>
        NET35,

        /// <summary>
        /// .NET Framework version: <c>4.0</c>.
        /// </summary>
        NET40,

        /// <summary>
        /// .NET Framework version: <c>4.5</c>.
        /// </summary>
        NET45
    }
}
