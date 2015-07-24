using System;
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
        /// <param name="settings">Additional settings to tweak Xml Poke behavior.</param>
        [CakeMethodAlias]
        public static void XmlPoke(this ICakeContext context, FilePath filePath, string xpath, string value, XmlPokeSettings settings = null)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

            if (string.IsNullOrWhiteSpace(xpath))
            {
                throw new ArgumentNullException("xpath");
            }

            if (settings == null)
            {
                settings = new XmlPokeSettings();
            }

            var sourceFile = context.FileSystem.GetFile(filePath);
            if (!sourceFile.Exists)
            {
                throw new FileNotFoundException("Source File not found.", sourceFile.Path.FullPath);
            }
            
            using (var sourceReader = sourceFile.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                var xmlReader = XmlReader.Create(sourceReader);
                var xmlWriter = XmlWriter.Create(sourceReader);
                XmlPoke(context, xmlReader, xmlWriter, xpath, value, settings);
            }
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
        [CakeMethodAlias]
        public static string XmlPoke(this ICakeContext context, string sourceXml, string xpath, string value, XmlPokeSettings settings = null)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (string.IsNullOrWhiteSpace(sourceXml))
            {
                throw new ArgumentNullException("sourceXml");
            }

            if (string.IsNullOrWhiteSpace(xpath))
            {
                throw new ArgumentNullException("xpath");
            }

            if (settings == null)
            {
                settings = new XmlPokeSettings();
            }

            using (var resultStream = new MemoryStream())
            using (var sourceReader = new StringReader(sourceXml))
            {
                var xmlReader = XmlReader.Create(sourceReader);
                var xmlWriter = XmlWriter.Create(resultStream);
                XmlPoke(context, xmlReader, xmlWriter, xpath, value, settings);
                resultStream.Position = 0;
                return settings.Encoding.GetString(resultStream.ToArray());
            }
        }

        /// <summary>
        /// Set the value of, or remove, target nodes.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="source">The source xml to transform.</param>
        /// <param name="destination">The destination to write too.</param>
        /// <param name="xpath">The xpath of the nodes to set.</param>
        /// <param name="value">The value to set too. Leave blank to remove the selected nodes.</param>
        /// <param name="settings">Additional settings to tweak Xml Poke behavior.</param>
        [CakeMethodAlias]
        public static void XmlPoke(this ICakeContext context, XmlReader source, XmlWriter destination, string xpath, string value, XmlPokeSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (destination == null)
            {
                throw new ArgumentNullException("destination");
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

            var nodes = document.SelectNodes(xpath, namespaceManager);
            if (nodes == null || nodes.Count == 0)
            {
                throw new CakeException("Failed to find nodes matching that XPath.");
            }

            if (value == null)
            {
                foreach (XmlNode node in nodes)
                {
                    // ReSharper disable once PossibleNullReferenceException
                    // Pretty sure we should never be working with orphaned nodes.
                    node.ParentNode.RemoveChild(node);
                }
            }
            else
            {
                foreach (XmlNode node in nodes)
                {
                    node.InnerXml = value;
                }
            }

            document.Save(destination);
        }
    }
}
