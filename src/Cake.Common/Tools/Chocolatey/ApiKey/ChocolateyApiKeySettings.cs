// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.Chocolatey.ApiKey
{
    /// <summary>
    /// Contains settings used by <see cref="ChocolateyApiKeySetter"/>.
    /// </summary>
    public sealed class ChocolateyApiKeySettings : ChocolateySettings
    {
        /// <summary>
        /// Gets or sets the API key for the server.
        /// </summary>
        /// <value>The API key for the server.</value>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remove the selected source.
        /// </summary>
        public bool Remove { get; set; }
    }
}