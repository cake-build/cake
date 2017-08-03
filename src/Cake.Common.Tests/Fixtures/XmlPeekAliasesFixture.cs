// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
       public FakeLog FakeLog { get; set; }
       public FilePath XmlPath { get; set; }
       public XmlPeekSettings Settings { get; set; }

       public XmlPeekAliasesFixture(bool xmlExists = true, bool xmlWithDtd = false, bool suppressWarning = false)
       {
           Settings = new XmlPeekSettings { SuppressWarning = suppressWarning };

           var environment = FakeEnvironment.CreateUnixEnvironment();
           var fileSystem = new FakeFileSystem(environment);
           fileSystem.CreateDirectory("/Working");

           if (xmlExists)
           {
               string content = xmlWithDtd ? Resources.XmlPeek_Xml_Dtd : Resources.XmlPeek_Xml;
               var xmlFile = fileSystem.CreateFile("/Working/web.config").SetContent(content);
               XmlPath = xmlFile.Path;
           }

           FileSystem = fileSystem;
           FakeLog = new FakeLog();

           Context = Substitute.For<ICakeContext>();
           Context.FileSystem.Returns(FileSystem);
           Context.Environment.Returns(environment);
           Context.Log.Returns(FakeLog);
       }

       public void SetContent(string xml)
       {
           var file = ((FakeFileSystem)FileSystem).GetFile(XmlPath).SetContent(xml);
       }

       public string Peek(string xpath)
       {
           return XmlPeekAliases.XmlPeek(Context, XmlPath, xpath, Settings);
       }
    }
}