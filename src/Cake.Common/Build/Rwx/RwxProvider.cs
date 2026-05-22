// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.Rwx.Commands;
using Cake.Common.Build.Rwx.Data;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.Rwx
{
    /// <summary>
    /// Responsible for communicating with RWX.
    /// </summary>
    public sealed class RwxProvider : IRwxProvider
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="RwxProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="fileSystem">The file system.</param>
        public RwxProvider(ICakeEnvironment environment, IFileSystem fileSystem)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            ArgumentNullException.ThrowIfNull(fileSystem);
            Environment = new RwxEnvironmentInfo(environment);
            Commands = new RwxCommands(environment, fileSystem, Environment);
        }

        /// <inheritdoc/>
        public bool IsRunningOnRwx => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("RWX"));

        /// <inheritdoc/>
        public RwxEnvironmentInfo Environment { get; }

        /// <inheritdoc/>
        public RwxCommands Commands { get; }
    }
}
