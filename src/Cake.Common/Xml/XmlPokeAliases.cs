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
        ///     XmlPoke(file, "/Commands/@xsi:noNamespaceSchemaLocation", "d:\Commands.xsd", new XmlPokeSettings {
        ///         Namespaces = new Dictionary<string, string> {
        ///             { /* Prefix */ "xsi", /* URI */ "http://www.w3.org/2001/XMLSchema-instance" }
        ///         }
        ///     });
        /// });
        ///     ]]>
        ///   </code>        
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
        /// http://nant.sourceforge.net/release/latest/help/tasks/xmlpoke.html
        /// </para>
        /// </example>
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
        ///     XmlPoke(file, "/Commands/@xsi:noNamespaceSchemaLocation", "d:\Commands.xsd", new XmlPokeSettings {
        ///         Namespaces = new Dictionary<string, string> {
        ///             { /* Prefix */ "xsi", /* URI */ "http://www.w3.org/2001/XMLSchema-instance" }
        ///         }
        ///     });
        /// });
        ///     ]]>
        ///   </code>        
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
        /// http://nant.sourceforge.net/release/latest/help/tasks/xmlpoke.html
        /// </para>
        /// </example>
        [CakeMethodAlias]
        public static void XmlPoke(this ICakeContext context, FilePath filePath, string xpath, string value, XmlPokeSettings settings)
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
                    XmlPoke(xmlReader, xmlWriter, xpath, value, settings);
                }

                using (var fileStream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    memoryStream.Position = 0;
                    memoryStream.CopyTo(fileStream);
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
        ///     var result = XmlPoke(xmlString, "/configuration/appSettings/add[@key = 'server']/@value", "productionhost.somecompany.com");
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
        ///     var result = XmlPoke(xmlString, "/Commands/@xsi:noNamespaceSchemaLocation", "d:\Commands.xsd", new XmlPokeSettings {
        ///         Namespaces = new Dictionary<string, string> {
        ///             { /* Prefix */ "xsi", /* URI */ "http://www.w3.org/2001/XMLSchema-instance" }
        ///         }
        ///     });
        /// });
        ///     ]]>
        ///   </code>        
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
        ///     var result = XmlPoke(xmlString, "/configuration/appSettings/add[@testing]", null);
        /// });
        ///     ]]>
        ///   </code>
        /// </example>
        /// <para>
        /// Credit to NAnt for the original example.
        /// http://nant.sourceforge.net/release/latest/help/tasks/xmlpoke.html
        /// </para>
        /// </example>
        [CakeMethodAlias]
        public static string XmlPoke(this ICakeContext context, string sourceXml, string xpath, string value)
        {
            return context.XmlPoke(sourceXml, xpath, value, new XmlPokeSettings());
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
        ///     var result = XmlPoke(xmlString, "/configuration/appSettings/add[@key = 'server']/@value", "productionhost.somecompany.com");
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
        ///     var result = XmlPoke(xmlString, "/Commands/@xsi:noNamespaceSchemaLocation", "d:\Commands.xsd", new XmlPokeSettings {
        ///         Namespaces = new Dictionary<string, string> {
        ///             { /* Prefix */ "xsi", /* URI */ "http://www.w3.org/2001/XMLSchema-instance" }
        ///         }
        ///     });
        /// });
        ///     ]]>
        ///   </code>        
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
        ///     var result = XmlPoke(xmlString, "/configuration/appSettings/add[@testing]", null);
        /// });
        ///     ]]>
        ///   </code>
        /// </example>
        /// <para>
        /// Credit to NAnt for the original example.
        /// http://nant.sourceforge.net/release/latest/help/tasks/xmlpoke.html
        /// </para>
        /// </example>
        [CakeMethodAlias]
        public static string XmlPoke(this ICakeContext context, string sourceXml, string xpath, string value, XmlPokeSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (string.IsNullOrWhiteSpace(sourceXml))
            {
                throw new ArgumentNullException("sourceXml");
            }

            using (var resultStream = new MemoryStream())
            using (var fileReader = new StringReader(sourceXml))
            using (var xmlReader = XmlReader.Create(fileReader))
            using (var xmlWriter = XmlWriter.Create(resultStream))
            {
                XmlPoke(xmlReader, xmlWriter, xpath, value, settings);
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
        private static void XmlPoke(XmlReader source, XmlWriter destination, string xpath, string value, XmlPokeSettings settings)
        {
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
                throw new CakeException(string.Format("Failed to find nodes matching the XPath '{0}'", xpath));
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
