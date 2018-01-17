// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        /// <summary>
        /// Gets the MSBuild <see cref="Verbosity"/> from string value.
        /// </summary>
        /// <param name="verbosity">The verbosity string value.</param>
        /// <returns>MSBuild <see cref="Verbosity"/> enumeration.</returns>
        /// <remarks>Valid values are 'quiet', 'minimal', 'normal', 'detailed' and 'diagnostic'.</remarks>
        public static Verbosity GetMSBuildVerbosity(this string verbosity)
        {
            switch (verbosity?.ToLower())
            {
                case "quiet":
                    return Verbosity.Quiet;
                case "minimal":
                    return Verbosity.Minimal;
                case "normal":
                    return Verbosity.Normal;
                case "detailed":
                    return Verbosity.Verbose;
                case "diagnostic":
                    return Verbosity.Diagnostic;
                default:
                    throw new CakeException($"Encountered unknown MSBuild build log verbosity '{verbosity}'. Valid values are 'quiet', 'minimal', 'normal', 'detailed' and 'diagnostic'.");
            }
        }
    }
}
