// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Tools.Chocolatey.Pack
{
    /// <summary>
    /// Represents a Chocolatey nuspec file
    /// </summary>
    public sealed class ChocolateyNuSpecContent
    {
        /// <summary>
        /// Gets or sets the location of the file or files to include.
        /// The path is relative to the NuSpec file unless an absolute path is specified.
        /// The wildcard character - <c>*</c> - is allowed.
        /// Using a double wildcard - <c>**</c> implies a recursive directory search.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the relative path to the directory within the package where the source files will be placed.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the file or files to exclude.
        /// This is usually combined with a wildcard value in the <c>src</c> attribute.
        /// The <c>exclude</c> attribute can contain a semi-colon delimited list of files or a file pattern.
        /// Using a double wildcard - <c>**</c> - implies a recursive exclude pattern.
        /// </summary>
        public string Exclude { get; set; }
    }
}
