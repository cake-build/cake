// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.IO;
using System.Xml;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Xml
{
    /// <summary>
    /// Contains functionality related to XML XPath queries.
    /// </summary>
    [CakeAliasCategory("XML")]
    public static class XmlPeekAliases
    {
        /// <summary>
        /// Gets the value of a target node.
        /// </summary>
        /// <returns>The value found at the given XPath query.</returns>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The target file.</param>
        /// <param name="xpath">The xpath of the node to get.</param>
        /// <example>
        /// <code>
        /// string autoFacVersion = XmlPeek("./src/Cake/packages.config", "/packages/package[@id='Autofac']/@version");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static string XmlPeek(this ICakeContext context, FilePath filePath, string xpath)
        {
            return context.XmlPeek(filePath, xpath, new XmlPeekSettings());
        }

        /// <summary>
        /// Get the value of a target node.
        /// </summary>
        /// <returns>The value found at the given XPath query.</returns>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The target file.</param>
        /// <param name="xpath">The xpath of the nodes to set.</param>
        /// <param name="settings">Additional settings to tweak Xml Peek behavior.</param>
        /// <example>
        /// <code>
        /// <para>XML document:</para>
        /// <![CDATA[
        /// <?xml version="1.0" encoding="UTF-8"?>
        /// <pastery xmlns = "http://cakebuild.net/pastery" >
        ///     < cake price="1.62" />
        /// </pastery>
        /// ]]>
        /// </code>
        /// <para>XmlPeek usage:</para>
        /// <code>
        /// string version = XmlPeek("./pastery.xml", "/pastery:pastery/pastery:cake/@price",
        ///     new XmlPeekSettings {
        ///         Namespaces = new Dictionary&lt;string, string&gt; {{ "pastery", "http://cakebuild.net/pastery" }}
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string XmlPeek(this ICakeContext context, FilePath filePath, string xpath, XmlPeekSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var file = context.FileSystem.GetFile(filePath);
            if (!file.Exists)
            {
                throw new FileNotFoundException("Source File not found.", file.Path.FullPath);
            }

            using (var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
            using (var xmlReader = XmlReader.Create(fileStream, GetXmlReaderSettings(settings)))
            {
                var xmlValue = XmlPeek(xmlReader, xpath, settings);
                if (xmlValue == null)
                {
                    context.Log.Warning("Warning: Failed to find node matching the XPath '{0}'", xpath);
                }
                return xmlValue;
            }
        }

        /// <summary>
        /// Gets the value of a target node.
        /// </summary>
        /// <returns>The value found at the given XPath query (or the first, if multiple eligible nodes are found).</returns>
        /// <param name="source">The source xml to transform.</param>
        /// <param name="xpath">The xpath of the nodes to set.</param>
        /// <param name="settings">Additional settings to tweak Xml Peek behavior.</param>
        private static string XmlPeek(XmlReader source, string xpath, XmlPeekSettings settings)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrWhiteSpace(xpath))
            {
                throw new ArgumentNullException("xpath");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var document = new XmlDocument();
            document.PreserveWhitespace = settings.PreserveWhitespace;
            document.Load(source);

            var namespaceManager = new XmlNamespaceManager(document.NameTable);
            foreach (var xmlNamespace in settings.Namespaces)
            {
                namespaceManager.AddNamespace(xmlNamespace.Key /* Prefix */, xmlNamespace.Value /* URI */);
            }

            var node = document.SelectSingleNode(xpath, namespaceManager);

            return node != null ? node.Value : null;
        }

        /// <summary>
        /// Gets a XmlReaderSettings from a XmlPeekSettings
        /// </summary>
        /// <returns>The xml reader settings.</returns>
        /// <param name="settings">Additional settings to tweak Xml Peek behavior.</param>
        private static XmlReaderSettings GetXmlReaderSettings(XmlPeekSettings settings)
        {
            var xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.DtdProcessing = (DtdProcessing)settings.DtdProcessing;
            return xmlReaderSettings;
        }
    }
}
