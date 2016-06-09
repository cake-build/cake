// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.Reflection;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script alias generator.
    /// </summary>
    public interface IScriptAliasFinder
    {
        /// <summary>
        /// Finds script aliases in the provided assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to find script aliases in.</param>
        /// <returns>The script aliases that were found.</returns>
        IReadOnlyList<ScriptAlias> FindAliases(IEnumerable<Assembly> assemblies);
    }
}
