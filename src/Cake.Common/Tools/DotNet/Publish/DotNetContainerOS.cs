// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNet.Publish;

/// <summary>
/// Represents the target operating system of a container image, passed as <c>--os</c>.
/// Known values are available as static properties, other values can be implicitly converted from a string.
/// </summary>
/// <param name="Value">The operating system value, i.e. <c>linux</c>.</param>
/// <example>
/// <code>
/// DotNetPublish("./src/hwapp/hwapp.csproj", new DotNetPublishSettings
/// {
///     Container = new DotNetPublishContainerSettings
///     {
///         OS = DotNetContainerOS.Linux,
///         Arch = DotNetContainerArchitecture.X64
///     }
/// }.WithContainerArchive("./artifacts/hwapp.tar.gz"));
/// </code>
/// </example>
public readonly record struct DotNetContainerOS(string Value)
{
    /// <summary>
    /// Gets the Linux operating system.
    /// </summary>
    public static DotNetContainerOS Linux { get; } = new("linux");

    /// <summary>
    /// Gets the Windows operating system.
    /// </summary>
    public static DotNetContainerOS Windows { get; } = new("win");

    /// <summary>
    /// Converts a string to a <see cref="DotNetContainerOS"/>.
    /// </summary>
    /// <param name="value">The operating system value.</param>
    public static implicit operator DotNetContainerOS(string value) => new(value);

    /// <inheritdoc/>
    public override string ToString() => Value;
}
