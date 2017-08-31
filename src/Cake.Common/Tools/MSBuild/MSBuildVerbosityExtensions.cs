using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Contains functionality related to MSBuild verbosity.
    /// </summary>
    public static class MSBuildVerbosityExtensions
    {
        /// <summary>
        /// Gets the MSBuild verbosity from <see cref="Verbosity"/>.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <returns>MSBuild verbosity string.</returns>
        public static string GetMSBuildVerbosityName(this Verbosity verbosity)
        {
            switch (verbosity)
            {
                case Verbosity.Quiet:
                    return "quiet";
                case Verbosity.Minimal:
                    return "minimal";
                case Verbosity.Normal:
                    return "normal";
                case Verbosity.Verbose:
                    return "detailed";
                case Verbosity.Diagnostic:
                    return "diagnostic";
                default:
                    throw new CakeException("Encountered unknown MSBuild build log verbosity.");
            }
        }
    }
}
