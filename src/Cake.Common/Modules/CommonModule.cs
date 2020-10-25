// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Composition;

namespace Cake.Common.Modules
{
    /// <summary>
    /// The module responsible for registering
    /// default types in the Cake.Common assembly.
    /// </summary>
    public sealed class CommonModule : ICakeModule
    {
        /// <inheritdoc/>
        public void Register(ICakeContainerRegistrar registrar)
        {
        }
    }
}
