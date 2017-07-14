// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNetCore.NuGet.Push
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCoreNuGetPusher" />.
    /// </summary>
    public sealed class DotNetCoreNuGetPushSettings : DotNetCoreSettings
    {
        /// <summary>
        /// Gets or sets a value indicating the server URL.
        /// </summary>
        /// <remarks>
        /// This option is required unless DefaultPushSource config value is set in the NuGet config file.
        /// </remarks>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the symbol server URL.
        /// </summary>
        public string SymbolSource { get; set; }

        /// <summary>
        /// Gets or sets a value indicating timeout for pushing to a server in seconds.
        /// <remarks>
        /// Defaults to 300 seconds (5 minutes). Specifying 0 (zero seconds) applies the default value.
        /// </remarks>
        /// </summary>
        public int? Timeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the API key for the server.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the API key for the symbol server.
        /// </summary>
        public string SymbolApiKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether buffering is disabled when pushing to an HTTP(S) server.
        /// </summary>
        /// <remarks>
        /// This decreases memory usage.
        /// </remarks>
        public bool DisableBuffering { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether symbols should be not be pushed if present.
        /// </summary>
        public bool IgnoreSymbols { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force command-line output in English.
        /// </summary>
        public bool ForceEnglishOutput { get; set; }
    }
}