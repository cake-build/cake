// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Runtime.Versioning;

namespace Cake.NuGet
{
    /// <summary>
    /// Parses version-specific folder names into an instance of FrameworkName
    /// </summary>
    public interface IFrameworkNameParser
    {
        /// <summary>
        /// This function tries to normalize a string that represents framework version names
        /// (the name of a framework version-specific folder in a nuget package, i.e., "net451" or "net35") into
        /// something a framework name that the package manager understands.
        /// </summary>
        /// <param name="frameworkName">value to be parsed.</param>
        /// <returns>A FrameworkName instance corresponding with the provided frameworkName token.
        /// When parsing is unsuccessful, returns a FrameworkName with an Identifier property of "Unsupported."</returns>
        /// <exception cref="ArgumentNullException">when frameworkName is null.</exception>
        FrameworkName ParseFrameworkName(string frameworkName);
    }
}
