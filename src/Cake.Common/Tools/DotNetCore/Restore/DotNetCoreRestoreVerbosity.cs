// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Tools.DotNetCore.Restore
{
    /// <summary>
    /// Contains the verbosity of logging to use. Used by <see cref="DotNetCoreRestoreSettings"/>.
    /// </summary>
    public enum DotNetCoreRestoreVerbosity
    {
        /// <summary>
        /// Error level.
        /// </summary>
        Error,

        /// <summary>
        /// Warning level.
        /// </summary>
        Warning,

        /// <summary>
        /// Information level.
        /// </summary>
        Information,

        /// <summary>
        /// Verbose level.
        /// </summary>
        Verbose
    }
}
