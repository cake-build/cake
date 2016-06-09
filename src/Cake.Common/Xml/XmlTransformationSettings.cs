// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Text;
using System.Xml;

namespace Cake.Common.Xml
{
    /// <summary>
    /// Contains settings for <see cref="XmlTransformation"/>
    /// </summary>
    public sealed class XmlTransformationSettings
    {
        internal XmlWriterSettings XmlWriterSettings { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether overwriting existing file is permitted
        /// </summary>
        public bool Overwrite { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the XML writer should check to ensure that all characters in the document conform to the "2.2 Characters" section of the W3C XML 1.0 Recommendation.
        /// </summary>
        public bool CheckCharacters
        {
            get { return XmlWriterSettings.CheckCharacters; }
            set { XmlWriterSettings.CheckCharacters = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating level of conformance that the XmlWriter checks the XML output for.
        /// </summary>
        public ConformanceLevel ConformanceLevel
        {
            get { return XmlWriterSettings.ConformanceLevel; }
            set { XmlWriterSettings.ConformanceLevel = value; }
        }

        #if !__MonoCS__
        /// <summary>
        /// Gets or sets a value indicating whether the XmlWriter does not escape URI attributes.
        /// </summary>
        public bool DoNotEscapeUriAttributes
        {
            get { return XmlWriterSettings.DoNotEscapeUriAttributes; }
            set { XmlWriterSettings.DoNotEscapeUriAttributes = value; }
        }
        #endif

        /// <summary>
        /// Gets or sets the type of text encoding to use.
        /// </summary>
        public Encoding Encoding
        {
            get { return XmlWriterSettings.Encoding; }
            set { XmlWriterSettings.Encoding = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to indent elements.
        /// </summary>
        public bool Indent
        {
            get { return XmlWriterSettings.Indent; }
            set { XmlWriterSettings.Indent = value; }
        }

        /// <summary>
        /// Gets or sets the character string to use when indenting. This setting is used when the Indent property is set to true.
        /// </summary>
        public string IndentChars
        {
            get { return XmlWriterSettings.IndentChars; }
            set { XmlWriterSettings.IndentChars = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the XmlWriter should remove duplicate namespace declarations when writing XML content. The default behavior is for the writer to output all namespace declarations that are present in the writer's namespace resolver.
        /// </summary>
        public NamespaceHandling NamespaceHandling
        {
            get { return XmlWriterSettings.NamespaceHandling; }
            set { XmlWriterSettings.NamespaceHandling = value; }
        }

        /// <summary>
        /// Gets or sets the character string to use for line breaks.
        /// </summary>
        public string NewLineChars
        {
            get { return XmlWriterSettings.NewLineChars; }
            set { XmlWriterSettings.NewLineChars = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to normalize line breaks in the output.
        /// </summary>
        public NewLineHandling NewLineHandling
        {
            get { return XmlWriterSettings.NewLineHandling; }
            set { XmlWriterSettings.NewLineHandling = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to write attributes on a new line.
        /// </summary>
        public bool NewLineOnAttributes
        {
            get { return XmlWriterSettings.NewLineOnAttributes; }
            set { XmlWriterSettings.NewLineOnAttributes = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to omit an XML declaration.
        /// </summary>
        public bool OmitXmlDeclaration
        {
            get { return XmlWriterSettings.OmitXmlDeclaration; }
            set { XmlWriterSettings.OmitXmlDeclaration = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the XmlWriter will add closing tags to all unclosed element tags when the Close method is called
        /// </summary>
        public bool WriteEndDocumentOnClose
        {
            get { return XmlWriterSettings.WriteEndDocumentOnClose; }
            set { XmlWriterSettings.WriteEndDocumentOnClose = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlTransformationSettings"/> class.
        /// </summary>
        public XmlTransformationSettings()
        {
            XmlWriterSettings = new XmlWriterSettings();
            Overwrite = true;
        }
    }
}
