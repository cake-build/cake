// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Cake.Core.IO;

namespace Cake.Core.Reflection
{
    /// <summary>
    /// Represents an assembly loader.
    /// </summary>
    public interface IAssemblyLoader
    {
        /// <summary>
        /// Loads the specified assembly from it's assembly name.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <returns>The loaded assembly.</returns>
        Assembly Load(AssemblyName assemblyName);

        /// <summary>
        /// Loads the specified assembly from the specified path.
        /// </summary>
        /// <param name="path">The assembly path to load.</param>
        /// <returns>The loaded assembly.</returns>
        Assembly Load(FilePath path);
    }
}
