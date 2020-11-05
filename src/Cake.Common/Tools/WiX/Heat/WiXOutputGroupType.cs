// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.WiX.Heat
{
    /// <summary>
    /// The Output Group of Visual Studio project.
    /// </summary>
    public enum WiXOutputGroupType
    {
        /// <summary>
        /// OutputGroup: <c>Binaries</c>
        /// </summary>
        Binaries = 0,

        /// <summary>
        /// OutputGroup: <c>Symbols</c>
        /// </summary>
        Symbols = 1,

        /// <summary>
        /// OutputGroup: <c>Documents</c>
        /// </summary>
        Documents = 2,

        /// <summary>
        /// OutputGroup: <c>Satellites</c>
        /// </summary>
        Satellites = 3,

        /// <summary>
        /// OutputGroup: <c>Sources</c>
        /// </summary>
        Sources = 4,

        /// <summary>
        /// OutputGroup: <c>Content</c>
        /// </summary>
        Content = 5
    }
}