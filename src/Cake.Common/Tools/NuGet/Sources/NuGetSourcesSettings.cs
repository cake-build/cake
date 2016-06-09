// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.Sources
{
    /// <summary>
    /// Contains settings used by <see cref="NuGetSources"/>.
    /// </summary>
    public sealed class NuGetSourcesSettings : ToolSettings
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
        /// Gets or sets the output verbosity.
        /// </summary>
        /// <value>The output verbosity.</value>
        public NuGetVerbosity? Verbosity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this source contains sensitive data, i.e. authentication token in url.
        /// </summary>
        /// <value>
        /// <c>true</c> if this source contains sensitive data; otherwise, <c>false</c>.
        /// </value>
        public bool IsSensitiveSource { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not encrypt the password and store it in clear text. (Default: <c>false</c>)
        /// </summary>
        /// <value>
        /// <c>true</c> if password is stored as unencrypted; otherwise, <c>false</c>.
        /// </value>
        public bool StorePasswordInClearText { get; set; }
    }
}
