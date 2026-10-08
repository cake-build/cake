// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Cake.Common.Tools.DotNet.NuGet.Why;

/// <summary>
/// Contains settings used by <see cref="DotNetNuGetWhyer" />.
/// </summary>
public sealed class DotNetNuGetWhySettings : DotNetSettings
{
    /// <summary>
    /// Gets or sets the project or solution file to operate on.
    /// If not specified, the command searches the current directory for one.
    /// </summary>
    public string Project { get; set; }

    /// <summary>
    /// Gets or sets the target frameworks to show the dependency graph for.
    /// </summary>
    public ICollection<string> Frameworks { get; set; } = new List<string>();
}
