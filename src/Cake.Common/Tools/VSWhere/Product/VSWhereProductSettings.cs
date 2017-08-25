// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.VSWhere.Product
{
    /// <summary>
    /// Contains settings used by <see cref="VSWhereProduct"/>.
    /// </summary>
    public sealed class VSWhereProductSettings : VSWhereSettings
    {
        /// <summary>
        /// Gets or sets the products to find. Defaults to Community, Professional, and Enterprise. Specify "*" by itself to search all product instances installed.
        /// </summary>
        internal string Products { get; set; }
    }
}
