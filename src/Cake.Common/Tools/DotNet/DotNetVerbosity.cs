// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNetCore;

namespace Cake.Common.Tools.DotNet
{
    /// <summary>
    /// Contains the verbosity of logging to use.
    /// </summary>
    public sealed class DotNetVerbosity
    {
        /// <summary>
        /// Quiet level.
        /// </summary>
        public const DotNetCoreVerbosity Quiet = DotNetCoreVerbosity.Quiet;

        /// <summary>
        /// Minimal level.
        /// </summary>
        public const DotNetCoreVerbosity Minimal = DotNetCoreVerbosity.Minimal;

        /// <summary>
        /// Normal level.
        /// </summary>
        public const DotNetCoreVerbosity Normal = DotNetCoreVerbosity.Normal;

        /// <summary>
        /// Detailed level.
        /// </summary>
        public const DotNetCoreVerbosity Detailed = DotNetCoreVerbosity.Detailed;

        /// <summary>
        /// Diagnostic level.
        /// </summary>
        public const DotNetCoreVerbosity Diagnostic = DotNetCoreVerbosity.Diagnostic;
    }
}
