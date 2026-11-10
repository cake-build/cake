// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.IO;

/// <summary>
/// Contains settings used by <see cref="TarAliases"/>.
/// </summary>
public sealed class TarSettings
{
    /// <summary>
    /// Gets or sets the compression used when creating the archive.
    /// Defaults to <see cref="TarCompression.None"/>.
    /// </summary>
    public TarCompression Compression { get; set; } = TarCompression.None;

    /// <summary>
    /// Gets or sets a value indicating whether an existing destination archive is overwritten.
    /// Defaults to <c>true</c>.
    /// </summary>
    public bool Overwrite { get; set; } = true;
}
