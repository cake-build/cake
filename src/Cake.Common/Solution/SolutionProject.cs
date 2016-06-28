// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Solution
{
    /// <summary>
    /// Represents a project in a MSBuild solution.
    /// </summary>
    public class SolutionProject
    {
        /// <summary>
        /// Gets the project identity.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the project name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the project path.
        /// </summary>
        public FilePath Path { get; }

        /// <summary>
        /// Gets the project type identity.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the parent project if any, otherwise null.
        /// </summary>
        public SolutionProject Parent { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionProject"/> class.
        /// </summary>
        /// <param name="id">The project identity.</param>
        /// <param name="name">The project name.</param>
        /// <param name="path">The project path.</param>
        /// <param name="type">The project type identity.</param>
        public SolutionProject(string id, string name, FilePath path, string type)
        {
            Id = id;
            Name = name;
            Path = path;
            Type = type;
        }
    }
}