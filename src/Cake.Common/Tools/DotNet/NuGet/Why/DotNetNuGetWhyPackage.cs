// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Cake.Common.Tools.DotNet.NuGet.Why;

/// <summary>
/// A package node in a <c>dotnet nuget why</c> dependency graph.
/// </summary>
public sealed class DotNetNuGetWhyPackage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DotNetNuGetWhyPackage"/> class.
    /// </summary>
    /// <param name="id">The package identifier.</param>
    /// <param name="version">The resolved package version.</param>
    /// <param name="versionConstraint">The version constraint, if present.</param>
    /// <param name="dependencies">The child package dependencies.</param>
    public DotNetNuGetWhyPackage(
        string id,
        string version,
        string versionConstraint,
        IReadOnlyList<DotNetNuGetWhyPackage> dependencies)
    {
        Id = id;
        Version = version;
        VersionConstraint = versionConstraint;
        Dependencies = dependencies ?? [];
    }

    /// <summary>
    /// Gets the package identifier.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the resolved package version.
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// Gets the version constraint printed by the CLI, such as <c>&gt;= 3.2.2</c> or <c>= 3.2.2</c>.
    /// </summary>
    public string VersionConstraint { get; }

    /// <summary>
    /// Gets the child package dependencies.
    /// </summary>
    public IReadOnlyList<DotNetNuGetWhyPackage> Dependencies { get; }
}
