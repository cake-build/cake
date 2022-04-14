// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Net
{
    /// <summary>
    /// Contains settings for <see cref="HttpAliases"/>.
    /// </summary>
    public sealed class UploadFileSettings
    {
        /// <summary>
        /// Gets or sets the username to use when uploadingthe file.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password to use when uploading the file.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether default credentials are sent when uploading the file.
        /// </summary>
        /// <remarks>
        /// If set to true, any username and password that has been specified will be ignored.
        /// </remarks>
        public bool UseDefaultCredentials { get; set; }
    }
}