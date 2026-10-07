// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.Project.Convert;

/// <summary>
/// Contains settings used by <see cref="DotNetProjectConverter" />.
/// </summary>
public sealed class DotNetProjectConvertSettings : DotNetSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether to delete the original file-based program after conversion.
    /// </summary>
    public bool DeleteSource { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show what would happen without writing files.
    /// </summary>
    public bool DryRun { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to overwrite existing output.
    /// </summary>
    public bool Force { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to allow the command to stop and wait for user input or action.
    /// For example, to complete authentication.
    /// </summary>
    public bool Interactive { get; set; }

    /// <summary>
    /// Gets or sets the output directory for the converted project.
    /// </summary>
    public DirectoryPath Output { get; set; }
}
