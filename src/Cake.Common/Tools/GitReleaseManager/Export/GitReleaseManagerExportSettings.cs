// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.GitReleaseManager.Export
{
    /// <summary>
    /// Contains settings used by <see cref="GitReleaseManagerExporter"/>.
    /// </summary>
    public sealed class GitReleaseManagerExportSettings : GitReleaseManagerSettings
    {
        /// <summary>
        /// Gets or sets the tag name to be used when exporting the release notes.
        /// </summary>
        public string TagName { get; set; }
    }
}