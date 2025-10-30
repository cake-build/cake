// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Cake.Common.Build.WoodpeckerCI.Data;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.WoodpeckerCI.Commands
{
    /// <summary>
    /// Provides WoodpeckerCI commands for a current build.
    /// </summary>
    public sealed class WoodpeckerCICommands
    {
        private readonly ICakeEnvironment _environment;
        private readonly IFileSystem _fileSystem;
        private readonly WoodpeckerCIEnvironmentInfo _woodpeckerEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="WoodpeckerCICommands"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="woodpeckerEnvironment">The WoodpeckerCI environment.</param>
        public WoodpeckerCICommands(
            ICakeEnvironment environment,
            IFileSystem fileSystem,
            WoodpeckerCIEnvironmentInfo woodpeckerEnvironment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _woodpeckerEnvironment = woodpeckerEnvironment ?? throw new ArgumentNullException(nameof(woodpeckerEnvironment));
        }

        /// <summary>
        /// Sets an environment variable that will be available to subsequent steps.
        /// </summary>
        /// <param name="name">The environment variable name.</param>
        /// <param name="value">The environment variable value.</param>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     WoodpeckerCI.Commands.SetEnvironmentVariable("MY_VAR", "my_value");
        /// }
        /// </code>
        /// </example>
        public void SetEnvironmentVariable(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Environment variable name cannot be null or empty.", nameof(name));
            }

            // Write to the WoodpeckerCI environment file for persistence between steps
            var envFile = _woodpeckerEnvironment.Workspace?.CombineWithFilePath(".woodpecker/env");
            if (envFile != null)
            {
                var file = _fileSystem.GetFile(envFile);
                using var stream = file.Open(FileMode.Append, FileAccess.Write, FileShare.None);
                using var writer = new StreamWriter(stream);
                writer.Write(name);
                writer.Write('=');
                writer.Write(value);
                writer.Write('\n');
            }
        }

        /// <summary>
        /// Gets an environment variable that was set by a previous step.
        /// </summary>
        /// <param name="name">The environment variable name.</param>
        /// <returns>The environment variable value, or null if not found.</returns>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     var value = WoodpeckerCI.Commands.GetEnvironmentVariable("MY_VAR");
        ///     Information("MY_VAR = {0}", value);
        /// }
        /// </code>
        /// </example>
        public string GetEnvironmentVariable(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Environment variable name cannot be null or empty.", nameof(name));
            }

            return _environment.GetEnvironmentVariable(name);
        }
    }
}
