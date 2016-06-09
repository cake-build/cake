// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.Push
{
    /// <summary>
    /// Contains settings used by <see cref="NuGetPusher"/>.
    /// </summary>
    public sealed class NuGetPushSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the server URL. If not specified, nuget.org is used unless
        /// DefaultPushSource config value is set in the NuGet config file.
        /// Starting with NuGet 2.5, if NuGet.exe identifies a UNC/folder source,
        /// it will perform the file copy to the source.
        /// </summary>
        /// <value>The server URL.</value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the API key for the server.
        /// </summary>
        /// <value>The API key for the server.</value>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the timeout for pushing to a server.
        /// Defaults to 300 seconds (5 minutes).
        /// </summary>
        /// <value>The timeout for pushing to a server.</value>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// Gets or sets the verbosity.
        /// </summary>
        /// <value>The verbosity.</value>
        public NuGetVerbosity? Verbosity { get; set; }

        /// <summary>
        /// Gets or sets the NuGet configuration file.
        /// </summary>
        /// <value>The NuGet configuration file.</value>
        public FilePath ConfigFile { get; set; }
    }
}
