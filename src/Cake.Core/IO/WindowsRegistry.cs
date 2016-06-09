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
        /// <summary>
        /// Gets the LocalMachine <see cref="IRegistryKey"/>.
        /// </summary>
        public IRegistryKey LocalMachine
        {
            get { return new WindowsRegistryKey(Microsoft.Win32.Registry.LocalMachine); }
        }
    }
}
