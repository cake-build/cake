// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Solution
{
    /// <summary>
    /// The MSBuild solution file parser.
    /// </summary>
    public sealed class SolutionParser
    {
        private const string SolutionFolder = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}";
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionParser"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        public SolutionParser(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
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
            if (solutionPath == null)
            {
                throw new ArgumentNullException("solutionPath");
            }

            if (solutionPath.IsRelative)
            {
                solutionPath = solutionPath.MakeAbsolute(_environment);
            }

            // Get the release notes file.
            var file = _fileSystem.GetFile(solutionPath);
            if (!file.Exists)
            {
                const string format = "Solution file '{0}' do not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, solutionPath.FullPath);
                throw new CakeException(message);
            }

            string
                version = null,
                visualStudioVersion = null,
                minimumVisualStudioVersion = null;
            var projects = new List<SolutionProject>();

            foreach (var line in file.ReadLines(Encoding.UTF8))
            {
                if (line.StartsWith("Project(\"{"))
                {
                    var project = ParseSolutionProjectLine(file, line);
                    if (!StringComparer.OrdinalIgnoreCase.Equals(project.Type, SolutionFolder))
                    {
                        projects.Add(project);
                    }
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

            return new SolutionProject(
                idBuilder.ToString(),
                nameBuilder.ToString(),
                file.Path.GetDirectory().CombineWithFilePath(pathBuilder.ToString()),
                projectTypeBuilder.ToString());
        }
    }
}
