// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.Chocolatey.Pin
{
    /// <summary>
    /// Contains settings used by <see cref="ChocolateyPinner"/>.
    /// </summary>
    public sealed class ChocolateyPinSettings : ChocolateySettings
    {
        /// <summary>
        /// Gets or sets the version of the package to pin.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a reason for pinning a package during installation.
        /// </summary>
        public string PinReason { get; set; }
    }
}