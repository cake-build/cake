// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.NuGet
{
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
        }

        public static class Paths
        {
            public const string Tools = "Paths_Tools";
        }
    }
}