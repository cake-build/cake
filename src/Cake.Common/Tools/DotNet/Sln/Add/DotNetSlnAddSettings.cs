// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.Sln.Add
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetSlnAdder" />.
    /// </summary>
    public sealed class DotNetSlnAddSettings : DotNetSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to place the projects in the root of the solution, rather than creating a solution folder.
        /// Can't be used with <see cref="SolutionFolder" />.
        /// </summary>
        public bool InRoot { get; set; }

        /// <summary>
        /// Gets or sets the destination solution folder path to add the projects to. Can't be used with <see cref="InRoot" />.
        /// </summary>
        public DirectoryPath SolutionFolder { get; set; }
    }
}
