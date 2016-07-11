// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Xml
{
    /// <summary>
    /// Provides functionality to perform XML transformation
    /// </summary>
    public static class XmlTransformation
    {
        /// <summary>
        /// Performs XML XSL transformation
        /// </summary>
        /// <param name="xsl">XML style sheet.</param>
        /// <param name="xml">XML data.</param>
        /// <returns>Transformed XML string.</returns>
        public static string Transform(string xsl, string xml)
        {
            var settings = new XmlTransformationSettings
            {
                OmitXmlDeclaration = true,
                Encoding = new UTF8Encoding(false)
            };

            return Transform(xsl, xml, settings);
        }

        /// <summary>
        /// Performs XML XSL transformation
        /// </summary>
        /// <param name="xsl">XML style sheet.</param>
        /// <param name="xml">XML data.</param>
        /// <param name="settings">Settings for result file xml transformation.</param>
        /// <returns>Transformed XML string.</returns>
        public static string Transform(string xsl, string xml, XmlTransformationSettings settings)
        {
            if (string.IsNullOrWhiteSpace(xsl))
            {
                throw new ArgumentNullException("xsl", "Null or empty XML style sheet supplied.");
            }

            if (string.IsNullOrWhiteSpace(xml))
            {
                throw new ArgumentNullException("xml", "Null or empty XML data supplied.");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings", "Null settings supplied.");
            }

            using (TextReader
                xslReader = new StringReader(xsl),
                xmlReader = new StringReader(xml))
            {
                using (var result = new MemoryStream())
                {
                    Transform(xslReader, xmlReader, result, settings.XmlWriterSettings);
                    result.Position = 0;
                    return settings.Encoding.GetString(result.ToArray());
                }
            }
        }

        /// <summary>
        /// Performs XML XSL transformation
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="xslPath">Path to xml style sheet.</param>
        /// <param name="xmlPath">Path xml data.</param>
        /// <param name="resultPath">Transformation result path.</param>
        public static void Transform(IFileSystem fileSystem, FilePath xslPath, FilePath xmlPath, FilePath resultPath)
        {
            var settings = new XmlTransformationSettings();
            Transform(fileSystem, xslPath, xmlPath, resultPath, settings);
        }

        /// <summary>
        /// Performs XML XSL transformation
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="xslPath">Path to xml style sheet.</param>
        /// <param name="xmlPath">Path xml data.</param>
        /// <param name="resultPath">Transformation result path.</param>
        /// <param name="settings">Settings for result file xml transformation.</param>
        public static void Transform(IFileSystem fileSystem, FilePath xslPath, FilePath xmlPath, FilePath resultPath, XmlTransformationSettings settings)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem", "Null filesystem supplied.");
            }

            if (xslPath == null)
            {
                throw new ArgumentNullException("xslPath", "Null XML style sheet path supplied.");
            }

            if (xmlPath == null)
            {
                throw new ArgumentNullException("xmlPath", "Null XML data path supplied.");
            }

            if (resultPath == null)
            {
                throw new ArgumentNullException("resultPath", "Null result path supplied.");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings", "Null settings supplied.");
            }

            IFile
                xslFile = fileSystem.GetFile(xslPath),
                xmlFile = fileSystem.GetFile(xmlPath),
                resultFile = fileSystem.GetFile(resultPath);

            if (!xslFile.Exists)
            {
                throw new FileNotFoundException("Xsl File not found.", xslFile.Path.FullPath);
            }

            if (!xmlFile.Exists)
            {
                throw new FileNotFoundException("XML File not found.", xmlFile.Path.FullPath);
            }

            if (!settings.Overwrite && resultFile.Exists)
            {
                throw new CakeException("Result file found and overwrite set to false.");
            }

            using (Stream
                xslStream = xslFile.OpenRead(),
                xmlStream = xmlFile.OpenRead(),
                resultStream = resultFile.OpenWrite())
            {
                XmlReader
                    xslReader = XmlReader.Create(xslStream),
                    xmlReader = XmlReader.Create(xmlStream);

                var resultWriter = XmlWriter.Create(resultStream, settings.XmlWriterSettings);

                Transform(xslReader, xmlReader, resultWriter);
            }
        }

        /// <summary>
        /// Performs XML XSL transformation
        /// </summary>
        /// <param name="xsl">XML style sheet.</param>
        /// <param name="xml">XML data.</param>
        /// <param name="result">Transformation result.</param>
        /// <param name="settings">Optional settings for result file xml writer</param>
        private static void Transform(TextReader xsl, TextReader xml, Stream result, XmlWriterSettings settings = null)
        {
            if (xsl == null)
            {
                throw new ArgumentNullException("xsl", "Null XML style sheet supplied.");
            }

            if (xml == null)
            {
                throw new ArgumentNullException("xml", "Null XML data supplied.");
            }

            if (result == null)
            {
                throw new ArgumentNullException("result", "Null result supplied.");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings", "Null settings supplied.");
            }

            var xslXmlReader = XmlReader.Create(xsl);
            var xmlXmlReader = XmlReader.Create(xml);
            var resultXmlTextWriter = XmlWriter.Create(result, settings);
            Transform(xslXmlReader, xmlXmlReader, resultXmlTextWriter);
        }

        /// <summary>
        /// Performs XML XSL transformation
        /// </summary>
        /// <param name="xsl">XML style sheet.</param>
        /// <param name="xml">XML data.</param>
        /// <param name="result">Transformation result.</param>
        private static void Transform(XmlReader xsl, XmlReader xml, XmlWriter result)
        {
            if (xsl == null)
            {
                throw new ArgumentNullException("xsl", "Null XML style sheet supplied.");
            }

            if (xml == null)
            {
                throw new ArgumentNullException("xml", "Null XML data supplied.");
            }

            if (result == null)
            {
                throw new ArgumentNullException("result", "Null result supplied.");
            }

            var xslTransform = new XslCompiledTransform();
            xslTransform.Load(xsl);
            xslTransform.Transform(xml, result);
        }
    }
}
