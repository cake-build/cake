// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents the Windows registry.
    /// </summary>
    public interface IRegistry
    {
        /// <summary>
        /// Gets a registry key representing HKEY_CURRENT_USER.
        /// </summary>
        /// <value>
        /// A registry key representing HKEY_CURRENT_USER.
        /// </value>
        IRegistryKey CurrentUser { get; }

        /// <summary>
        /// Gets a registry key representing HKEY_LOCAL_MACHINE.
        /// </summary>
        /// <value>
        /// A registry key representing HKEY_LOCAL_MACHINE.
        /// </value>
        IRegistryKey LocalMachine { get; }

        /// <summary>
        /// Gets a registry key representing HKEY_CLASSES_ROOT.
        /// </summary>
        /// <value>
        /// A registry key representing HKEY_CLASSES_ROOT.
        /// </value>
        IRegistryKey ClassesRoot { get; }

        /// <summary>
        /// Gets a registry key representing HKEY_USERS.
        /// </summary>
        /// <value>
        /// A registry key representing HKEY_USERS.
        /// </value>
        IRegistryKey Users { get; }

        /// <summary>
        /// Gets a registry key representing HKEY_PERFORMANCE_DATA.
        /// </summary>
        /// <value>
        /// A registry key representing HKEY_PERFORMANCE_DATA.
        /// </value>
        IRegistryKey PerformanceData { get; }

        /// <summary>
        /// Gets a registry key representing HKEY_CURRENT_CONFIG.
        /// </summary>
        /// <value>
        /// A registry key representing HKEY_CURRENT_CONFIG.
        /// </value>
        IRegistryKey CurrentConfig { get; }
    }
}