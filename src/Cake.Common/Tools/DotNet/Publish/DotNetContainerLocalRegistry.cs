// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNet.Publish;

/// <summary>
/// Represents the local container registry (container engine) a container image is loaded into,
/// i.e. the <c>LocalRegistry</c> MSBuild property.
/// Known values are available as static properties, other values can be implicitly converted from a string.
/// </summary>
/// <param name="Value">The local registry value, i.e. <c>Docker</c>.</param>
/// <example>
/// <code>
/// DotNetPublish("./src/hwapp/hwapp.csproj", new DotNetPublishSettings()
///     .WithContainerLocalRegistry(DotNetContainerLocalRegistry.Podman, "cake-it-hwapp", "test"));
/// </code>
/// </example>
public readonly record struct DotNetContainerLocalRegistry(string Value)
{
    /// <summary>
    /// Gets the Docker local registry.
    /// </summary>
    public static DotNetContainerLocalRegistry Docker { get; } = new(nameof(Docker));

    /// <summary>
    /// Gets the Podman local registry.
    /// </summary>
    public static DotNetContainerLocalRegistry Podman { get; } = new(nameof(Podman));

    /// <summary>
    /// Gets the Windows Subsystem for Linux Container (WSLC) local registry.
    /// </summary>
    /// <remarks>
    /// Requires .NET 11 SDK or newer.
    /// </remarks>
    public static DotNetContainerLocalRegistry Wslc { get; } = new(nameof(Wslc));

    /// <summary>
    /// Gets the Apple macOS container local registry.
    /// </summary>
    /// <remarks>
    /// Requires .NET 11 SDK or newer.
    /// </remarks>
    public static DotNetContainerLocalRegistry MacOSContainer { get; } = new(nameof(MacOSContainer));

    /// <summary>
    /// Converts a string to a <see cref="DotNetContainerLocalRegistry"/>.
    /// </summary>
    /// <param name="value">The local registry value.</param>
    public static implicit operator DotNetContainerLocalRegistry(string value) => new(value);

    /// <inheritdoc/>
    public override string ToString() => Value;
}
