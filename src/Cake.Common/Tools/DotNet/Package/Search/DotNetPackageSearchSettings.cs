// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.Package.Search
{
    /// <summary>
    /// Represents the settings for searching .NET packages.
    /// </summary>
    public class DotNetPackageSearchSettings : DotNetSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to allow prerelease packages to be shown.
        /// </summary>
        public bool Prerelease { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an exact match is required. Causes <see cref="Take"/> and <see cref="Skip"/> options to be ignored.
        /// </summary>
        public bool ExactMatch { get; set; }

        /// <summary>
        /// Gets or sets the NuGet configuration file. If specified, only the settings from this file will be used. If not specified, the hierarchy of configuration files from the current directory will be used.
        /// </summary>
        /// <seealso href="https://docs.microsoft.com/nuget/consume-packages/configuring-nuget-behavior"/>
        public FilePath ConfigFile { get; set; }

        /// <summary>
        /// Gets or sets a list of package sources to search.
        /// </summary>
        public ICollection<string> Sources { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the number of results to return.
        /// </summary>
        public int? Take { get; set; }

        /// <summary>
        /// Gets or sets the number of results to skip, to allow pagination.
        /// </summary>
        public int? Skip { get; set; }
    }
}
