// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Net
{
    /// <summary>
    /// Contains settings for <see cref="HttpAliases"/>
    /// </summary>
    public sealed class DownloadFileSettings
    {
        /// <summary>
        /// Gets or sets the Username to use when downloading the file
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the Password to use when downloading the file
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value that controls whether default credentials are sent when downloading the file
        /// </summary>
        public bool UseDefaultCredentials { get; set; }
    }
}