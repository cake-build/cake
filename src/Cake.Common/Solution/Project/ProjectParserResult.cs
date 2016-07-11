// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.Linq;

namespace Cake.Common.Solution.Project
{
    /// <summary>
    /// Represents the content in an MSBuild project file.
    /// </summary>
    public sealed class ProjectParserResult
    {
        private readonly string _configuration;
        private readonly string _platform;
        private readonly string _projectGuid;
        private readonly string _outputType;
        private readonly string _rootNameSpace;
        private readonly string _assemblyName;
        private readonly string _targetFrameworkVersion;
        private readonly string _targetFrameworkProfile;
        private readonly ICollection<ProjectFile> _files;

        /// <summary>
        /// Gets the build configuration.
        /// </summary>
        /// <value>The build configuration.</value>
        public string Configuration
        {
            get { return _configuration; }
        }

        /// <summary>
        /// Gets the target platform.
        /// </summary>
        /// <value>The platform.</value>
        public string Platform
        {
            get { return _platform; }
        }

        /// <summary>
        /// Gets the unique project identifier.
        /// </summary>
        /// <value>The unique project identifier.</value>
        public string ProjectGuid
        {
            get { return _projectGuid; }
        }

        /// <summary>
        /// Gets the compiler output type, i.e. <c>Exe/Library</c>.
        /// </summary>
        /// <value>The output type.</value>
        public string OutputType
        {
            get { return _outputType; }
        }

        /// <summary>
        /// Gets the default root namespace.
        /// </summary>
        /// <value>The root namespace.</value>
        public string RootNameSpace
        {
            get { return _rootNameSpace; }
        }

        /// <summary>
        /// Gets the build target assembly name.
        /// </summary>
        /// <value>The assembly name.</value>
        public string AssemblyName
        {
            get { return _assemblyName; }
        }

        /// <summary>
        /// Gets the compiler target framework version.
        /// </summary>
        /// <value>The target framework version.</value>
        public string TargetFrameworkVersion
        {
            get { return _targetFrameworkVersion; }
        }

        /// <summary>
        /// Gets the compiler target framework profile.
        /// </summary>
        /// <value>The target framework profile.</value>
        public string TargetFrameworkProfile
        {
            get { return _targetFrameworkProfile; }
        }

        /// <summary>
        /// Gets the project content files.
        /// </summary>
        /// <value>The files.</value>
        public ICollection<ProjectFile> Files
        {
            get { return _files; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectParserResult"/> class.
        /// </summary>
        /// <param name="configuration">The build configuration.</param>
        /// <param name="platform">The target platform.</param>
        /// <param name="projectGuid">The unique project identifier.</param>
        /// <param name="outputType">The compiler output type.</param>
        /// <param name="rootNameSpace">The default root namespace.</param>
        /// <param name="assemblyName">Gets the build target assembly name.</param>
        /// <param name="targetFrameworkVersion">The compiler framework version.</param>
        /// <param name="targetFrameworkProfile">The compiler framework profile.</param>
        /// <param name="files">The project content files.</param>
        public ProjectParserResult(
            string configuration,
            string platform,
            string projectGuid,
            string outputType,
            string rootNameSpace,
            string assemblyName,
            string targetFrameworkVersion,
            string targetFrameworkProfile,
            IEnumerable<ProjectFile> files)
        {
            _configuration = configuration;
            _platform = platform;
            _projectGuid = projectGuid;
            _outputType = outputType;
            _rootNameSpace = rootNameSpace;
            _assemblyName = assemblyName;
            _targetFrameworkVersion = targetFrameworkVersion;
            _targetFrameworkProfile = targetFrameworkProfile;
            _files = files.ToList().AsReadOnly();
        }
    }
}
