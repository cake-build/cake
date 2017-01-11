// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;

namespace Cake.Core.Reflection
{
    internal sealed class AssemblyVerifier
    {
        private readonly ICakeLog _log;
        private readonly bool _skipVerification;

        public AssemblyVerifier(ICakeConfiguration configuration, ICakeLog log)
        {
            _log = log;
            var skip = configuration.GetValue(Constants.Settings.SkipVerification);
            _skipVerification = skip != null && skip.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        public void Verify(Assembly assembly)
        {
            if (_skipVerification)
            {
                _log.Debug("Skipping verification of assembly '{0}'.", assembly.FullName);
                return;
            }

            // Verify that the assembly is valid.
            // We're still pre 1.0, so there are breaking changes from time to time.
            // We can refuse to load assemblies that reference too old version of Cake.
            var references = assembly.GetReferencedAssemblies();
            foreach (var reference in references)
            {
                if (reference.Name.Equals("Cake.Core") && reference.Version < Constants.LatestBreakingChange)
                {
                    // The assembly is referencing a version of Cake that contains breaking changes.
                    throw new CakeException($"The assembly '{assembly.FullName}' is referencing an older version of Cake.Core ({reference.Version.ToString(3)}). " +
                        $"This assembly need to reference at least Cake.Core version {Constants.LatestBreakingChange.ToString(3)}. " +
                        $"Another option is to downgrade Cake to an earlier version.");
                }
            }
        }
    }
}
