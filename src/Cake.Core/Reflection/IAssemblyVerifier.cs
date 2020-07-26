// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Cake.Core.Configuration;

namespace Cake.Core.Reflection
{
    /// <summary>
    /// Represents an assembly verifier.
    /// </summary>
    public interface IAssemblyVerifier
    {
        /// <summary>
        /// Verifies an assembly.
        /// </summary>
        /// <param name="assembly">The target assembly.</param>
        void Verify(Assembly assembly);
    }
}
