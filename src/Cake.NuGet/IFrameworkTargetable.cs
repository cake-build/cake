// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.Runtime.Versioning;

namespace Cake.NuGet
{
    /// <summary>
    /// Represents an item that may support specific .Net framework versions
    /// </summary>
    public interface IFrameworkTargetable
    {
        /// <summary>
        /// Gets the frameworks supported by this item.
        /// </summary>
        /// <value>
        /// The supported frameworks.
        /// </value>
        IEnumerable<FrameworkName> SupportedFrameworks { get; }
    }
}
