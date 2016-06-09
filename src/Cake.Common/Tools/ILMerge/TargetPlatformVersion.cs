// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Diagnostics.CodeAnalysis;

namespace Cake.Common.Tools.ILMerge
{
    /// <summary>
    /// Represents the .NET Framework for the target assembly
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum TargetPlatformVersion
    {
        /// <summary>
        /// NET Framework v1
        /// </summary>
        v1,

        /// <summary>
        /// NET Framework v1.1
        /// </summary>
        v11,

        /// <summary>
        /// NET Framework v2
        /// </summary>
        v2,

        /// <summary>
        /// NET Framework v4
        /// </summary>
        v4
    }
}
