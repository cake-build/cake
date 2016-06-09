// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Tools.NuGet
{
    /// <summary>
    /// Represents NuGet verbosity.
    /// </summary>
    public enum NuGetVerbosity
    {
        /// <summary>
        /// Verbosity: <c>Normal</c>
        /// </summary>
        Normal = 1,

        /// <summary>
        /// Verbosity: <c>Quiet</c>
        /// </summary>
        Quiet = 2,

        /// <summary>
        /// Verbosity: <c>Detailed</c>
        /// </summary>
        Detailed = 4
    }
}
