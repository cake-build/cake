// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.NuGet;

internal static class Constants
{
    public static class NuGet
    {
        /// <summary>
        /// The config key name for overriding the default NuGet package source.
        /// </summary>
        public const string Source = "NuGet_Source";

        /// <summary>
        /// The config key name for using the in process client for installing packages.
        /// </summary>
        public const string UseInProcessClient = "NuGet_UseInProcessClient";

        /// <summary>
        /// The config key name for enabling loading of NuGet package dependencies.
        /// </summary>
        public const string LoadDependencies = "NuGet_LoadDependencies";

        /// <summary>
        /// The config key name for overriding the default NuGet config file.
        /// </summary>
        public const string ConfigFile = "NuGet_ConfigFile";

        /// <summary>
        /// The config key name for non-interactive mode.
        /// </summary>
        public const string NonInteractive = "NuGet_NonInteractive";

        /// <summary>
        /// When <c>false</c> (default), #addin/#module asset selection uses the embedded
        /// portable RID graph (win-x64, linux-x64, osx-arm64, …). Version-specific folders
        /// such as win10-x64 or ubuntu.16.04-x64 are not compatible with the host RID.
        /// When <c>true</c>, if a Microsoft.NETCore.Platforms-style runtime.json sits next
        /// to Cake.NuGet.dll it is used instead, so those version-specific RIDs can still
        /// match. If the sidecar file is missing, the portable graph is used.
        /// </summary>
        public const string UseLegacyRidGraph = "NuGet_UseLegacyRidGraph";
    }

    public static class Paths
    {
        public const string Tools = "Paths_Tools";
    }
}
