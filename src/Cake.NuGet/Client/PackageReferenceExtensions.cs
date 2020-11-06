// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Cake.Core.Configuration;
using Cake.Core.Packaging;

namespace Cake.NuGet
{
    internal static class PackageReferenceExtensions
    {
        private const string LoadDependenciesKey = "loaddependencies";

        public static bool ShouldLoadDependencies(this PackageReference packageReference, ICakeConfiguration config)
        {
            bool loadDependencies;
            if (packageReference.Parameters.ContainsKey(LoadDependenciesKey))
            {
                bool.TryParse(packageReference.Parameters[LoadDependenciesKey].FirstOrDefault() ?? bool.TrueString, out loadDependencies);
            }
            else
            {
                bool.TryParse(config.GetValue(Constants.NuGet.LoadDependencies) ?? bool.FalseString, out loadDependencies);
            }
            return loadDependencies;
        }

        public static bool IsPrerelease(this PackageReference packageReference)
        {
            var isPreRelease = false;
            if (packageReference.Parameters.ContainsKey("prerelease"))
            {
                bool.TryParse(packageReference.Parameters["prerelease"].FirstOrDefault() ?? bool.TrueString, out isPreRelease);
            }
            return isPreRelease;
        }
    }
}