// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.GitReleaseManager
{
    /// <summary>
    /// Contains settings used by <see cref="GitReleaseManagerSettings"/>.
    /// </summary>
    public class GitReleaseManagerSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the path on which GitReleaseManager should be executed.
        /// </summary>
        public DirectoryPath TargetDirectory { get; set; }

        /// <summary>
        /// Gets or sets the path to the GitReleaseManager log file.
        /// </summary>
        public FilePath LogFilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to use debug level logging.
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to use verbose level logging.
        /// </summary>
        public bool Verbose { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to show the GitReleaseManager logo during execution.
        /// </summary>
        public bool NoLogo { get; set; }
    }
}