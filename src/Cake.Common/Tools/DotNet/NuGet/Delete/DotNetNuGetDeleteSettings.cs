// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNetCore.NuGet.Delete;

namespace Cake.Common.Tools.DotNet.NuGet.Delete
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCoreNuGetDeleter" />.
    /// </summary>
    public class DotNetNuGetDeleteSettings : DotNetSettings
    {
        /// <summary>
        /// Gets or sets a value indicating the server URL.
        /// </summary>
        /// <remarks>
        /// Supported URLs for nuget.org include http://www.nuget.org, http://www.nuget.org/api/v3,
        /// and http://www.nuget.org/api/v2/package. For private feeds, substitute the host name
        /// (for example, %hostname%/api/v3).
        /// </remarks>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to append "api/v2/package" to the source URL.
        /// </summary>
        /// <remarks>
        /// Available since .NET Core 2.1 SDK.
        /// </remarks>
        public bool NoServiceEndpoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to block and require manual action for operations like authentication.
        /// </summary>
        /// <remarks>
        /// Available since .NET Core 2.2 SDK.
        /// </remarks>
        public bool Interactive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to prompt for user input or confirmations.
        /// </summary>
        public bool NonInteractive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the API key for the server.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force command-line output in English.
        /// </summary>
        public bool ForceEnglishOutput { get; set; }
    }
}