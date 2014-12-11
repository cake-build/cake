using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Solution
{
    /// <summary>
    /// The MSBuild Solution File Parser
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
        /// Parses solution project files
        /// </summary>
        /// <param name="solutionPath"></param>
        /// <returns></returns>
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

            var solutionParserResult = new SolutionParserResult(
                file
                    .ReadLines(Encoding.UTF8)
                    .Where(line => line.StartsWith("Project(\"{"))
                    .Select(
                        line =>
                        {
                            var withinQuotes = false;
                            StringBuilder
                                projectTypeBuilder = new StringBuilder(),
                                nameBuilder = new StringBuilder(),
                                pathBuilder = new StringBuilder(),
                                idBuilder = new StringBuilder();
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
                                            break;
                                    }
                                    continue;
                                }

                                if (!withinQuotes) continue;

                                result[position].Append(c);
                            }

                            return new SolutionProject(
                                idBuilder.ToString(),
                                nameBuilder.ToString(),
                                file
                                    .Path
                                    .GetDirectory()
                                    .CombineWithFilePath(pathBuilder.ToString()),
                                projectTypeBuilder.ToString()
                                );
                        }
                    )
                    //Exclude solution folder
                    .Where(project => !StringComparer.OrdinalIgnoreCase.Equals(project.Type, SolutionFolder))
                );

            return solutionParserResult;
        }
    }
}
