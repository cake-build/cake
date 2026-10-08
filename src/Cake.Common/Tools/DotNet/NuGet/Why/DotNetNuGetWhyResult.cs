// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Cake.Common.Tools.DotNet.NuGet.Why;

/// <summary>
/// The parsed result of <c>dotnet nuget why</c>.
/// </summary>
public sealed class DotNetNuGetWhyResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DotNetNuGetWhyResult"/> class.
    /// </summary>
    /// <param name="projects">The projects included in the output.</param>
    public DotNetNuGetWhyResult(IReadOnlyList<DotNetNuGetWhyProject> projects)
    {
        Projects = projects ?? [];
    }

    /// <summary>
    /// Gets the projects included in the output.
    /// </summary>
    public IReadOnlyList<DotNetNuGetWhyProject> Projects { get; }
}
