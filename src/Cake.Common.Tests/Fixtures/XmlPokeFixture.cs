using System.IO;
using System.Linq;
using System.Xml;

using Cake.Common.Tests.Properties;
using Cake.Common.Xml;
using Cake.Core.IO;
using Cake.Testing.Fakes;

namespace Cake.Common.Tests.Fixtures
{
    using Cake.Core;

    using NSubstitute;

    internal sealed class XmlPokeFixture
    {
       public IFileSystem FileSystem { get; set; }
       public ICakeContext Context { get; set; }
       public FilePath XmlPath { get; set; }
       public XmlPokeSettings Settings { get; set; }

       public XmlPokeFixture(bool xmlExists = true)
       {
           Settings = new XmlPokeSettings();

           var environment = FakeEnvironment.CreateUnixEnvironment();
           var fileSystem = new FakeFileSystem(environment);
           fileSystem.CreateDirectory("/Working");

           if (xmlExists)
           {
               var xmlFile = fileSystem.CreateFile("/Working/web.config").SetContent(Resources.XmlPoke_Xml);
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

        public bool TestIsValue(string xpath, string value)
        {
            var xmlString = new StreamReader(FileSystem.GetFile(XmlPath).OpenRead()).ReadToEnd();
            return TestIsValue(xmlString, xpath, value);
        }

        public bool TestIsValue(string xml, string xpath, string value)
        {
            using (var reader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(reader))
            {

                var document = new XmlDocument();
                document.Load(xmlReader);

                var namespaceManager = new XmlNamespaceManager(document.NameTable);
                foreach (var xmlNamespace in Settings.Namespaces)
                {
                    namespaceManager.AddNamespace(xmlNamespace.Key /* Prefix */, xmlNamespace.Value /* URI */);
                }

                var nodes = document.SelectNodes(xpath, namespaceManager);
                return nodes.Cast<XmlNode>().All(node => node.Value == value);
            }
        }

        public bool TestIsRemoved(string xpath)
        {
            var xmlString = new StreamReader(FileSystem.GetFile(XmlPath).OpenRead()).ReadToEnd();
            return TestIsRemoved(xmlString, xpath);
        }

        public bool TestIsRemoved(string xml, string xpath)
        {
            using (var reader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(reader))
            {

                var document = new XmlDocument();
                document.Load(xmlReader);

                var namespaceManager = new XmlNamespaceManager(document.NameTable);
                foreach (var xmlNamespace in Settings.Namespaces)
                {
                    namespaceManager.AddNamespace(xmlNamespace.Key /* Prefix */, xmlNamespace.Value /* URI */);
                }

                var nodes = document.SelectNodes(xpath, namespaceManager);
                return nodes.Count == 0;
            }
        }
    }
}
