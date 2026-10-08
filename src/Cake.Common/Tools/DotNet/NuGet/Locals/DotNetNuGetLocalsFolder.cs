// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNet.NuGet.Locals;

/// <summary>
/// The NuGet local folder used by <c>dotnet nuget locals</c>.
/// </summary>
public enum DotNetNuGetLocalsFolder
{
    /// <summary>
    /// All local NuGet folders.
    /// </summary>
    All,

    /// <summary>
    /// The HTTP request cache.
    /// </summary>
    HttpCache,

    /// <summary>
    /// The global packages folder.
    /// </summary>
    GlobalPackages,

    /// <summary>
    /// The temporary cache.
    /// </summary>
    Temp,

    /// <summary>
    /// The plugins cache.
    /// </summary>
    PluginsCache
}
