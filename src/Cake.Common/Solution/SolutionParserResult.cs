using System.Collections.Generic;
using System.Linq;

namespace Cake.Common.Solution
{
    /// <summary>
    /// Represents the content in an MSBuild solution file
    /// </summary>
    public class SolutionParserResult
    {
        private readonly ICollection<SolutionProject> _projects;

        /// <summary>
        /// Solution Projects
        /// </summary>
        public ICollection<SolutionProject> Projects
        {
            get { return _projects; }
        }

        /// <summary>
        /// Solution Projects
        /// </summary>
        /// <param name="projects">Solution Projects</param>
        public SolutionParserResult(IEnumerable<SolutionProject> projects)
        {
            _projects = projects
                .ToList()
                .AsReadOnly();
        }
    }
}