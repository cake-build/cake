// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// Contains the common settings used by all commands in <see cref="OctopusDeployReleaseCreator"/>.
    /// </summary>
    public abstract class OctopusDeploySettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the username to use when authenticating with the server
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password to use when authenticating with the server
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the octopus server url.
        /// </summary>
        public string Server { get; set; }

         /// <summary>
        /// Gets or sets the user's API key.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the text file of default values
        /// </summary>
        public FilePath ConfigurationFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the enable debug logging flag is set
        /// </summary>
        public bool EnableDebugLogging { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ignore SSL errors flag is set
        /// </summary>
        public bool IgnoreSslErrors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the enable service messages flag is set
        /// </summary>
        public bool EnableServiceMessages { get; set; }
    }
}
