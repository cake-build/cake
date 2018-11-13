// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.VSWhere.Latest
{
    /// <summary>
    /// Contains settings used by <see cref="VSWhereLatest"/>.
    /// </summary>
    public sealed class VSWhereLatestSettings : VSWhereSettings
    {
        /// <summary>
        /// Gets or sets the products to find. Defaults to Community, Professional, and Enterprise. Specify "*" by itself to search all product instances installed.
        /// </summary>
        public string Products { get; set; }
    }
}
