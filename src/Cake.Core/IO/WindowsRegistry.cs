// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents an Windows implementation of <see cref="IRegistry"/>.
    /// </summary>
    public sealed class WindowsRegistry : IRegistry
    {
#pragma warning disable CA1416
        /// <inheritdoc/>
        public IRegistryKey CurrentUser => new WindowsRegistryKey(Microsoft.Win32.Registry.CurrentUser);

        /// <inheritdoc/>
        public IRegistryKey LocalMachine => new WindowsRegistryKey(Microsoft.Win32.Registry.LocalMachine);

        /// <inheritdoc/>
        public IRegistryKey ClassesRoot => new WindowsRegistryKey(Microsoft.Win32.Registry.ClassesRoot);

        /// <inheritdoc/>
        public IRegistryKey Users => new WindowsRegistryKey(Microsoft.Win32.Registry.Users);

        /// <inheritdoc/>
        public IRegistryKey PerformanceData => new WindowsRegistryKey(Microsoft.Win32.Registry.PerformanceData);

        /// <inheritdoc/>
        public IRegistryKey CurrentConfig => new WindowsRegistryKey(Microsoft.Win32.Registry.CurrentConfig);
#pragma warning restore CA1416
    }
}