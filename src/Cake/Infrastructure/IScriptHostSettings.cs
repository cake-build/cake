// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Infrastructure
{
    /// <summary>
    /// Represents settings for a script host.
    /// </summary>
    public interface IScriptHostSettings
    {
        /// <summary>
        /// Gets a value indicating whether to run in debug mode.
        /// </summary>
        bool Debug { get; }

        /// <summary>
        /// Gets the script file path.
        /// </summary>
        FilePath Script { get; }
    }
}
