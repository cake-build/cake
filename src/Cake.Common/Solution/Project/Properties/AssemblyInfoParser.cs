using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Solution.Project.Properties
{
    /// <summary>
    /// The assembly info parser.
    /// </summary>
    public sealed class AssemblyInfoParser
    {
        private const string VersionPattern = @"^\s*\[assembly: {0}\(""([.]*(\d*|\*?)[.]*(\d*|\*?)[.]*(\d*|\*?)[.]*(\d*|\*?))""\)";
        private const string GeneralNonQuotedAttributePattern = @"^\s*\[assembly: {0}\((?<attributeValue>.*)\)";
        private const string GeneralQuotedAttributePattern = @"^\s*\[assembly: {0}\(""(?<attributeValue>.*)""\)";
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
                const string format = "Assembly info file '{0}' does not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, assemblyInfoPath.FullPath);
                throw new CakeException(message);
            }

            using (var reader = new StreamReader(file.OpenRead()))
            {
                var content = reader.ReadToEnd();
                return new AssemblyInfoParseResult(
                    ParseGeneralNonQuotedAttribute("CLSCompliant", content),
                    ParseGeneralQuotedAttribute("AssemblyCompany", content),
                    ParseGeneralNonQuotedAttribute("ComVisible", content),
                    ParseGeneralQuotedAttribute("AssemblyConfiguration", content),
                    ParseGeneralQuotedAttribute("AssemblyCopyright", content),
                    ParseGeneralQuotedAttribute("AssemblyDescription", content),
                    ParseVersion("AssemblyFileVersion", content),
                    ParseGeneralQuotedAttribute("Guid", content),
                    ParseVersion("AssemblyInformationalVersion", content),
                    ParseGeneralQuotedAttribute("InternalsVisibleTo", content),
                    ParseGeneralQuotedAttribute("AssemblyProduct", content),
                    ParseGeneralQuotedAttribute("AssemblyTitle", content),
                    ParseGeneralQuotedAttribute("AssemblyTrademark", content),
                    ParseVersion("AssemblyVersion", content));
            }
        }

        private static string ParseVersion(string attributeName, string content)
        {
            var regex = new Regex(string.Format(CultureInfo.InvariantCulture, VersionPattern, attributeName), RegexOptions.Multiline);
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

        private static string ParseGeneralQuotedAttribute(string attributeName, string content)
        {
            var regex = new Regex(string.Format(CultureInfo.InvariantCulture, GeneralQuotedAttributePattern, attributeName), RegexOptions.Multiline);
            var match = regex.Match(content);
            if (match.Groups.Count > 0)
            {
                var value = match.Groups["attributeValue"].Value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }
            return string.Empty;
        }

        private static string ParseGeneralNonQuotedAttribute(string attributeName, string content)
        {
            var regex = new Regex(string.Format(CultureInfo.InvariantCulture, GeneralNonQuotedAttributePattern, attributeName), RegexOptions.Multiline);
            var match = regex.Match(content);
            if (match.Groups.Count > 0)
            {
                var value = match.Groups["attributeValue"].Value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }
            return string.Empty;
        }
    }
}