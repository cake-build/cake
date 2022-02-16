// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Solution.Project.XmlDoc
{
    /// <summary>
    /// The MSBuild Xml documentation example code parser.
    /// </summary>
    public sealed class XmlDocExampleCodeParser
    {
        private readonly IFileSystem _fileSystem;
        private readonly IGlobber _globber;
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDocExampleCodeParser"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="log">The log.</param>
        public XmlDocExampleCodeParser(IFileSystem fileSystem, IGlobber globber, ICakeLog log)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }

            if (globber == null)
            {
                throw new ArgumentNullException(nameof(globber));
            }

            _fileSystem = fileSystem;
            _globber = globber;
            _log = log;
        }

        /// <summary>
        /// Parses Xml documentation example code from given path.
        /// </summary>
        /// <param name="xmlFilePath">Path to the file to parse.</param>
        /// <returns>Parsed Example Code.</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public IEnumerable<XmlDocExampleCode> Parse(FilePath xmlFilePath)
        {
            if (xmlFilePath == null)
            {
                throw new ArgumentNullException(nameof(xmlFilePath), "Invalid xml file path supplied.");
            }

            var xmlFile = _fileSystem.GetFile(xmlFilePath);
            if (!xmlFile.Exists)
            {
                throw new FileNotFoundException("Supplied xml file not found.", xmlFilePath.FullPath);
            }

            using (var xmlStream = xmlFile.OpenRead())
            {
                using (var xmlReader = XmlReader.Create(xmlStream))
                {
                    return (
                        from doc in XDocument.Load(xmlReader).Elements("doc")
                        from members in doc.Elements("members")
                        from member in members.Elements("member")
                        from example in member.Elements("example")
                        from code in example.Elements("code")
                        let cleanedCode = string.Join("\r\n",
                            code.Value.Split('\r', '\n')
                                .Where(line => !string.IsNullOrWhiteSpace(line)))
                        select new XmlDocExampleCode(
                                member.Attributes("name").Select(name => name.Value).FirstOrDefault(),
                                cleanedCode)).ToArray();
                }
            }
        }

        /// <summary>
        /// Parses Xml documentation example code from file(s) using given pattern.
        /// </summary>
        /// <param name="pattern">The globber file pattern.</param>
        /// <returns>Parsed Example Code.</returns>
        public IEnumerable<XmlDocExampleCode> ParseFiles(GlobPattern pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern?.Pattern))
            {
                throw new ArgumentNullException(nameof(pattern), "Invalid pattern supplied.");
            }

            var files = _globber.GetFiles(pattern).ToArray();
            if (files.Length == 0)
            {
                _log.Verbose("The provided pattern did not match any files.");
                return Enumerable.Empty<XmlDocExampleCode>();
            }

            return files.SelectMany(Parse);
        }
    }
}