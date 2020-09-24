// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.WiX.Heat
{
    /// <summary>
    /// Type of elements to generate.
    /// </summary>
    public enum WiXGenerateType
    {
        /// <summary>
        /// Generates components
        /// </summary>
        Components,

        /// <summary>
        /// Generates a container
        /// </summary>
        Container,

        /// <summary>
        /// Generates a payload group
        /// </summary>
        PayloadGroup,

        /// <summary>
        /// Generates a layout
        /// </summary>
        Layout
    }
}