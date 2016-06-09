// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;

namespace Cake.Common.Solution
{
    /// <summary>
    /// Represents a project in a MSBuild solution.
    /// </summary>
    public sealed class SolutionProject
    {
        private readonly string _id;
        private readonly string _name;
        private readonly FilePath _path;
        private readonly string _type;

        /// <summary>
        /// Gets the project identity.
        /// </summary>
        public string Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the project name.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the project path.
        /// </summary>
        public FilePath Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets the project type identity.
        /// </summary>
        public string Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionProject"/> class.
        /// </summary>
        /// <param name="id">The project identity.</param>
        /// <param name="name">The project name.</param>
        /// <param name="path">The project path.</param>
        /// <param name="type">The project type identity.</param>
        public SolutionProject(string id, string name, FilePath path, string type)
        {
            _id = id;
            _name = name;
            _path = path;
            _type = type;
        }
    }
}
