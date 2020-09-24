// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    /// Represents the level of detail at which NUnit should set internal tracing.
    /// </summary>
    public enum NUnitInternalTraceLevel
    {
        /// <summary>
        /// Do not display any trace messages
        /// </summary>
        Off = 0,

        /// <summary>
        /// Display Error messages only
        /// </summary>
        Error = 1,

        /// <summary>
        /// Display Warning level and higher messages
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Display informational and higher messages
        /// </summary>
        Info = 3,

        /// <summary>
        /// Display debug messages and higher - i.e. all messages
        /// </summary>
        Debug = 4,

        /// <summary>
        /// Display debug messages and higher - i.e. all messages
        /// </summary>
        Verbose = Debug
    }
}
