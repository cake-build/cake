// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    internal static class Constants
    {
        public static readonly Version LatestBreakingChange = new Version(0, 26, 0);
        public static readonly Version LatestPotentialBreakingChange = new Version(0, 33, 0);

        public static class Settings
        {
            public const string SkipVerification = "Settings_SkipVerification";
            public const string SkipPackageVersionCheck = "Settings_SkipPackageVersionCheck";
            public const string NoMonoCoersion = "Settings_NoMonoCoersion";
            public const string ShowProcessCommandLine = "Settings_ShowProcessCommandLine";
        }

        public static class Cache
        {
            public const string Enabled = "Cache_Enabled";
            public const string ForceRecompile = "Cache_Force_Recompile";
        }

        public static class Paths
        {
            public const string Tools = "Paths_Tools";
            public const string Addins = "Paths_Addins";
            public const string Modules = "Paths_Modules";
        }
    }
}
