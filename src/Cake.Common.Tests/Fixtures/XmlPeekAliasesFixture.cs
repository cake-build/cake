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
    internal sealed class XmlPeekAliasesFixture
    {
       public IFileSystem FileSystem { get; set; }
       public ICakeContext Context { get; set; }
       public FilePath XmlPath { get; set; }
       public XmlPeekSettings Settings { get; set; }

       public XmlPeekAliasesFixture(bool xmlExists = true)
       {
           Settings = new XmlPeekSettings();

           var environment = FakeEnvironment.CreateUnixEnvironment();
           var fileSystem = new FakeFileSystem(environment);
           fileSystem.CreateDirectory("/Working");

           if (xmlExists)
           {
               var xmlFile = fileSystem.CreateFile("/Working/web.config").SetContent(Resources.XmlPeek_Xml);
               XmlPath = xmlFile.Path;
           }

           FileSystem = fileSystem;

           Context = Substitute.For<ICakeContext>();
           Context.FileSystem.Returns(FileSystem);
           Context.Environment.Returns(environment);
       }

       public string Peek(string xpath)
       {
           return XmlPeekAliases.XmlPeek(Context, XmlPath, xpath, Settings);
       }
    }
}