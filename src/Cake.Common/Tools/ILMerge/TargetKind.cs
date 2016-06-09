// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Tools.ILMerge
{
    /// <summary>
    /// Represents an ILMerge target.
    /// </summary>
    public enum TargetKind
    {
        /// <summary>
        /// TargetKind: <c>Default</c>
        /// </summary>
        Default,

        /// <summary>
        /// TargetKind: <c>Dynamic Link Library</c>
        /// </summary>
        Dll,

        /// <summary>
        /// TargetKind: <c>Executable</c>
        /// </summary>
        Exe,

        /// <summary>
        /// TargetKind: <c>Windows executable</c>
        /// </summary>
        WinExe
    }
}
