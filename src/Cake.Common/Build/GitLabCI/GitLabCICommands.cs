// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Cake.Core.IO;

namespace Cake.Common.Build.GitLabCI
{
    /// <summary>
    /// Provides GitLab CI commands for a current build.
    /// </summary>
    public sealed class GitLabCICommands
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitLabCICommands"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        public GitLabCICommands(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        /// <summary>
        /// Creates or updates an environment variable for any steps running next in a job.
        /// </summary>
        /// <param name="envPath">Path to env file.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The Value.</param>
        public void SetEnvironmentVariable(FilePath envPath, string key, string value)
        {
            if (envPath is null)
            {
                throw new ArgumentNullException(nameof(envPath));
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var file = _fileSystem.GetFile(envPath);
            using var stream = file.Open(FileMode.Append, FileAccess.Write, FileShare.None);
            using var writer = new StreamWriter(stream);
            writer.Write(key);
            writer.Write('=');
            writer.WriteLine(value);
        }
    }
}