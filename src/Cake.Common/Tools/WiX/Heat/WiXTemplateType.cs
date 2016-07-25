// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.WiX.Heat
{
    /// <summary>
    /// Template type to use for harvesting.
    /// </summary>
    public enum WiXTemplateType
    {
        /// <summary>
        /// TemplateType: <c>Fragment</c>
        /// </summary>
        Fragment = 0,

        /// <summary>
        /// TemplateType: <c>Module</c>
        /// </summary>
        Module = 1,

        /// <summary>
        /// TemplateType: <c>Product</c>
        /// </summary>
        Product = 2
    }
}