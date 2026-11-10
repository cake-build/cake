// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Cake.Common.Tools.DotNet.NuGet.Why;

/// <summary>
/// A per-framework dependency graph from <c>dotnet nuget why</c>.
/// </summary>
public sealed class DotNetNuGetWhyGraph
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DotNetNuGetWhyGraph"/> class.
    /// </summary>
    /// <param name="framework">The target framework, optionally including a runtime identifier.</param>
    /// <param name="dependencies">The root package dependencies.</param>
    public DotNetNuGetWhyGraph(string framework, IReadOnlyList<DotNetNuGetWhyPackage> dependencies)
    {
        Framework = framework;
        Dependencies = dependencies ?? [];
    }

    /// <summary>
    /// Gets the target framework, such as <c>net10.0</c> or <c>net9.0/win-x64</c>.
    /// </summary>
    public string Framework { get; }

    /// <summary>
    /// Gets the root package dependencies for this framework.
    /// </summary>
    public IReadOnlyList<DotNetNuGetWhyPackage> Dependencies { get; }
}
