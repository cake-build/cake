// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting
{
    /// <summary>
    /// Represents Frosting's task finder mechanism.
    /// </summary>
    public interface ICakeTaskFinder
    {
        /// <summary>
        /// Gets all task types present in the provided assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>An array of task types.</returns>
        Type[] GetTasks(IEnumerable<Assembly> assemblies);
    }
}
