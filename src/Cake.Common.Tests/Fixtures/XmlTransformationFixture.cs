using Cake.Common.Tests.Properties;
using Cake.Common.Xml;
using Cake.Core.IO;
using Cake.Testing.Fakes;

namespace Cake.Common.Tests.Fixtures
{
   internal sealed class XmlTransformationFixture
    {
       public IFileSystem FileSystem { get; set; }
       public FilePath XslPath { get; set; }
       public FilePath XmlPath { get; set; }
       public FilePath ResultPath { get; set; }
       public XmlTransformationSettings Settings { get; set; }


       public XmlTransformationFixture(bool xmlExists = true, bool xslExists = true, bool resultExist = false)
       {
           Settings = new XmlTransformationSettings();

           var fileSystem = new FakeFileSystem(false);
           fileSystem.GetCreatedDirectory("/Working");

           if (xmlExists)
           {
               var xmlFile = fileSystem.GetCreatedFile("/Working/breakfast_menu.xml", Resources.XmlTransformation_Xml);
               XmlPath = xmlFile.Path;
           }

           if (xslExists)
           {
               var xslFile = fileSystem.GetCreatedFile("/Working/breakfast_menu.xsl", Resources.XmlTransformation_Xsl);
               XslPath = xslFile.Path;
           }

           if (resultExist)
           {
               var resultFile = fileSystem.GetCreatedFile("/Working/breakfast_menu.htm", Resources.XmlTransformation_Htm);
               ResultPath = resultFile.Path;
           }
           else
           {
               ResultPath = "/Working/breakfast_menu.htm";
           }

           FileSystem = fileSystem;
       }

       public void Transform()
       {
           XmlTransformation.Transform(FileSystem, XslPath, XmlPath, ResultPath, Settings);
       }
    }
}
