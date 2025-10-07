// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Infrastructure;

namespace Cake.Features.Building
{
    /// <summary>
    /// Settings for the build feature.
    /// </summary>
    public sealed class BuildFeatureSettings : IScriptHostSettings
    {
        /// <summary>
        /// Gets the build host kind.
        /// </summary>
        public BuildHostKind BuildHostKind { get; }

        /// <summary>
        /// Gets or sets the script file path.
        /// </summary>
        public FilePath Script { get; set; }

        /// <summary>
        /// Gets or sets the verbosity level.
        /// </summary>
        public Verbosity Verbosity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in debug mode.
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run exclusively.
        /// </summary>
        public bool Exclusive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip bootstrapping.
        /// </summary>
        public bool NoBootstrapping { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildFeatureSettings"/> class.
        /// </summary>
        /// <param name="buildHostKind">The build host kind.</param>
        public BuildFeatureSettings(BuildHostKind buildHostKind)
        {
            BuildHostKind = buildHostKind;
        }
    }
}
