// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.Store
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetStorer" />.
    /// </summary>
    public class DotNetStoreSettings : DotNetSettings
    {
        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the version of the installed Shared Framework to use to run the application.
        /// </summary>
        public string FrameworkVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip the optimization phase.
        /// </summary>
        public bool SkipOptimization { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip symbol generation.
        /// </summary>
        /// <remarks>
        /// Currently, you can only generate symbols on Windows and Linux.
        /// </remarks>
        public bool SkipSymbols { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the SDK runtime identifier (RID).
        /// </summary>
        public bool UseCurrentRuntime { get; set; }
    }
}
