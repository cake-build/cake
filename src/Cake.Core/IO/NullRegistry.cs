// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core.IO
{
    /// <summary>
    /// A no-op registry implementation for platforms without Windows registry support.
    /// </summary>
    internal sealed class NullRegistry : IRegistry
    {
        /// <inheritdoc/>
        public IRegistryKey CurrentUser => NullRegistryKey.Instance;

        /// <inheritdoc/>
        public IRegistryKey LocalMachine => NullRegistryKey.Instance;

        /// <inheritdoc/>
        public IRegistryKey ClassesRoot => NullRegistryKey.Instance;

        /// <inheritdoc/>
        public IRegistryKey Users => NullRegistryKey.Instance;

        /// <inheritdoc/>
        public IRegistryKey PerformanceData => NullRegistryKey.Instance;

        /// <inheritdoc/>
        public IRegistryKey CurrentConfig => NullRegistryKey.Instance;
    }
}
