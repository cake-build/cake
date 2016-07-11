// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.IO.NuGet
{
    /// <summary>
    /// Represents a NuGet path resolver.
    /// </summary>
    public interface INuGetToolResolver
    {
        /// <summary>
        /// Resolves the path to nuget.exe.
        /// </summary>
        /// <returns>The path to nuget.exe.</returns>
        FilePath ResolvePath();
    }
}
