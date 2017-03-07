// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.Chocolatey.New
{
    /// <summary>
    /// Contains settings used by <see cref="ChocolateyScaffolder"/>.
    /// </summary>
    public sealed class ChocolateyNewSettings : ChocolateySettings
    {
        private readonly Dictionary<string, string> _additionalPropertyValues = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the path where the package will be created.
        /// </summary>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the version of the package to be created.
        /// </summary>
        public string PackageVersion { get; set; }

        /// <summary>
        /// Gets or sets the owner of the package to be created.
        /// </summary>
        public string MaintainerName { get; set; }

        /// <summary>
        /// Gets or sets the repository of the package source.
        /// </summary>
        public string MaintainerRepo { get; set; }

        /// <summary>
        /// Gets or sets the type of the installer.
        /// </summary>
        public string InstallerType { get; set; }

        /// <summary>
        /// Gets or sets the URL where the software to be installed can be downloaded from.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the URL where the 64-Bit version of the software to be installed can be downloaded from.
        /// </summary>
        public string Url64 { get; set; }

        /// <summary>
        /// Gets or sets the arguments for running the installer silently.
        /// </summary>
        public string SilentArgs { get; set; }

        /// <summary>
        /// Gets the list of additional property values which should be passed to the template.
        /// </summary>
        public Dictionary<string, string> AdditionalPropertyValues
        {
            get
            {
                return _additionalPropertyValues;
            }
        }
    }
}