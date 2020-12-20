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
        /// <inheritdoc/>
#pragma warning disable CA1416
        public IRegistryKey LocalMachine => new WindowsRegistryKey(Microsoft.Win32.Registry.LocalMachine);
#pragma warning restore CA1416
    }
}