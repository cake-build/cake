// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.IO;

/// <summary>
/// Represents the compression used when creating a tar archive.
/// </summary>
public enum TarCompression
{
    /// <summary>
    /// No compression. Creates an uncompressed <c>.tar</c> archive.
    /// </summary>
    None,

    /// <summary>
    /// GZip compression. Creates a <c>.tar.gz</c> archive.
    /// </summary>
    GZip
}
