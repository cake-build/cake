﻿using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Solution.Project.XmlDoc
{
    /// <summary>
    /// Contains functionality related to MSBuild XML document files.
    /// </summary>
    [CakeAliasCategory("MSBuild Resource")]
    public static class XmlDocAliases
    {
        /// <summary>
        /// Parses Xml documentation example code from given path.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="xmlFilePath">The Path to the file to parse.</param>
        /// <returns>Parsed example code.</returns>
        /// <example>
        /// <code>
        /// var exampleCodes = ParseXmlDocExampleCode("./Cake.Common.xml");
        /// foreach(var exampleCode in exampleCodes)
        /// {
        ///     Information(
        ///         "{0}\r\n{1}",
        ///         exampleCode.Name,
        ///         exampleCode.Code
        ///     );
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static IEnumerable<XmlDocExampleCode> ParseXmlDocExampleCode(this ICakeContext context, FilePath xmlFilePath)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (xmlFilePath == null)
            {
                throw new ArgumentNullException("xmlFilePath");
            }

            var parser = new XmlDocExampleCodeParser(context.FileSystem, context.Globber, context.Log);
            return parser.Parse(xmlFilePath);
        }

        /// <summary>
        /// Parses Xml documentation example code from file(s) using given pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The globber file pattern.</param>
        /// <returns>Parsed example code.</returns>
        /// <example>
        /// <code>
        /// var filesExampleCodes = ParseXmlDocFilesExampleCode("./Cake.*.xml");
        /// foreach(var exampleCode in filesExampleCodes)
        /// {
        ///     Information(
        ///         "{0}\r\n{1}",
        ///         exampleCode.Name,
        ///         exampleCode.Code
        ///     );
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static IEnumerable<XmlDocExampleCode> ParseXmlDocFilesExampleCode(this ICakeContext context, string pattern)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new ArgumentNullException("pattern");
            }

            var parser = new XmlDocExampleCodeParser(context.FileSystem, context.Globber, context.Log);
            return parser.ParseFiles(pattern);
        }
    }
}
