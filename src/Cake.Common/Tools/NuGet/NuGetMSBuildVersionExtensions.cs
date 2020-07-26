using System;

namespace Cake.Common.Tools.NuGet
{
    /// <summary>
    /// Contains extension methods for <see cref="NuGetMSBuildVersion"/>.
    /// </summary>
    public static class NuGetMSBuildVersionExtensions
    {
        /// <summary>
        /// Gets the string with MSBuild version.
        /// </summary>
        /// <param name="nuGetMSBuildVersion">The NuGet MSBuild version.</param>
        /// <returns>The string with MSBuild version.</returns>
        public static string GetNuGetMSBuildVersionString(this NuGetMSBuildVersion nuGetMSBuildVersion)
        {
            switch (nuGetMSBuildVersion)
            {
                case NuGetMSBuildVersion.MSBuild4:
                    return "4";

                case NuGetMSBuildVersion.MSBuild12:
                    return "12";

                case NuGetMSBuildVersion.MSBuild14:
                    return "14";

                case NuGetMSBuildVersion.MSBuild15_1:
                    return "15.1";

                case NuGetMSBuildVersion.MSBuild15_3:
                    return "15.3";

                case NuGetMSBuildVersion.MSBuild15_4:
                    return "15.4";

                case NuGetMSBuildVersion.MSBuild15_5:
                    return "15.5";

                case NuGetMSBuildVersion.MSBuild15_6:
                    return "15.6";

                case NuGetMSBuildVersion.MSBuild15_7:
                    return "15.7";

                case NuGetMSBuildVersion.MSBuild15_8:
                    return "15.8";

                case NuGetMSBuildVersion.MSBuild15_9:
                    return "15.9";

                case NuGetMSBuildVersion.MSBuild16_0:
                    return "16.0";

                default:
                    throw new ArgumentOutOfRangeException(nameof(nuGetMSBuildVersion), nuGetMSBuildVersion, "Invalid nuGetMSBuildVersion supplied.");
            }
        }
    }
}
