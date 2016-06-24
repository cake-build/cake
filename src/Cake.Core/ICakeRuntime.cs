// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Runtime.Versioning;

namespace Cake.Core
{
    /// <summary>
    /// Represents the runtime that Cake is running in.
    /// </summary>
    public interface ICakeRuntime
    {
        /// <summary>
        /// Gets the target .NET framework version that the current AppDomain is targeting.
        /// </summary>
        /// <returns>The target framework.</returns>
        FrameworkName TargetFramework { get; }
    }
}