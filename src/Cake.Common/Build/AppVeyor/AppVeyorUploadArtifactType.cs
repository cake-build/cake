// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Build.AppVeyor
{
    /// <summary>
    /// Provides the known artifact upload types for the AppVeyor.
    /// </summary>
    public enum AppVeyorUploadArtifactType
    {
        /// <summary>
        /// Automatically deploy artifact type
        /// </summary>
        Auto,

        /// <summary>
        /// The artifact is a web deploy package (.zip)
        /// </summary>
        WebDeployPackage,

        /// <summary>
        /// The artifact is a NuGet package (.nupkg)
        /// </summary>
        NuGetPackage
    }
}