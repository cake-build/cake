// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;

namespace Cake.Common.Tools.SignTool
{
    /// <summary>
    /// Represents a sign tool resolver.
    /// </summary>
    /// <remarks>
    /// This exists only to be able to test the sign tool.
    /// Do not use this interface since it will be removed.
    /// </remarks>
    public interface ISignToolResolver
    {
        /// <summary>
        /// Resolves the path to the sign tool.
        /// </summary>
        /// <returns>The path to the sign tool.</returns>
        FilePath GetPath();
    }
}
