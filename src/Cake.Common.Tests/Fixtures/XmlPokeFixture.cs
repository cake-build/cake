// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq;
using System.Xml;
using Cake.Common.Tests.Properties;
using Cake.Common.Xml;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    internal sealed class XmlPokeFixture
    {
        public IFileSystem FileSystem { get; set; }
        public ICakeContext Context { get; set; }
        public FilePath XmlPath { get; set; }
        public XmlPokeSettings Settings { get; set; }

        public XmlPokeFixture(bool xmlExists = true, bool xmlWithDtd = false)
        {
            Settings = new XmlPokeSettings();

            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateDirectory("/Working");

            if (xmlExists)
            {
                string content = xmlWithDtd ? Resources.XmlPoke_Xml_Dtd : Resources.XmlPoke_Xml;
                var xmlFile = fileSystem.CreateFile("/Working/web.config").SetContent(content);
                XmlPath = xmlFile.Path;
            }

            FileSystem = fileSystem;

            Context = Substitute.For<ICakeContext>();
            Context.FileSystem.Returns(FileSystem);
            Context.Environment.Returns(environment);
        }

        public void Poke(string xpath, string value)
        {
            XmlPokeAliases.XmlPoke(Context, XmlPath, xpath, value, Settings);
        }

        public string PokeString(string xml, string xpath, string value)
        {
            return XmlPokeAliases.XmlPokeString(Context, xml, xpath, value, Settings);
        }

        public bool TestIsValue(string xpath, string value)
        {
            var xmlString = new StreamReader(FileSystem.GetFile(XmlPath).OpenRead()).ReadToEnd();
            return TestIsValue(xmlString, xpath, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public bool TestIsValue(string xml, string xpath, string value)
        {
            using (var reader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(reader, GetXmlReaderSettings(Settings)))
            {
                var document = new XmlDocument();
                document.Load(xmlReader);

                var namespaceManager = new XmlNamespaceManager(document.NameTable);
                foreach (var xmlNamespace in Settings.Namespaces)
                {
                    namespaceManager.AddNamespace(xmlNamespace.Key /* Prefix */, xmlNamespace.Value /* URI */);
                }

                var nodes = document.SelectNodes(xpath, namespaceManager);
                return nodes != null && nodes.Cast<XmlNode>().All(node => node.Value == value);
            }
        }

        public bool TestIsRemoved(string xpath)
        {
            var xmlString = new StreamReader(FileSystem.GetFile(XmlPath).OpenRead()).ReadToEnd();
            return TestIsRemoved(xmlString, xpath);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public bool TestIsRemoved(string xml, string xpath)
        {
            using (var reader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(reader, GetXmlReaderSettings(Settings)))
            {
                var document = new XmlDocument();
                document.Load(xmlReader);

                var namespaceManager = new XmlNamespaceManager(document.NameTable);
                foreach (var xmlNamespace in Settings.Namespaces)
                {
                    namespaceManager.AddNamespace(xmlNamespace.Key /* Prefix */, xmlNamespace.Value /* URI */);
                }

                var nodes = document.SelectNodes(xpath, namespaceManager);
                return nodes != null && nodes.Count == 0;
            }
        }

        /// <summary>
        /// Gets a XmlReaderSettings from a XmlPokeSettings
        /// </summary>
        /// <returns>The xml reader settings.</returns>
        /// <param name="settings">Additional settings to tweak Xml Poke behavior.</param>
        private static XmlReaderSettings GetXmlReaderSettings(XmlPokeSettings settings)
        {
            var xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.DtdProcessing = (DtdProcessing)settings.DtdProcessing;

            return xmlReaderSettings;
        }
    }
}