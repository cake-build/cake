using Cake.Core.IO;

namespace Cake.Common.Solution
{
    /// <summary>
    /// Solution Project
    /// </summary>
    public class SolutionProject
    {
        private readonly string _id;
        private readonly string _name;
        private readonly FilePath _path;
        private readonly string _type;

        /// <summary>
        /// Project Id
        /// </summary>
        public string Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Project Name
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Project Path
        /// </summary>
        public FilePath Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Project Type Id
        /// </summary>
        public string Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Solution Project Constructor
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <param name="name">Project Name</param>
        /// <param name="path">Project Path</param>
        /// <param name="type">Project Type Id</param>
        public SolutionProject(
            string id,
            string name,
            FilePath path,
            string type
            )
        {
            _id = id;
            _name = name;
            _path = path;
            _type = type;
        }
    }
}