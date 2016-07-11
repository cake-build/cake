// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
        private const string NonQuotedPattern = @"^\s*\[assembly: {0} ?\((?<attributeValue>.*)\)";
        private const string QuotedPattern = @"^\s*\[assembly: {0} ?\(""(?<attributeValue>.*)""\)";
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
                    ParseSingle(NonQuotedPattern, "CLSCompliant", content),
                    ParseSingle(QuotedPattern, "AssemblyCompany", content),
                    ParseSingle(NonQuotedPattern, "ComVisible", content),
                    ParseSingle(QuotedPattern, "AssemblyConfiguration", content),
                    ParseSingle(QuotedPattern, "AssemblyCopyright", content),
                    ParseSingle(QuotedPattern, "AssemblyDescription", content),
                    ParseSingle(QuotedPattern, "AssemblyFileVersion", content) ?? DefaultVersion,
                    ParseSingle(QuotedPattern, "Guid", content),
                    ParseSingle(QuotedPattern, "AssemblyInformationalVersion", content) ?? DefaultVersion,
                    ParseSingle(QuotedPattern, "AssemblyProduct", content),
                    ParseSingle(QuotedPattern, "AssemblyTitle", content),
                    ParseSingle(QuotedPattern, "AssemblyTrademark", content),
                    ParseSingle(QuotedPattern, "AssemblyVersion", content) ?? DefaultVersion,
                    ParseMultiple(QuotedPattern, "InternalsVisibleTo", content));
            }
        }

        private static string ParseSingle(string pattern, string attributeName, string content)
        {
            return ParseMultiple(pattern, attributeName, content).SingleOrDefault();
        }

        private static IEnumerable<string> ParseMultiple(string pattern, string attributeName, string content)
        {
            var regex = new Regex(string.Format(CultureInfo.InvariantCulture, pattern, attributeName), RegexOptions.Multiline);
            foreach (Match match in regex.Matches(content))
            {
                if (match.Groups.Count > 0)
                {
                    var value = match.Groups["attributeValue"].Value;
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        yield return value;
                    }
                }
            }
        }
    }
}
