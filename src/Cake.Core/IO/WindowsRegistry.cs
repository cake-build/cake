// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core.IO;

/// <summary>
/// Represents an Windows implementation of <see cref="IRegistry"/>.
/// </summary>
public sealed class WindowsRegistry : IRegistry
{
#pragma warning disable CA1416
    /// <inheritdoc/>
    public IRegistryKey CurrentUser => CreateKey(() => Microsoft.Win32.Registry.CurrentUser);

    /// <inheritdoc/>
    public IRegistryKey LocalMachine => CreateKey(() => Microsoft.Win32.Registry.LocalMachine);

    /// <inheritdoc/>
    public IRegistryKey ClassesRoot => CreateKey(() => Microsoft.Win32.Registry.ClassesRoot);

    /// <inheritdoc/>
    public IRegistryKey Users => CreateKey(() => Microsoft.Win32.Registry.Users);

    /// <inheritdoc/>
    public IRegistryKey PerformanceData => CreateKey(() => Microsoft.Win32.Registry.PerformanceData);

    /// <inheritdoc/>
    public IRegistryKey CurrentConfig => CreateKey(() => Microsoft.Win32.Registry.CurrentConfig);

    private static IRegistryKey CreateKey(Func<Microsoft.Win32.RegistryKey> keyFactory) => new WindowsRegistryKey(keyFactory);
#pragma warning restore CA1416
}
