// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.Rwx.Data
{
    /// <summary>
    /// Provides RWX runtime information for the current build, exposing the
    /// directories RWX uses for output values and artifacts. These are written to
    /// at task execution time and complement static <c>outputs.values</c> /
    /// <c>outputs.artifacts</c> declarations in the task YAML.
    /// </summary>
    public sealed class RwxRuntimeInfo : RwxInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RwxRuntimeInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public RwxRuntimeInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the directory in which output values can be written. Each file
        /// in this directory becomes an output value keyed by its filename.
        /// </summary>
        /// <value>
        /// The path resolved from the <c>RWX_VALUES</c> environment variable, or
        /// <c>null</c> if the variable is not set.
        /// </value>
        public DirectoryPath ValuesPath => GetEnvironmentDirectoryPath("RWX_VALUES");

        /// <summary>
        /// Gets the directory into which artifacts can be uploaded. Files copied
        /// into this directory are captured as artifacts of the current task.
        /// </summary>
        /// <value>
        /// The path resolved from the <c>RWX_ARTIFACTS</c> environment variable, or
        /// <c>null</c> if the variable is not set.
        /// </value>
        public DirectoryPath ArtifactsPath => GetEnvironmentDirectoryPath("RWX_ARTIFACTS");

        /// <summary>
        /// Gets the directory into which environment variables can be exported
        /// for downstream tasks. Each file in this directory becomes an
        /// environment variable in tasks that depend on the current one via
        /// <c>use</c>, keyed by filename.
        /// </summary>
        /// <value>
        /// The path resolved from the <c>RWX_ENV</c> environment variable, or
        /// <c>null</c> if the variable is not set.
        /// </value>
        public DirectoryPath EnvPath => GetEnvironmentDirectoryPath("RWX_ENV");

        /// <summary>
        /// Gets a value indicating whether the values, artifacts, and env
        /// directories are all exposed by the current RWX runtime.
        /// </summary>
        /// <value>
        /// <c>true</c> if <see cref="ValuesPath"/>, <see cref="ArtifactsPath"/>,
        /// and <see cref="EnvPath"/> are all set; otherwise, <c>false</c>.
        /// </value>
        public bool IsRuntimeAvailable => ValuesPath != null && ArtifactsPath != null && EnvPath != null;
    }
}
