// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.IO;
using System.Xml;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Xml
{
    /// <summary>
    /// Contains functionality related to XML XSL transformation.
    /// </summary>
    [CakeAliasCategory("XML")]
    public static class XmlPokeAliases
    {
        /// <summary>
        /// Set the value of, or remove, target nodes.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The target file.</param>
        /// <param name="xpath">The xpath of the nodes to set.</param>
        /// <param name="value">The value to set too. Leave blank to remove the selected nodes.</param>
        /// <example>
        ///   <para>
        ///   Change the <c>server</c> setting in the configuration from <c>testhost.somecompany.com</c>
        ///   to <c>productionhost.somecompany.com</c>.
        ///   </para>
        ///   <para>XML file:</para>
        ///   <code>
        ///     <![CDATA[
        /// <?xml version="1.0" encoding="utf-8" ?>
        /// <configuration>
        ///     <appSettings>
        ///         <add key="server" value="testhost.somecompany.com" />
        ///     </appSettings>
        /// </configuration>
        ///     ]]>
        ///   </code>
        ///   <para>Cake Task:</para>
        ///   <code>
        ///     <![CDATA[
        /// Task("Transform")
        ///     .Does(() =>
        /// {
        ///     var file = File("test.xml");
        ///     XmlPoke(file, "/configuration/appSettings/add[@key = 'server']/@value", "productionhost.somecompany.com");
        /// });
        ///     ]]>
        ///   </code>
        /// </example>
        /// <example>
        ///   <para>
        ///   Modify the <c>noNamespaceSchemaLocation</c> in an XML file.
        ///   </para>
        ///   <para>XML file:</para>
        ///   <code>
        ///     <![CDATA[
        /// <?xml version="1.0" encoding="utf-8" ?>
        /// <Commands xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="Path Value">
        /// </Commands>
        ///     ]]>
        ///   </code>
        ///   <para>Cake Task:</para>
        ///   <code>
        ///     <![CDATA[
        /// Task("Transform")
        ///     .Does(() =>
        /// {
        ///     var file = File("test.xml");
        ///     XmlPoke(file, "/Commands/@xsi:noNamespaceSchemaLocation", "d:\\Commands.xsd", new XmlPokeSettings {
        ///         Namespaces = new Dictionary<string, string> {
        ///             { /* Prefix */ "xsi", /* URI */ "http://www.w3.org/2001/XMLSchema-instance" }
        ///         }
        ///     });
        /// });
        ///     ]]>
        ///   </code>
        /// </example>
        /// <example>
        ///   <para>
        ///   Remove an app setting from a config file.
        ///   </para>
        ///   <para>XML file:</para>
        ///   <code>
        ///     <![CDATA[
        /// <?xml version="1.0" encoding="utf-8" ?>
        /// <configuration>
        ///     <appSettings>
        ///         <add key="server" value="testhost.somecompany.com" />
        ///         <add key="testing" value="true" />
        ///     </appSettings>
        /// </configuration>
        ///     ]]>
        ///   </code>
        ///   <para>Cake Task:</para>
        ///   <code>
        ///     <![CDATA[
        /// Task("Transform")
        ///     .Does(() =>
        /// {
        ///     var file = File("test.xml");
        ///     XmlPoke(file, "/configuration/appSettings/add[@testing]", null);
        /// });
        ///     ]]>
        ///   </code>
        /// </example>
        /// <para>
        /// Credit to NAnt for the original example.
        /// http://nant.sourceforge.net/release/latest/help/tasks/xmlpoke.html.
        /// </para>
        [CakeMethodAlias]
        public static void XmlPoke(this ICakeContext context, FilePath filePath, string xpath, string value)
        {
            context.XmlPoke(filePath, xpath, value, new XmlPokeSettings());
        }

        /// <summary>
        /// Set the value of, or remove, target nodes.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The target file.</param>
        /// <param name="xpath">The xpath of the nodes to set.</param>
        /// <param name="value">The value to set too. Leave blank to remove the selected nodes.</param>
        /// <param name="settings">Additional settings to tweak Xml Poke behavior.</param>
        /// <example>
        ///   <para>
        ///   Change the <c>server</c> setting in the configuration from <c>testhost.somecompany.com</c>
        ///   to <c>productionhost.somecompany.com</c>.
        ///   </para>
        ///   <para>XML file:</para>
        ///   <code>
        ///     <![CDATA[
        /// <?xml version="1.0" encoding="utf-8" ?>
        /// <configuration>
        ///     <appSettings>
        ///         <add key="server" value="testhost.somecompany.com" />
        ///     </appSettings>
        /// </configuration>
        ///     ]]>
        ///   </code>
        ///   <para>Cake Task:</para>
        ///   <code>
        ///     <![CDATA[
        /// Task("Transform")
        ///     .Does(() =>
        /// {
        ///     var file = File("test.xml");
        ///     XmlPoke(file, "/configuration/appSettings/add[@key = 'server']/@value", "productionhost.somecompany.com");
        /// });
        ///     ]]>
        ///   </code>
        /// </example>
        /// <example>
        ///   <para>
        ///   Modify the <c>noNamespaceSchemaLocation</c> in an XML file.
        ///   </para>
        ///   <para>XML file:</para>
        ///   <code>
        ///     <![CDATA[
        /// <?xml version="1.0" encoding="utf-8" ?>
        /// <Commands xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="Path Value">
        /// </Commands>
        ///     ]]>
        ///   </code>
        ///   <para>Cake Task:</para>
        ///   <code>
        ///     <![CDATA[
        /// Task("Transform")
        ///     .Does(() =>
        /// {
        ///     var file = File("test.xml");
        ///     XmlPoke(file, "/Commands/@xsi:noNamespaceSchemaLocation", "d:\\Commands.xsd", new XmlPokeSettings {
        ///         Namespaces = new Dictionary<string, string> {
        ///             { /* Prefix */ "xsi", /* URI */ "http://www.w3.org/2001/XMLSchema-instance" }
        ///         }
        ///     });
        /// });
        ///     ]]>
        ///   </code>
        /// </example>
        /// <example>
        ///   <para>
        ///   Remove an app setting from a config file.
        ///   </para>
        ///   <para>XML file:</para>
        ///   <code>
        ///     <![CDATA[
        /// <?xml version="1.0" encoding="utf-8" ?>
        /// <configuration>
        ///     <appSettings>
        ///         <add key="server" value="testhost.somecompany.com" />
        ///         <add key="testing" value="true" />
        ///     </appSettings>
        /// </configuration>
        ///     ]]>
        ///   </code>
        ///   <para>Cake Task:</para>
        ///   <code>
        ///     <![CDATA[
        /// Task("Transform")
        ///     .Does(() =>
        /// {
        ///     var file = File("test.xml");
        ///     XmlPoke(file, "/configuration/appSettings/add[@testing]", null);
        /// });
        ///     ]]>
        ///   </code>
        /// </example>
        /// <para>
        /// Credit to NAnt for the original example.
        /// http://nant.sourceforge.net/release/latest/help/tasks/xmlpoke.html.
        /// </para>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        [CakeMethodAlias]
        public static void XmlPoke(this ICakeContext context, FilePath filePath, string xpath, string value, XmlPokeSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var file = context.FileSystem.GetFile(filePath);
            if (!file.Exists)
            {
                throw new FileNotFoundException("Source File not found.", file.Path.FullPath);
            }

            using (var resultStream = new MemoryStream())
            {
                using (var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    XmlPoke(fileStream, resultStream, xpath, value, settings);
                }
                using (var fileStream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    resultStream.Position = 0;
                    resultStream.CopyTo(fileStream);
                }
            }
        }

        /// <summary>
        /// Set the value of, or remove, target nodes.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sourceXml">The source xml to transform.</param>
        /// <param name="xpath">The xpath of the nodes to set.</param>
        /// <param name="value">The value to set too. Leave blank to remove the selected nodes.</param>
        /// <returns>Resulting XML.</returns>
        /// <example>
        ///   <para>
        ///   Change the <c>server</c> setting in the configuration from <c>testhost.somecompany.com</c>
        ///   to <c>productionhost.somecompany.com</c>.
        ///   </para>
        ///   <para>XML string:</para>
        ///   <code>
        ///     <![CDATA[
        /// <?xml version="1.0" encoding="utf-8" ?>
        /// <configuration>
        ///     <appSettings>
        ///         <add key="server" value="testhost.somecompany.com" />
        ///     </appSettings>
        /// </configuration>
        ///     ]]>
        ///   </code>
        ///   <para>Cake Task:</para>
        ///   <code>
        ///     <![CDATA[
        /// Task("Transform")
        ///     .Does(() =>
        /// {
        ///     var result = XmlPokeString(xmlString, "/configuration/appSettings/add[@key = 'server']/@value", "productionhost.somecompany.com");
        /// });
        ///     ]]>
        ///   </code>
        /// </example>
        /// <example>
        ///   <para>
        ///   Modify the <c>noNamespaceSchemaLocation</c> in an XML file.
        ///   </para>
        ///   <para>XML string:</para>
        ///   <code>
        ///     <![CDATA[
        /// <?xml version="1.0" encoding="utf-8" ?>
        /// <Commands xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="Path Value">
        /// </Commands>
        ///     ]]>
        ///   </code>
        ///   <para>Cake Task:</para>
        ///   <code>
        ///     <![CDATA[
        /// Task("Transform")
        ///     .Does(() =>
        /// {
        ///     var result = XmlPokeString(xmlString, "/Commands/@xsi:noNamespaceSchemaLocation", "d:\\Commands.xsd", new XmlPokeSettings {
        ///         Namespaces = new Dictionary<string, string> {
        ///             { /* Prefix */ "xsi", /* URI */ "http://www.w3.org/2001/XMLSchema-instance" }
        ///         }
        ///     });
        /// });
        ///     ]]>
        ///   </code>
        /// </example>
        /// <example>
        ///   <para>
        ///   Remove an app setting from a config file.
        ///   </para>
        ///   <para>XML string:</para>
        ///   <code>
        ///     <![CDATA[
        /// <?xml version="1.0" encoding="utf-8" ?>
        /// <configuration>
        ///     <appSettings>
        ///         <add key="server" value="testhost.somecompany.com" />
        ///         <add key="testing" value="true" />
        ///     </appSettings>
        /// </configuration>
        ///     ]]>
        ///   </code>
        ///   <para>Cake Task:</para>
        ///   <code>
        ///     <![CDATA[
        /// Task("Transform")
        ///     .Does(() =>
        /// {
        ///     var result = XmlPokeString(xmlString, "/configuration/appSettings/add[@testing]", null);
        /// });
        ///     ]]>
        ///   </code>
        /// </example>
        /// <para>
        /// Credit to NAnt for the original example.
        /// http://nant.sourceforge.net/release/latest/help/tasks/xmlpoke.html.
        /// </para>
        [CakeMethodAlias]
        public static string XmlPokeString(this ICakeContext context, string sourceXml, string xpath, string value)
        {
            return context.XmlPokeString(sourceXml, xpath, value, new XmlPokeSettings());
        }

        /// <summary>
        /// Set the value of, or remove, target nodes.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sourceXml">The source xml to transform.</param>
        /// <param name="xpath">The xpath of the nodes to set.</param>
        /// <param name="value">The value to set too. Leave blank to remove the selected nodes.</param>
        /// <param name="settings">Additional settings to tweak Xml Poke behavior.</param>
        /// <returns>Resulting XML.</returns>
        /// <example>
        ///   <para>
        ///   Change the <c>server</c> setting in the configuration from <c>testhost.somecompany.com</c>
        ///   to <c>productionhost.somecompany.com</c>.
        ///   </para>
        ///   <para>XML string:</para>
        ///   <code>
        ///     <![CDATA[
        /// <?xml version="1.0" encoding="utf-8" ?>
        /// <configuration>
        ///     <appSettings>
        ///         <add key="server" value="testhost.somecompany.com" />
        ///     </appSettings>
        /// </configuration>
        ///     ]]>
        ///   </code>
        ///   <para>Cake Task:</para>
        ///   <code>
        ///     <![CDATA[
        /// Task("Transform")
        ///     .Does(() =>
        /// {
        ///     var result = XmlPokeString(xmlString, "/configuration/appSettings/add[@key = 'server']/@value", "productionhost.somecompany.com");
        /// });
        ///     ]]>
        ///   </code>
        /// </example>
        /// <example>
        ///   <para>
        ///   Modify the <c>noNamespaceSchemaLocation</c> in an XML file.
        ///   </para>
        ///   <para>XML string:</para>
        ///   <code>
        ///     <![CDATA[
        /// <?xml version="1.0" encoding="utf-8" ?>
        /// <Commands xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="Path Value">
        /// </Commands>
        ///     ]]>
        ///   </code>
        ///   <para>Cake Task:</para>
        ///   <code>
        ///     <![CDATA[
        /// Task("Transform")
        ///     .Does(() =>
        /// {
        ///     var result = XmlPokeString(xmlString, "/Commands/@xsi:noNamespaceSchemaLocation", "d:\\Commands.xsd", new XmlPokeSettings {
        ///         Namespaces = new Dictionary<string, string> {
        ///             { /* Prefix */ "xsi", /* URI */ "http://www.w3.org/2001/XMLSchema-instance" }
        ///         }
        ///     });
        /// });
        ///     ]]>
        ///   </code>
        /// </example>
        /// <example>
        ///   <para>
        ///   Remove an app setting from a config file.
        ///   </para>
        ///   <para>XML string:</para>
        ///   <code>
        ///     <![CDATA[
        /// <?xml version="1.0" encoding="utf-8" ?>
        /// <configuration>
        ///     <appSettings>
        ///         <add key="server" value="testhost.somecompany.com" />
        ///         <add key="testing" value="true" />
        ///     </appSettings>
        /// </configuration>
        ///     ]]>
        ///   </code>
        ///   <para>Cake Task:</para>
        ///   <code>
        ///     <![CDATA[
        /// Task("Transform")
        ///     .Does(() =>
        /// {
        ///     var result = XmlPokeString(xmlString, "/configuration/appSettings/add[@testing]", null);
        /// });
        ///     ]]>
        ///   </code>
        /// </example>
        /// <para>
        /// Credit to NAnt for the original example.
        /// http://nant.sourceforge.net/release/latest/help/tasks/xmlpoke.html.
        /// </para>
        [CakeMethodAlias]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string XmlPokeString(this ICakeContext context, string sourceXml, string xpath, string value, XmlPokeSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrWhiteSpace(sourceXml))
            {
                throw new ArgumentNullException(nameof(sourceXml));
            }

            using (var resultStream = new MemoryStream())
            {
                using (var sourceStream = new MemoryStream(settings.Encoding.GetBytes(sourceXml)))
                {
                    XmlPoke(sourceStream, resultStream, xpath, value, settings);
                }
                resultStream.Position = 0;
                return settings.Encoding.GetString(resultStream.ToArray());
            }
        }

        /// <summary>
        /// Set the value of, or remove, target nodes.
        /// </summary>
        /// <param name="source">The source xml to transform.</param>
        /// <param name="destination">The destination to write too.</param>
        /// <param name="xpath">The xpath of the nodes to set.</param>
        /// <param name="value">The value to set too. Leave blank to remove the selected nodes.</param>
        /// <param name="settings">Additional settings to tweak Xml Poke behavior.</param>
        private static void XmlPoke(Stream source, Stream destination, string xpath, string value, XmlPokeSettings settings)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (string.IsNullOrWhiteSpace(xpath))
            {
                throw new ArgumentNullException(nameof(xpath));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            using (var xmlReader = XmlReader.Create(source, GetXmlReaderSettings(settings)))
            {
                // Load the document from the reader
                var document = new XmlDocument();
                document.PreserveWhitespace = settings.PreserveWhitespace;
                document.Load(xmlReader);
                // Add namespaces
                var namespaceManager = new XmlNamespaceManager(document.NameTable);
                foreach (var xmlNamespace in settings.Namespaces)
                {
                    namespaceManager.AddNamespace(xmlNamespace.Key /* Prefix */, xmlNamespace.Value /* URI */);
                }
                // Select the desired nodes
                var nodes = document.SelectNodes(xpath, namespaceManager);
                if (nodes == null || nodes.Count == 0)
                {
                    const string format = "Failed to find nodes matching the XPath '{0}'";
                    var message = string.Format(CultureInfo.InvariantCulture, format, xpath);
                    throw new CakeException(message);
                }

                // Adjust the nodes / values
                if (value == null)
                {
                    // Remove the nodes
                    foreach (XmlNode node in nodes)
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        // Pretty sure we should never be working with orphaned nodes.
                        node.ParentNode.RemoveChild(node);
                    }
                }
                else
                {
                    // Set the value
                    foreach (XmlNode node in nodes)
                    {
                        node.InnerXml = value;
                    }
                }

                // Get the writer settings
                var writerSettings = GetXmlWriterSettings(settings);
                // Adjust the writer settings according to the declaration
                var hasDeclaration = document.FirstChild.NodeType == XmlNodeType.XmlDeclaration;
                if (!hasDeclaration)
                {
                    writerSettings.OmitXmlDeclaration = true;
                }
                // Write the document
                using (var xmlWriter = XmlWriter.Create(destination, writerSettings))
                {
                    document.Save(xmlWriter);
                }
            }
        }

        /// <summary>
        /// Gets a XmlReaderSettings from a XmlPokeSettings.
        /// </summary>
        /// <returns>The xml reader settings.</returns>
        /// <param name="settings">Additional settings to tweak Xml Poke behavior.</param>
        private static XmlReaderSettings GetXmlReaderSettings(XmlPokeSettings settings)
        {
            var xmlReaderSettings = new XmlReaderSettings();

            xmlReaderSettings.DtdProcessing = (DtdProcessing)settings.DtdProcessing;
            return xmlReaderSettings;
        }

        /// <summary>
        /// Gets a XmlWriterSettings from a XmlPokeSettings.
        /// </summary>
        /// <param name="settings">Additional settings to tweak Xml Poke behavior.</param>
        /// <returns>The xml writer settings.</returns>
        private static XmlWriterSettings GetXmlWriterSettings(XmlPokeSettings settings)
        {
            var writerSettings = new XmlWriterSettings
            {
                Encoding = settings.Encoding,
                NewLineOnAttributes = settings.NewLineOnAttributes,
                IndentChars = settings.IndentChars,
                Indent = settings.Indent
            };
            // Currently only returns the default settings
            return writerSettings;
        }
    }
}
