// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Cake.Common.Solution
{
    /// <summary>
    /// Represents the content in an MSBuild solution file.
    /// </summary>
    public sealed class SolutionParserResult
    {
        /// <summary>
        /// Gets the file format version.
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Gets the version of Visual Studio that created the file.
        /// </summary>
        public string VisualStudioVersion { get; }

        /// <summary>
        /// Gets the minimum supported version of Visual Studio.
        /// </summary>
        public string MinimumVisualStudioVersion { get; }

        /// <summary>
        /// Gets all solution projects.
        /// </summary>
        public IReadOnlyCollection<SolutionProject> Projects { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionParserResult"/> class.
        /// </summary>
        /// <param name="version">The file format version.</param>
        /// <param name="visualStudioVersion">The version of Visual Studio that created the file.</param>
        /// <param name="minimumVisualStudioVersion">The minimum supported version of Visual Studio.</param>
        /// <param name="projects">The solution projects.</param>
        public SolutionParserResult(string version, string visualStudioVersion,
            string minimumVisualStudioVersion, IReadOnlyCollection<SolutionProject> projects)
        {
            Version = version;
            VisualStudioVersion = visualStudioVersion;
            MinimumVisualStudioVersion = minimumVisualStudioVersion;
            Projects = projects;
        }
    }
}