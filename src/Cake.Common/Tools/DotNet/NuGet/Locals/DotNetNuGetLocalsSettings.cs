// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNet.NuGet.Locals;

/// <summary>
/// Contains settings used by <see cref="DotNetNuGetLocalser" />.
/// </summary>
public sealed class DotNetNuGetLocalsSettings : DotNetSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether to force command-line output in English.
    /// </summary>
    public bool ForceEnglishOutput { get; set; }
}
