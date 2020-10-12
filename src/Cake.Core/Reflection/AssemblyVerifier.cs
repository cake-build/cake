// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;

namespace Cake.Core.Reflection
{
    /// <summary>
    /// Responsible for verifying assemblies.
    /// </summary>
    public sealed class AssemblyVerifier : IAssemblyVerifier
    {
        private readonly ICakeLog _log;
        private readonly bool _skipVerification;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyVerifier"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="log">The log.</param>
        public AssemblyVerifier(ICakeConfiguration configuration, ICakeLog log)
        {
            _log = log;
            var skip = configuration.GetValue(Constants.Settings.SkipVerification);
            _skipVerification = skip != null && skip.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Verifies an assembly.
        /// </summary>
        /// <param name="assembly">The target assembly.</param>
        public void Verify(Assembly assembly)
        {
            _log.Debug(_skipVerification ? "Skipping verification of assembly '{0}'." : "Verifying assembly '{0}'.", assembly.FullName);

            // Verify that the assembly is valid.
            // We're still pre 1.0, so there are breaking changes from time to time.
            // We can refuse to load assemblies that reference too old version of Cake.
            var references = assembly.GetReferencedAssemblies();
            foreach (var reference in references)
            {
                if (reference.Name.Equals("Cake.Core"))
                {
                    if (reference.Version < Constants.LatestBreakingChange)
                    {
                        // The assembly is referencing a version of Cake that contains breaking changes.
                        const string message = "The assembly '{0}' \r\n" +
                                               "is referencing an older version of Cake.Core ({1}). \r\n" +
                                               "This assembly must reference at least Cake.Core version {2}. \r\n" +
                                               "Another option is to downgrade Cake to an earlier version. \r\n" +
                                               "It's not recommended, but you can explicitly opt out of assembly verification \r\n" +
                                               "by configuring the Skip Verification setting to true\r\n" +
                                               "(i.e. command line parameter \"--settings_skipverification=true\", \r\n" +
                                               "environment variable \"CAKE_SETTINGS_SKIPVERIFICATION=true\", \r\n" +
                                               "read more about configuration at https://cakebuild.net/docs/running-builds/configuration/)";

                        var args = new object[]
                        {
                            assembly.FullName,
                            reference.Version.ToString(3),
                            Constants.LatestBreakingChange.ToString(3)
                        };

                        if (_skipVerification)
                        {
                            _log.Debug(
                                message,
                                args);
                            return;
                        }

                        throw new CakeException(string.Format(message, args));
                    }

                    if (reference.Version < Constants.LatestPotentialBreakingChange)
                    {
                        // The assembly is referencing a version of Cake that might contain breaking changes.
                        const string message = "The assembly '{0}' \r\n" +
                                               "is referencing an older version of Cake.Core ({1}). \r\n" +
                                               "For best compatibility it should target Cake.Core version {2}.";
                        _log.Warning(
                            _skipVerification ? Verbosity.Verbose : Verbosity.Minimal,
                            message,
                            assembly.FullName,
                            reference.Version.ToString(3),
                            Constants.LatestPotentialBreakingChange.ToString(3));
                    }
                }
            }
        }
    }
}
