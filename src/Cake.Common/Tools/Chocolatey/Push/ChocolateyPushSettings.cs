// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.Chocolatey.Push
{
    using System;

    /// <summary>
    /// Contains settings used by <see cref="ChocolateyPusher"/>.
    /// </summary>
    public sealed class ChocolateyPushSettings : ChocolateySettings
    {
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the API key for the server.
        /// </summary>
        /// <value>The API key for the server.</value>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the client code generated for delegating access vy a user to the Intune endpoints.
        /// </summary>
        public string ClientCode { get; set; }

        /// <summary>
        /// Gets or sets the URL used when requesting the client code.
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets the Intune API endpoint to use.
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip cleanup local files when pushing to Intune.
        /// </summary>
        public bool SkipCleanup { get; set; }
    }
}