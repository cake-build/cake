// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.NuGet.Pack
{
    /// <summary>
    /// Specifies the package's content to identify the exact files that are included in the package.
    /// </summary>
    public class NuSpecContentFile
    {
        /// <summary>
        /// Gets or sets the location of the file or files to include.
        /// The wildcard character - <c>*</c> - is allowed.
        /// Using a double wildcard - <c>**</c> implies a recursive directory search.
        /// </summary>
        public string Include { get; set; }

        /// <summary>
        /// Gets or sets files or file patterns to exclude from <c>src</c> location.
        /// The wildcard character - <c>*</c> - is allowed.
        /// Using a double wildcard - <c>**</c> implies a recursive directory search.
        /// </summary>
        public string Exclude { get; set; }

        /// <summary>
        /// Gets or sets the build action to assign to the content item for MSBuild, e.g. Compile.
        /// Defaults to <c>Compile</c>.
        /// </summary>
        public string BuildAction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy content items to the build (or publish) output folder.
        /// Defaults to <c>false</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if content items should be copied to the output folder; otherwise, <c>false</c>.
        /// </value>
        public bool CopyToOutput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy content items to a single folder in the build output
        /// or to preserve the folder structure in the package. This flag only works when <c>copyToOutput</c>
        /// flag is set to true. Defaults to <c>false</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if content items should be copied to a single folder; otherwise, <c>false</c>.
        /// </value>
        public bool Flatten { get; set; }
    }
}
