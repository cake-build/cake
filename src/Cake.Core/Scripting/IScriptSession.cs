// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Cake.Core.IO;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script session.
    /// </summary>
    public interface IScriptSession
    {
        /// <summary>
        /// Gets a value indicating whether or not the ScriptSession supports cached execution.
        /// </summary>
        bool SupportsCachedExecution { get; }

        /// <summary>
        /// Gets a value indicating whether or not the current cache is valid. If true, the cached version can be executed immediately.
        /// </summary>
        bool IsCacheValid { get; }

        /// <summary>
        /// Adds a reference path to the session.
        /// </summary>
        /// <param name="path">The reference path.</param>
        void AddReference(FilePath path);

        /// <summary>
        /// Adds an assembly reference to the session.
        /// </summary>
        /// <param name="assembly">The assembly reference.</param>
        void AddReference(Assembly assembly);

        /// <summary>
        /// Imports a namespace to the session.
        /// </summary>
        /// <param name="namespace">The namespace to import.</param>
        void ImportNamespace(string @namespace);

        /// <summary>
        /// Executes the specified script.
        /// </summary>
        /// <param name="script">The script to execute.</param>
        void Execute(Script script);
    }
}