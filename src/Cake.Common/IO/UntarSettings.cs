// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.IO;

/// <summary>
/// Contains settings used by <see cref="TarAliases"/> when extracting tar archives.
/// </summary>
public sealed class UntarSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether files that are already up to date are skipped.
    /// Defaults to <c>true</c>.
    /// A destination file is skipped when it exists and its last write time is greater than or equal to the archive entry.
    /// </summary>
    public bool SkipUnchangedFiles { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether read-only destination files are overwritten.
    /// Defaults to <c>false</c>.
    /// </summary>
    public bool OverwriteReadOnlyFiles { get; set; }
}
