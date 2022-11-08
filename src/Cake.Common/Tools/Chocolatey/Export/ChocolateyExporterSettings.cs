// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Tools.Chocolatey.Export
{
    /// <summary>
    /// Contains settings used by <see cref="ChocolateyExporter"/>.
    /// </summary>
    public sealed class ChocolateyExportSettings : ChocolateySettings
    {
        /// <summary>
        /// Gets or sets the path to the file that will be generated.
        /// </summary>
        /// <value>The path to the file that is generated.</value>
        public FilePath OutputFilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include version numbers for installed packages.
        /// </summary>
        /// <value>The include version numbers flag.</value>
        public bool IncludeVersionNumbers { get; set; }
    }
}