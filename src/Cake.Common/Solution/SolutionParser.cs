// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Solution
{
    /// <summary>
    /// The MSBuild solution file parser.
    /// </summary>
    public sealed class SolutionParser
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionParser"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        public SolutionParser(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);
            ArgumentNullException.ThrowIfNull(environment);
            _fileSystem = fileSystem;
            _environment = environment;
        }

        /// <summary>
        /// Parses a MSBuild solution.
        /// </summary>
        /// <param name="solutionPath">The solution path.</param>
        /// <returns>A parsed solution.</returns>
        public SolutionParserResult Parse(FilePath solutionPath)
        {
            ArgumentNullException.ThrowIfNull(solutionPath);
            if (solutionPath.IsRelative)
            {
                solutionPath = solutionPath.MakeAbsolute(_environment);
            }

            // Get solution file.
            var file = _fileSystem.GetFile(solutionPath);
            if (!file.Exists)
            {
                const string format = "Solution file '{0}' does not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, solutionPath.FullPath);
                throw new CakeException(message);
            }

            var fileExtension = file.Path.GetExtension().ToLowerInvariant();

            return fileExtension switch
            {
                ".sln" => ParseSlnSolution(file),
                ".slnx" => ParseSlnxSolution(file),
                _ => throw new CakeException($"Unknown file extension {fileExtension} for solution file '{solutionPath.FullPath}'."),
            };
        }

        private static SolutionParserResult ParseSlnSolution(IFile file)
        {
            string
                version = null,
                visualStudioVersion = null,
                minimumVisualStudioVersion = null;
            var projects = new List<SolutionProject>();
            bool inNestedProjectsSection = false;
            foreach (var line in file.ReadLines(Encoding.UTF8))
            {
                var trimmed = line.Trim();

                if (trimmed == string.Empty)
                {
                    continue;
                }

                if (line.StartsWith("Project(\"{"))
                {
                    var project = ParseSolutionProjectLine(file, line);
                    if (StringComparer.OrdinalIgnoreCase.Equals(project.Type, SolutionFolder.TypeIdentifier))
                    {
                        projects.Add(new SolutionFolder(project.Id, project.Name, project.Path));
                        continue;
                    }
                    projects.Add(project);
                }
                else if (line.StartsWith("Microsoft Visual Studio Solution File, "))
                {
                    version = string.Concat(line.Skip(39));
                }
                else if (line.StartsWith("VisualStudioVersion = "))
                {
                    visualStudioVersion = string.Concat(line.Skip(22));
                }
                else if (line.StartsWith("MinimumVisualStudioVersion = "))
                {
                    minimumVisualStudioVersion = string.Concat(line.Skip(29));
                }
                else if (trimmed.StartsWith("GlobalSection(NestedProjects)"))
                {
                    inNestedProjectsSection = true;
                }
                else if (inNestedProjectsSection && trimmed.StartsWith("EndGlobalSection"))
                {
                    inNestedProjectsSection = false;
                }
                else if (inNestedProjectsSection)
                {
                    ParseNestedProjectLine(projects, trimmed);
                }
            }
            var solutionParserResult = new SolutionParserResult(
                version,
                visualStudioVersion,
                minimumVisualStudioVersion,
                projects.AsReadOnly());
            return solutionParserResult;
        }

        private static SolutionProject ParseSolutionProjectLine(IFile file, string line)
        {
            var withinQuotes = false;
            var projectTypeBuilder = new StringBuilder();
            var nameBuilder = new StringBuilder();
            var pathBuilder = new StringBuilder();
            var idBuilder = new StringBuilder();
            var result = new[]
            {
                projectTypeBuilder,
                nameBuilder,
                pathBuilder,
                idBuilder
            };
            var position = 0;
            foreach (var c in line.Skip(8))
            {
                if (c == '"')
                {
                    withinQuotes = !withinQuotes;
                    if (!withinQuotes)
                    {
                        if (position++ >= result.Length)
                        {
                            break;
                        }
                    }
                    continue;
                }
                if (!withinQuotes)
                {
                    continue;
                }
                result[position].Append(c);
            }

            var projectPath = new FilePath(pathBuilder.ToString());

            var projectFullPath = projectPath.IsRelative ?
                file.Path.GetDirectory().CombineWithFilePath(projectPath) :
                projectPath;

            return new SolutionProject(
                idBuilder.ToString(),
                nameBuilder.ToString(),
                projectFullPath,
                projectTypeBuilder.ToString());
        }

        private static void ParseNestedProjectLine(List<SolutionProject> projects, string line)
        {
            // pattern: {Child} = {Parent}
            var projectIds = line.Split(new[] { " = " }, StringSplitOptions.RemoveEmptyEntries);
            var child = projects.FirstOrDefault(x => StringComparer.OrdinalIgnoreCase.Equals(x.Id, projectIds[0].Trim()));
            if (child == null)
            {
                return;
            }

            // Parent should be a folder
            var parent = projects.FirstOrDefault(x => StringComparer.OrdinalIgnoreCase.Equals(x.Id, projectIds[1].Trim())) as SolutionFolder;
            if (parent == null)
            {
                return;
            }

            parent.Items.Add(child);
            child.Parent = parent;
        }

        private static SolutionParserResult ParseSlnxSolution(IFile file)
        {
            var xmlReaderSettings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit, // Prevent XXE
                XmlResolver = null, // Disable external entity resolution
            };
            using var xmlContentStream = file.OpenRead();
            var xmlDocument = new XmlDocument();

            using var xmlReader = XmlReader.Create(xmlContentStream, xmlReaderSettings);
            xmlDocument.Load(xmlReader);

            var root = xmlDocument.DocumentElement;
            if (root == null)
            {
                throw new CakeException($"Solution file '{file.Path.FullPath}' does not contain a root element.");
            }

            var projects = new List<SolutionProject>();
            foreach (var xmlElement in root.ChildNodes.OfType<XmlElement>())
            {
                var projectsFromElement = ParseSlnxElement(xmlElement, file);
                projects.AddRange(projectsFromElement);
            }

            // the new SLNX has no information about the file format version, VS version and minimal VS version, so it's omitted.
            return new SolutionParserResult(string.Empty, string.Empty, string.Empty, projects.AsReadOnly());
        }

        private static List<SolutionProject> ParseSlnxElement(XmlElement xmlElement, IFile solutionFile)
        {
            return xmlElement.Name switch
            {
                "Folder" => ParseSlnxFolder(xmlElement, solutionFile),
                "Project" => ParseSlnxProject(xmlElement, solutionFile),
                _ =>[],
            };
        }

        private static List<SolutionProject> ParseSlnxFolder(XmlElement xmlElement, IFile solutionFile)
        {
            var folderName = xmlElement.GetAttribute("Name");
            if (string.IsNullOrWhiteSpace(folderName))
            {
                throw new CakeException($"Could not find solution folder name attribute in solution file '{solutionFile.Path.FullPath}'.");
            }

            // the name of the folder is also its path. And it always has a starting and trailing "/", which break the check if its relative, so they are removed.
            var folderPath = new FilePath(folderName.Trim('/'));
            var folderFullPath = folderPath.IsRelative ?
                solutionFile.Path.GetDirectory().CombineWithFilePath(folderPath) :
                folderPath;

            var actualFolderName = folderName.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();

            // elements in the SLNX file don't have any IDs anymore, so it's omitted.
            var solutionFolder = new SolutionFolder(string.Empty, actualFolderName, folderFullPath);
            var projects = new List<SolutionProject>
            {
                solutionFolder,
            };

            foreach (var childElement in xmlElement.ChildNodes.OfType<XmlElement>())
            {
                var childProjects = ParseSlnxElement(childElement, solutionFile);
                foreach (var childProject in childProjects)
                {
                    childProject.Parent = solutionFolder;
                    solutionFolder.Items.Add(childProject);
                    projects.Add(childProject);
                }
            }

            return projects;
        }

        private static List<SolutionProject> ParseSlnxProject(XmlElement xmlElement, IFile solutionFile)
        {
            const string defaultProjectTypeId = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";

            var projectPath = xmlElement.GetAttribute("Path");
            if (string.IsNullOrWhiteSpace(projectPath))
            {
                throw new CakeException($"Could not find solution project path attribute in solution file '{solutionFile.Path.FullPath}'.");
            }

            var projectTypeId = xmlElement.GetAttribute("Type");
            projectTypeId = string.IsNullOrWhiteSpace(projectTypeId)
                ? defaultProjectTypeId
                // the new project type id notation does not quite fit the known format, so it is adjusted.
                : $"{{{projectTypeId.ToUpper()}}}";

            var projectFilePath = new FilePath(projectPath);
            var projectFileFullPath = projectFilePath.IsRelative ?
                solutionFile.Path.GetDirectory().CombineWithFilePath(projectFilePath) :
                projectFilePath;

            // the project name is part of its path.
            var actualProjectName = projectFilePath.GetFilenameWithoutExtension().FullPath;

            // elements in the SLNX file don't have any IDs anymore, so it's omitted.
            var solutionProject = new SolutionProject(string.Empty, actualProjectName, projectFileFullPath, projectTypeId);
            return
            [
                solutionProject,
            ];
        }
    }
}