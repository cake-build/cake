// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DNU.Pack
{
    /// <summary>
    /// Contains settings used by <see cref="DNUPacker" />.
    /// </summary>
    public class DNUPackSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a list of frameworks to use.
        /// </summary>
        public ICollection<string> Frameworks { get; set; }

        /// <summary>
        /// Gets or sets a list of configurations to use.
        /// </summary>
        public ICollection<string> Configurations { get; set; }

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not show output such as dependencies in use.
        /// </summary>
        public bool Quiet { get; set; }
    }
}
