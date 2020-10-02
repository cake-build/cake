// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Solution.Project.Properties
{
    /// <summary>
    /// The assembly info creator.
    /// </summary>
    public sealed class AssemblyInfoCreator
    {
        private const string CSharpComment = "//";
        private const string CSharpUsingFormat = "using {0};";
        private const string CSharpAttributeFormat = "[assembly: {0}]";
        private const string CSharpAttributeWithValueFormat = "[assembly: {0}({1})]";
        private const string CSharpAttributeWithKeyValueFormat = "[assembly: {0}({1}, {2})]";
        private const string VBComment = "'";
        private const string VBUsingFormat = "Imports {0}";
        private const string VBAttributeFormat = "<Assembly: {0}>";
        private const string VBAttributeWithValueFormat = "<Assembly: {0}({1})>";
        private const string VBAttributeWithKeyValueFormat = "<Assembly: {0}({1}, {2})>";

        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfoCreator"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        public AssemblyInfoCreator(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }
            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;
        }

        /// <summary>
        /// Creates an assembly info file.
        /// </summary>
        /// <param name="outputPath">The output path.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="attributeFormat">The attribute format.</param>
        /// <param name="attributeWithValueFormat">The attribute with value format.</param>
        /// <param name="attributeWithKeyValueFormat">The attribute with key value format.</param>
        /// <param name="vbAttributeFormat">The VB attribute format.</param>
        /// <param name="vbAttributeWithValueFormat">The VB attribute with value format.</param>
        /// <param name="vbAttributeWithKeyValueFormat">The VB attribute with key value format.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public void Create(FilePath outputPath, AssemblyInfoSettings settings,
            string attributeFormat = CSharpAttributeFormat,
            string attributeWithValueFormat = CSharpAttributeWithValueFormat,
            string attributeWithKeyValueFormat = CSharpAttributeWithKeyValueFormat,
            string vbAttributeFormat = VBAttributeFormat,
            string vbAttributeWithValueFormat = VBAttributeWithValueFormat,
            string vbAttributeWithKeyValueFormat = VBAttributeWithKeyValueFormat)
        {
            if (outputPath == null)
            {
                throw new ArgumentNullException(nameof(outputPath));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            string comment = CSharpComment;
            string usingFormat = CSharpUsingFormat;

            var isVisualBasicAssemblyInfoFile = false;

            if (outputPath.GetExtension() == ".vb")
            {
                isVisualBasicAssemblyInfoFile = true;
                comment = VBComment;
                usingFormat = VBUsingFormat;
                attributeFormat = vbAttributeFormat;
                attributeWithValueFormat = vbAttributeWithValueFormat;
                attributeWithKeyValueFormat = vbAttributeWithKeyValueFormat;
            }

            var data = new AssemblyInfoCreatorData(settings, isVisualBasicAssemblyInfoFile);

            outputPath = outputPath.MakeAbsolute(_environment);
            _log.Verbose("Creating assembly info file: {0}", outputPath);

            using (var stream = _fileSystem.GetFile(outputPath).OpenWrite())
            using (var writer = new StreamWriter(stream, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(comment + "------------------------------------------------------------------------------");
                writer.WriteLine(comment + " <auto-generated>");
                writer.WriteLine(comment + "     This code was generated by Cake.");
                writer.WriteLine(comment + " </auto-generated>");
                writer.WriteLine(comment + "------------------------------------------------------------------------------");

                if (data.Namespaces.Count > 0)
                {
                    var namespaces = data.Namespaces.Select(n => string.Format(usingFormat, n));
                    foreach (var @namespace in namespaces)
                    {
                        writer.WriteLine(@namespace);
                    }
                    writer.WriteLine();
                }

                if (data.Attributes.Count > 0)
                {
                    foreach (var attribute in data.Attributes)
                    {
                        writer.WriteLine(string.Format(attributeWithValueFormat, attribute.Key, attribute.Value));
                    }
                    writer.WriteLine();
                }

                if (data.InternalVisibleTo.Count > 0)
                {
                    foreach (var temp in data.InternalVisibleTo)
                    {
                        writer.WriteLine(string.Format(attributeFormat, temp));
                    }
                    writer.WriteLine();
                }

                if (data.CustomAttributes.Count > 0)
                {
                    writer.WriteLine(comment + " Custom Attributes");
                    foreach (var attribute in data.CustomAttributes)
                    {
                        writer.WriteLine(string.Format(attributeWithValueFormat, attribute.Key, attribute.Value));
                    }
                }

                if (data.MetadataAttributes.Count > 0)
                {
                    writer.WriteLine(comment + " Metadata Attributes");
                    var mdAttribute = new AssemblyInfoMetadataAttribute();
                    foreach (var attribute in data.MetadataAttributes)
                    {
                        writer.WriteLine(string.Format(attributeWithKeyValueFormat, mdAttribute.Name, attribute.Key, attribute.Value));
                    }
                }
            }
        }
    }
}