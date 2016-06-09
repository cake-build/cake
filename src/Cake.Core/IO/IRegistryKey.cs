// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a Windows registry key.
    /// </summary>
    public interface IRegistryKey : IDisposable
    {
        /// <summary>
        /// Gets all sub keys.
        /// </summary>
        /// <returns>All sub keys.</returns>
        string[] GetSubKeyNames();

        /// <summary>
        /// Opens the sub key with the specified name.
        /// </summary>
        /// <param name="name">The name of the key.</param>
        /// <returns>The sub key with the specified name</returns>
        IRegistryKey OpenKey(string name);

        /// <summary>
        /// Gets the value of the key.
        /// </summary>
        /// <param name="name">The name of the key.</param>
        /// <returns>The value of the key.</returns>
        object GetValue(string name);
    }
}
