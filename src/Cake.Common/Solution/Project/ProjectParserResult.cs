using System.Collections.Generic;
using System.Linq;

namespace Cake.Common.Solution.Project
{
    /// <summary>
    /// Represents the content in an MSBuild Project file 
    /// </summary>
    public class ProjectParserResult
    {
        private readonly string _configuration;
        private readonly string _platform;
        private readonly string _projectGuid;
        private readonly string _outputType;
        private readonly string _rootNameSpace;
        private readonly string _assemblyName;
        private readonly string _targetFrameworkVersion;
        private readonly ICollection<ProjectFile> _files;

        /// <summary>
        /// Build Configuration
        /// </summary>
        public string Configuration
        {
            get { return _configuration; }
        }

         /// <summary>
        /// Target platform
        /// </summary>
        public string Platform
        {
            get { return _platform; }
        }

        /// <summary>
        /// Project identifier
        /// </summary>
        public string ProjectGuid
        {
            get { return _projectGuid; }
        }

         /// <summary>
        /// Compiler output i.e. Exe/Library
        /// </summary>
        public string OutputType
        {
            get { return _outputType; }
        }

         /// <summary>
        /// Default root name space
        /// </summary>
        public string RootNameSpace
        {
            get { return _rootNameSpace; }
        }

        /// <summary>
        /// Build target assembly name
        /// </summary>
        public string AssemblyName
        {
            get { return _assemblyName; }
        }

        /// <summary>
        /// Compiler target framework version
        /// </summary>
        public string TargetFrameworkVersion
        {
            get { return _targetFrameworkVersion; }
        }

         /// <summary>
        /// Project content files
        /// </summary>
       public ICollection<ProjectFile> Files
        {
            get { return _files; }
        }

        /// <summary>
        /// Constructor for <see cref="ProjectParserResult"/>
        /// </summary>
        /// <param name="configuration">Build Configuration</param>
        /// <param name="platform">Target platform</param>
        /// <param name="projectGuid">Project identifier</param>
        /// <param name="outputType">Compiler output i.e. Exe/Library</param>
        /// <param name="rootNameSpace">Build target assembly name</param>
        /// <param name="assemblyName">Compiler framework version</param>
        /// <param name="targetFrameworkVersion">Compiler framework version</param>
        /// <param name="files">Project content files</param>
        public ProjectParserResult(
            string configuration,
            string platform,
            string projectGuid,
            string outputType,
            string rootNameSpace,
            string assemblyName,
            string targetFrameworkVersion,
            IEnumerable<ProjectFile> files
            )
        {
            _configuration = configuration;
            _platform = platform;
            _projectGuid = projectGuid;
            _outputType = outputType;
            _rootNameSpace = rootNameSpace;
            _assemblyName = assemblyName;
            _targetFrameworkVersion = targetFrameworkVersion;
            _files = files
                .ToList()
                .AsReadOnly();
        }
    }
}