// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Solution.Project
{
    /// <summary>
    /// The MSBuild project file parser.
    /// </summary>
    public sealed class ProjectParser
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectParser"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        public ProjectParser(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            _fileSystem = fileSystem;
            _environment = environment;
        }

        /// <summary>
        /// Parses a project file.
        /// </summary>
        /// <param name="projectPath">The project path.</param>
        /// <returns>The parsed project.</returns>
        public ProjectParserResult Parse(FilePath projectPath)
        {
            if (projectPath == null)
            {
                throw new ArgumentNullException(nameof(projectPath));
            }

            if (projectPath.IsRelative)
            {
                projectPath = projectPath.MakeAbsolute(_environment);
            }

            // Get the project file.
            var file = _fileSystem.GetFile(projectPath);
            if (!file.Exists)
            {
                const string format = "Project file '{0}' do not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, projectPath.FullPath);
                throw new CakeException(message);
            }

            XDocument document;
            using (var stream = file.OpenRead())
            {
                document = XDocument.Load(stream);
            }

            var projectProperties =
                (from project in document.Elements(ProjectXElement.Project)
                 from propertyGroup in project.Elements(ProjectXElement.PropertyGroup)
                 let configuration = propertyGroup
                     .Elements(ProjectXElement.Configuration)
                     .Select(cfg => cfg.Value)
                     .FirstOrDefault()
                 let platform = propertyGroup
                     .Elements(ProjectXElement.Platform)
                     .Select(cfg => cfg.Value)
                     .FirstOrDefault()
                 let configPropertyGroups = project.Elements(ProjectXElement.PropertyGroup)
                                            .Where(x => x.Elements(ProjectXElement.OutputPath).Any() && x.Attribute("Condition") != null)
                                            .Where(x => x.Attribute("Condition").Value.Contains(string.Concat("== '", configuration, "|", platform, "'")))
                 where !string.IsNullOrWhiteSpace(configuration)
                 select new
                 {
                     Configuration = configuration,
                     Platform = platform,
                     ProjectGuid = propertyGroup
                         .Elements(ProjectXElement.ProjectGuid)
                         .Select(projectGuid => projectGuid.Value)
                         .FirstOrDefault(),
                     OutputType = propertyGroup
                         .Elements(ProjectXElement.OutputType)
                         .Select(outputType => outputType.Value)
                         .FirstOrDefault(),
                     OutputPath = configPropertyGroups
                         .Elements(ProjectXElement.OutputPath)
                         .Select(outputPath => DirectoryPath.FromString(outputPath.Value))
                         .FirstOrDefault(),
                     RootNameSpace = propertyGroup
                         .Elements(ProjectXElement.RootNamespace)
                         .Select(rootNameSpace => rootNameSpace.Value)
                         .FirstOrDefault(),
                     AssemblyName = propertyGroup
                         .Elements(ProjectXElement.AssemblyName)
                         .Select(assemblyName => assemblyName.Value)
                         .FirstOrDefault(),
                     TargetFrameworkVersion = propertyGroup
                         .Elements(ProjectXElement.TargetFrameworkVersion)
                         .Select(targetFrameworkVersion => targetFrameworkVersion.Value)
                         .FirstOrDefault(),
                     TargetFrameworkProfile = propertyGroup
                         .Elements(ProjectXElement.TargetFrameworkProfile)
                         .Select(targetFrameworkProfile => targetFrameworkProfile.Value)
                         .FirstOrDefault()
                 }).FirstOrDefault();

            if (projectProperties == null)
            {
                throw new CakeException("Failed to parse project properties");
            }

            var rootPath = projectPath.GetDirectory();

            var projectFiles =
                (from project in document.Elements(ProjectXElement.Project)
                 from itemGroup in project.Elements(ProjectXElement.ItemGroup)
                 from element in itemGroup.Elements()
                 where element.Name != ProjectXElement.Reference &&
                       element.Name != ProjectXElement.Import &&
                       element.Name != ProjectXElement.BootstrapperPackage &&
                       element.Name != ProjectXElement.ProjectReference &&
                       element.Name != ProjectXElement.Service
                 from include in element.Attributes("Include")
                 let value = include.Value
                 where !string.IsNullOrEmpty(value)
                 let filePath = rootPath.CombineWithFilePath(value)
                 select new ProjectFile
                 {
                     FilePath = filePath,
                     RelativePath = value,
                     Compile = element.Name == ProjectXElement.Compile
                 }).ToArray();

            var references =
                (from project in document.Elements(ProjectXElement.Project)
                 from itemGroup in project.Elements(ProjectXElement.ItemGroup)
                 from element in itemGroup.Elements()
                 where element.Name == ProjectXElement.Reference
                 from include in element.Attributes("Include")
                 let includeValue = include.Value
                 let hintPathElement = element.Element(ProjectXElement.HintPath)
                 let nameElement = element.Element(ProjectXElement.Name)
                 let fusionNameElement = element.Element(ProjectXElement.FusionName)
                 let specificVersionElement = element.Element(ProjectXElement.SpecificVersion)
                 let aliasesElement = element.Element(ProjectXElement.Aliases)
                 let privateElement = element.Element(ProjectXElement.Private)
                 select new ProjectAssemblyReference
                 {
                     Include = includeValue,
                     HintPath = string.IsNullOrEmpty(hintPathElement?.Value)
                        ? null : rootPath.CombineWithFilePath(hintPathElement.Value),
                     Name = nameElement?.Value,
                     FusionName = fusionNameElement?.Value,
                     SpecificVersion = specificVersionElement == null ? (bool?)null : bool.Parse(specificVersionElement.Value),
                     Aliases = aliasesElement?.Value,
                     Private = privateElement == null ? (bool?)null : bool.Parse(privateElement.Value)
                 }).ToArray();

            var projectReferences =
                (from project in document.Elements(ProjectXElement.Project)
                 from itemGroup in project.Elements(ProjectXElement.ItemGroup)
                 from element in itemGroup.Elements()
                 where element.Name == ProjectXElement.ProjectReference
                 from include in element.Attributes("Include")
                 let value = include.Value
                 where !string.IsNullOrEmpty(value)
                 let filePath = rootPath.CombineWithFilePath(value)
                 let nameElement = element.Element(ProjectXElement.Name)
                 let projectElement = element.Element(ProjectXElement.Project)
                 let packageElement = element.Element(ProjectXElement.Package)
                 select new ProjectReference
                 {
                     FilePath = filePath,
                     RelativePath = value,
                     Name = nameElement?.Value,
                     Project = projectElement?.Value,
                     Package = string.IsNullOrEmpty(packageElement?.Value)
                        ? null : rootPath.CombineWithFilePath(packageElement.Value)
                 }).ToArray();

            return new ProjectParserResult(
                projectProperties.Configuration,
                projectProperties.Platform,
                projectProperties.ProjectGuid,
                projectProperties.OutputType,
                projectProperties.OutputPath,
                projectProperties.RootNameSpace,
                projectProperties.AssemblyName,
                projectProperties.TargetFrameworkVersion,
                projectProperties.TargetFrameworkProfile,
                projectFiles,
                references,
                projectReferences);
        }
    }
}