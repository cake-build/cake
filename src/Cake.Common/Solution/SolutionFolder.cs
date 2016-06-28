// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Solution
{
    /// <summary>
    /// Represents a folder in a MSBuild solution.
    /// </summary>
    public sealed class SolutionFolder : SolutionProject
    {
        /// <summary>
        /// Visual Studio project type guid for solution folder
        /// </summary>
        /// <remarks>
        /// More information can be found http://www.codeproject.com/Reference/720512/List-of-Visual-Studio-Project-Type-GUIDs
        /// </remarks>
        public const string TypeIdentifier = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}";

        /// <summary>
        /// Gets Child items of this folder
        /// </summary>
        public List<SolutionProject> Items { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionFolder"/> class.
        /// </summary>
        /// <param name="id">The folder project identity.</param>
        /// <param name="name">The folder name.</param>
        /// <param name="path">The folder path.</param>
        public SolutionFolder(string id, string name, FilePath path) : base(id, name, path, TypeIdentifier)
        {
            Items = new List<SolutionProject>();
        }
    }
}