// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNet.Publish;

/// <summary>
/// Represents the target architecture of a container image, passed as <c>--arch</c>.
/// Known values are available as static properties, other values can be implicitly converted from a string.
/// </summary>
/// <param name="Value">The architecture value, i.e. <c>x64</c>.</param>
/// <example>
/// <code>
/// DotNetPublish("./src/hwapp/hwapp.csproj", new DotNetPublishSettings
/// {
///     Container = new DotNetPublishContainerSettings
///     {
///         OS = DotNetContainerOS.Linux,
///         Arch = DotNetContainerArchitecture.Arm64
///     }
/// }.WithContainerArchive("./artifacts/hwapp.tar.gz"));
/// </code>
/// </example>
public readonly record struct DotNetContainerArchitecture(string Value)
{
    /// <summary>
    /// Gets the x64 architecture.
    /// </summary>
    public static DotNetContainerArchitecture X64 { get; } = new("x64");

    /// <summary>
    /// Gets the x86 architecture.
    /// </summary>
    public static DotNetContainerArchitecture X86 { get; } = new("x86");

    /// <summary>
    /// Gets the Arm64 architecture.
    /// </summary>
    public static DotNetContainerArchitecture Arm64 { get; } = new("arm64");

    /// <summary>
    /// Gets the Arm (32-bit) architecture.
    /// </summary>
    public static DotNetContainerArchitecture Arm { get; } = new("arm");

    /// <summary>
    /// Gets the s390x architecture.
    /// </summary>
    public static DotNetContainerArchitecture S390x { get; } = new("s390x");

    /// <summary>
    /// Gets the ppc64le architecture.
    /// </summary>
    public static DotNetContainerArchitecture Ppc64le { get; } = new("ppc64le");

    /// <summary>
    /// Gets the LoongArch64 architecture.
    /// </summary>
    public static DotNetContainerArchitecture LoongArch64 { get; } = new("loongarch64");

    /// <summary>
    /// Gets the RISC-V 64-bit architecture.
    /// </summary>
    public static DotNetContainerArchitecture RiscV64 { get; } = new("riscv64");

    /// <summary>
    /// Converts a string to a <see cref="DotNetContainerArchitecture"/>.
    /// </summary>
    /// <param name="value">The architecture value.</param>
    public static implicit operator DotNetContainerArchitecture(string value) => new(value);

    /// <inheritdoc/>
    public override string ToString() => Value;
}
