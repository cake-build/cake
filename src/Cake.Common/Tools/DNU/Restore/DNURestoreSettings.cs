// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DNU.Restore
{
    /// <summary>
    /// Contains settings used by <see cref="DNURestorer" />.
    /// </summary>
    public class DNURestoreSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a list of packages sources to use for this command.
        /// </summary>
        public ICollection<string> Sources { get; set; }

        /// <summary>
        /// Gets or sets a list of packages sources to use as a fallback.
        /// </summary>
        public ICollection<string> FallbackSources { get; set; }

        /// <summary>
        /// Gets or sets the HTTP proxy to use when retrieving packages.
        /// </summary>
        public string Proxy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not use local cache.
        /// </summary>
        public bool NoCache { get; set; }

        /// <summary>
        /// Gets or sets the path to restore packages.
        /// </summary>
        public DirectoryPath Packages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore failed remote sources if there are local packages meeting version requirements.
        /// </summary>
        public bool IgnoreFailedSources { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not show output such as HTTP request/cache information.
        /// </summary>
        public bool Quiet { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to restores in parallel when more than one project.json is discovered.
        /// </summary>
        public bool Parallel { get; set; }

        /// <summary>
        /// Gets or sets to creates dependencies file with locked property. Overwrites file if it exists.
        /// </summary>
        public DNULocked? Locked { get; set; }

        /// <summary>
        /// Gets or sets a list of runtime identifiers to restore for.
        /// </summary>
        public ICollection<string> Runtimes { get; set; }
    }
}
