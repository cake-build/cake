// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Common.Solution.Project.XmlDoc;
using Cake.Common.Tests.Properties;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    internal sealed class XmlDocExampleCodeParserFixture
    {
        public IFileSystem FileSystem { get; set; }
        public IGlobber Globber { get; set; }
        public ICakeLog Log { get; set; }
        public FilePath XmlFilePath { get; set; }
        public string Pattern { get; set; }

        public XmlDocExampleCodeParserFixture()
        {
            XmlFilePath = "/Working/Cake.Common.xml";
            Pattern = "/Working/Cake.*.xml";

            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateFile(XmlFilePath.FullPath).SetContent(Resources.XmlDoc_ExampeCode_Cake_Common_Xml);
            fileSystem.CreateFile("/Working/Cake.UnCommon.xml").SetContent(Resources.XmlDoc_ExampeCode_Cake_Common_Xml);
            FileSystem = fileSystem;

            Globber = Substitute.For<IGlobber>();
            Globber.GetFiles(Pattern).Returns(new FilePath[] { "/Working/Cake.Common.xml", "/Working/Cake.UnCommon.xml"});

            Log = Substitute.For<ICakeLog>();
        }

        public IEnumerable<XmlDocExampleCode> Parse()
        {
            var parser = new XmlDocExampleCodeParser(FileSystem, Globber, Log);
            return parser.Parse(XmlFilePath);
        }

        public IEnumerable<XmlDocExampleCode> ParseFiles()
        {
            var parser = new XmlDocExampleCodeParser(FileSystem, Globber, Log);
            return parser.ParseFiles(Pattern);
        }
    }
}
