// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.NuGet.Locals;

/// <summary>
/// The parsed result of <c>dotnet nuget locals --list</c>.
/// </summary>
/// <param name="HttpCache">The HTTP cache directory, if listed.</param>
/// <param name="GlobalPackages">The global packages directory, if listed.</param>
/// <param name="Temp">The temporary cache directory, if listed.</param>
/// <param name="PluginsCache">The plugins cache directory, if listed.</param>
public sealed record DotNetNuGetLocalsListResult(
    DirectoryPath HttpCache,
    DirectoryPath GlobalPackages,
    DirectoryPath Temp,
    DirectoryPath PluginsCache);
