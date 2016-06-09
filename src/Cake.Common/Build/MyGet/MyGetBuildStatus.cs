// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Build.MyGet
{
    /// <summary>
    /// Provides the known values for MyGet Build Status
    /// </summary>
    public enum MyGetBuildStatus
    {
        /// <summary>
        /// Failure Status
        /// </summary>
        Failure,

        /// <summary>
        /// Error Status
        /// </summary>
        Error,

        /// <summary>
        /// Warning Status
        /// </summary>
        Warning,

        /// <summary>
        /// Normal Status
        /// </summary>
        Normal
    }
}
