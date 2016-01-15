using System;
using System.Globalization;
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
        [CakeMethodAlias]
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
            
            var file = context.FileSystem.GetFile(filePath);
            if (!file.Exists)
            {
                throw new FileNotFoundException("Source File not found.", file.Path.FullPath);
            }
            
            using (var memoryStream = new MemoryStream())
            {
                using (var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                using (var xmlReader = XmlReader.Create(fileStream))
                using (var xmlWriter = XmlWriter.Create(memoryStream))
                {
                    var xmlValue = XmlPeek(xmlReader, xpath, settings);
                    if (xmlValue == null)
                    {
                        context.Log.Warning("Warning: Failed to find node matching the XPath '{0}'", xpath);
                    }
                    return xmlValue;
                }
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
    }
}