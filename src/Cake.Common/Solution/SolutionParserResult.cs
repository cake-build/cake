using System.Collections.Generic;

namespace Cake.Common.Solution
{
    /// <summary>
    /// Represents the content in an MSBuild solution file
    /// </summary>
    public sealed class SolutionParserResult
    {
        private readonly string _version;
        private readonly string _visualStudioVersion;
        private readonly string _minimumVisualStudioVersion;
        private readonly IReadOnlyCollection<SolutionProject> _projects;

        /// <summary>
        /// Fileformat version
        /// </summary>
        public string Version
        {
            get { return _version; }
        }

        /// <summary>
        /// Version of Visual Studio that created the file
        /// </summary>
        public string VisualStudioVersion
        {
            get { return _visualStudioVersion; }
        }

        /// <summary>
        /// Minimum supported versions of Visual Studio
        /// </summary>
        public string MinimumVisualStudioVersion
        {
            get { return _minimumVisualStudioVersion; }
        }

        /// <summary>
        /// Solution Projects
        /// </summary>
        public IReadOnlyCollection<SolutionProject> Projects
        {
            get { return _projects; }
        }

        /// <summary>
        /// Solution Projects
        /// </summary>
        /// <param name="version">Fileformat version</param>
        /// <param name="visualStudioVersion">Version of Visual Studio that created the file</param>
        /// <param name="minimumVisualStudioVersion">Minimum supported versions of Visual Studio</param>
        /// <param name="projects">Solution Projects</param>
        public SolutionParserResult(
            string version,
            string visualStudioVersion,
            string minimumVisualStudioVersion,
            IReadOnlyCollection<SolutionProject> projects)
        {
            _version = version;
            _visualStudioVersion = visualStudioVersion;
            _minimumVisualStudioVersion = minimumVisualStudioVersion;
            _projects = projects;
        }
    }
}