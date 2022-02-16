// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNetCore.NuGet.Source;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.NuGet.Source
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCoreNuGetSourcer" />.
    /// </summary>
    public class DotNetNuGetSourceSettings : DotNetSettings
    {
        /// <summary>
        /// Gets or sets the path to the package(s) source.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this source contains sensitive data, i.e. authentication token in url.
        /// </summary>
        public bool IsSensitiveSource { get; set; }

        /// <summary>
        /// Gets or sets the user name to be used when connecting to an authenticated source.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password to be used when connecting to an authenticated source.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable storing portable package source credentials by disabling password encryption.
        /// </summary>
        public bool StorePasswordInClearText { get; set; }

        /// <summary>
        /// Gets or sets the comma-separated list of valid authentication types for this source.
        /// </summary>
        /// <remarks>
        /// By default, all authentication types are valid. Example: basic,negotiate.
        /// </remarks>
        public string ValidAuthenticationTypes { get; set; }

        /// <summary>
        /// Gets or sets the NuGet configuration file.
        /// </summary>
        public FilePath ConfigFile { get; set; }
    }
}