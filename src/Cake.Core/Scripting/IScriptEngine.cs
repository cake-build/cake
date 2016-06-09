// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script engine.
    /// </summary>
    public interface IScriptEngine
    {
        /// <summary>
        /// Creates a new script session.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="arguments">The script arguments.</param>
        /// <returns>A new script session.</returns>
        IScriptSession CreateSession(IScriptHost host, IDictionary<string, string> arguments);
    }
}
