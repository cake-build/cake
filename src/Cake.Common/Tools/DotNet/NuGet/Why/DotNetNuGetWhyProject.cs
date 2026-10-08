// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Cake.Common.Tools.DotNet.NuGet.Why;

/// <summary>
/// A project-level result from <c>dotnet nuget why</c>.
/// </summary>
public sealed class DotNetNuGetWhyProject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DotNetNuGetWhyProject"/> class.
    /// </summary>
    /// <param name="name">The project name.</param>
    /// <param name="package">The queried package identifier.</param>
    /// <param name="graphs">The per-framework dependency graphs.</param>
    public DotNetNuGetWhyProject(string name, string package, IReadOnlyList<DotNetNuGetWhyGraph> graphs)
    {
        Name = name;
        Package = package;
        Graphs = graphs ?? [];
    }

    /// <summary>
    /// Gets the project name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the queried package identifier.
    /// </summary>
    public string Package { get; }

    /// <summary>
    /// Gets the per-framework dependency graphs.
    /// </summary>
    public IReadOnlyList<DotNetNuGetWhyGraph> Graphs { get; }
}
