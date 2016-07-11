// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Properties;
using Cake.Common.Xml;
using Cake.Core.IO;
using Cake.Testing;

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

           var environment = FakeEnvironment.CreateUnixEnvironment();
           var fileSystem = new FakeFileSystem(environment);
           fileSystem.CreateDirectory("/Working");

           if (xmlExists)
           {
               var xmlFile = fileSystem.CreateFile("/Working/breakfast_menu.xml").SetContent(Resources.XmlTransformation_Xml);
               XmlPath = xmlFile.Path;
           }

           if (xslExists)
           {
               var xslFile = fileSystem.CreateFile("/Working/breakfast_menu.xsl").SetContent(Resources.XmlTransformation_Xsl);
               XslPath = xslFile.Path;
           }

           if (resultExist)
           {
               var resultFile = fileSystem.CreateFile("/Working/breakfast_menu.htm").SetContent(Resources.XmlTransformation_Htm);
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
