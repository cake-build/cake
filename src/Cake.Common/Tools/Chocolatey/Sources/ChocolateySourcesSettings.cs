// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.Chocolatey.Sources
{
    using global::Cake.Core.IO;

    /// <summary>
    /// Contains settings used by <see cref="ChocolateySources"/>.
    /// </summary>
    public sealed class ChocolateySourcesSettings : ChocolateySettings
    {
        /// <summary>
        /// Gets or sets the (optional) user name.
        /// </summary>
        /// <value>Optional user name to be used when connecting to an authenticated source.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the (optional) password.
        /// </summary>
        /// <value>Optional password to be used when connecting to an authenticated source.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the path to a PFX certificate for use with x509 authenticated feeds.
        /// </summary>
        public FilePath Certificate { get; set; }

        /// <summary>
        /// Gets or sets the password for the <see cref="Certificate"/>.
        /// </summary>
        public string CertificatePassword { get; set; }

        /// <summary>
        /// Gets or sets the (optional) priority.
        /// </summary>
        /// <value>Optional priority to be used when creating source.</value>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether  source should explicitly bypass any explicitly set proxy.
        /// </summary>
        public bool ByPassProxy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether source should be allowed to be used in self service mode.
        /// </summary>
        public bool AllowSelfService { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether source should be visible to non-admin users.
        /// </summary>
        public bool AdminOnly { get; set; }
    }
}