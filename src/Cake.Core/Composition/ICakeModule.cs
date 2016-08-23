﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core.Composition
{
    /// <summary>
    /// Represents a module responsible for
    /// registering types and instances.
    /// </summary>
    public interface ICakeModule
    {
        /// <summary>
        /// Performs custom registrations in the provided registrar.
        /// </summary>
        /// <param name="registrar">The container registrar.</param>
        void Register(ICakeContainerRegistrar registrar);
    }
}