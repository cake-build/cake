using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Solution.Project.Properties
{
    /// <summary>
    /// The assembly info creator.
    /// </summary>
    public class AssemblyInfoParser
    {
        private const string Pattern = @"{0}\(""([.]*(\d*|\*?)[.]*(\d*|\*?)[.]*(\d*|\*?)[.]*(\d*|\*?))""\)";
        private const string DefaultVersion = "1.0.0.0";

        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfoParser"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        public AssemblyInfoParser(IFileSystem fileSystem, ICakeEnvironment environment)
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
        /// Parses information from an assembly info file.
        /// </summary>
        /// <param name="assemblyInfoPath">The file path.</param>
        /// <returns>Information about the assembly info content.</returns>
        public AssemblyInfoParseResult Parse(FilePath assemblyInfoPath)
        {
            if (assemblyInfoPath == null)
            {
                throw new ArgumentNullException("assemblyInfoPath");
            }

            if (assemblyInfoPath.IsRelative)
            {
                assemblyInfoPath = assemblyInfoPath.MakeAbsolute(_environment);
            }

            // Get the release notes file.
            var file = _fileSystem.GetFile(assemblyInfoPath);
            if (!file.Exists)
            {
                const string format = "Assembly info file '{0}' do not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, assemblyInfoPath.FullPath);
                throw new CakeException(message);
            }

            using (var reader = new StreamReader(file.OpenRead()))
            {
                var content = reader.ReadToEnd();
                return new AssemblyInfoParseResult(
                    ParseVersion("AssemblyVersion", content),
                    ParseVersion("AssemblyFileVersion", content),
                    ParseVersion("AssemblyInformationalVersion", content));
            }
        }

        private static string ParseVersion(string attributeName, string content)
        {
            var regex = new Regex(string.Format(CultureInfo.InvariantCulture, Pattern, attributeName));
            var match = regex.Match(content);
            if (match.Groups.Count > 0)
            {
                var value = match.Groups[1].Value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }   
            }
            return DefaultVersion;
        }
    }
}
