// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.Configuration
{
    /// <summary>
    /// Represents the Cake configuration.
    /// </summary>
    public interface ICakeConfiguration
    {
        /// <summary>
        /// Gets the value that corresponds to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value for the specified key, or <c>null</c> if key doesn't exists.</returns>
        string GetValue(string key);
    }
}
