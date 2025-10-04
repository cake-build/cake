// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.WoodpeckerCI.Commands;
using Cake.Common.Build.WoodpeckerCI.Data;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.WoodpeckerCI
{
    /// <summary>
    /// Responsible for communicating with WoodpeckerCI.
    /// </summary>
    public sealed class WoodpeckerCIProvider : IWoodpeckerCIProvider
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="WoodpeckerCIProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="fileSystem">The file system.</param>
        public WoodpeckerCIProvider(ICakeEnvironment environment, IFileSystem fileSystem)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            Environment = new WoodpeckerCIEnvironmentInfo(environment);
            Commands = new WoodpeckerCICommands(environment, fileSystem, Environment);
        }

        /// <inheritdoc/>
        public bool IsRunningOnWoodpeckerCI => !string.IsNullOrWhiteSpace(Environment.CI) &&
                                              Environment.CI.Equals("woodpecker", StringComparison.OrdinalIgnoreCase);

        /// <inheritdoc/>
        public WoodpeckerCIEnvironmentInfo Environment { get; }

        /// <inheritdoc/>
        public WoodpeckerCICommands Commands { get; }
    }
}
