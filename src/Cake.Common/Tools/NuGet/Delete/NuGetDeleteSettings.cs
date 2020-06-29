// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.Delete
{
    /// <summary>
    /// Contains settings used by <see cref="NuGetDeleter"/>.
    /// </summary>
    public sealed class NuGetDeleteSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the server URL.
        /// When using NuGet pre 3.4.2, this value is optional
        /// and nuget.org is used if omitted (unless DefaultPushSource
        /// config value is set in the NuGet config file.
        /// When using NuGet 3.4.2 (or more recent), this value is mandatory.
        /// Starting with NuGet 2.5, if NuGet.exe identifies a UNC/folder source,
        /// it will perform the file copy to the source.
        /// </summary>
        /// <value>The server URL.</value>
        /// <remarks>
        /// For your convenience, here is the URL for some of the most popular
        /// public NuGet servers:
        /// - NuGet Gallery: https://nuget.org/api/v2/package
        /// - MyGet: https://www.myget.org/F/&lt;your_username&gt;/api/v2/package.
        /// </remarks>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the API key for the server.
        /// </summary>
        /// <value>The API key for the server.</value>
        public string ApiKey { get; set; }

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
