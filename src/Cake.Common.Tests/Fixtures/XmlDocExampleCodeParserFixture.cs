using System.Collections.Generic;
using Cake.Common.Solution.Project.XmlDoc;
using Cake.Common.Tests.Properties;
using Cake.Core.IO;
using Cake.Testing.Fakes;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    internal sealed class XmlDocExampleCodeParserFixture
    {
        public IFileSystem FileSystem { get; set; }
        public IGlobber Globber { get; set; }
        public FilePath XmlFilePath { get; set; }
        public string Pattern { get; set; }

        public XmlDocExampleCodeParserFixture()
        {
            XmlFilePath = "/Working/Cake.Common.xml";
            Pattern = "/Working/Cake.*.xml";

            var fileSystem = new FakeFileSystem(false);
            fileSystem.GetCreatedFile(XmlFilePath.FullPath, Resources.XmlDoc_ExampeCode_Cake_Common_Xml);
            fileSystem.GetCreatedFile("/Working/Cake.UnCommon.xml" , Resources.XmlDoc_ExampeCode_Cake_Common_Xml);
            FileSystem = fileSystem;

            Globber = Substitute.For<IGlobber>();
            Globber.GetFiles(Pattern).Returns(new FilePath[] { "/Working/Cake.Common.xml", "/Working/Cake.UnCommon.xml"});
        }

        public IEnumerable<XmlDocExampleCode> Parse()
        {
            var parser = new XmlDocExampleCodeParser(FileSystem, Globber);
            return parser.Parse(XmlFilePath);
        }

        public IEnumerable<XmlDocExampleCode> ParseFiles()
        {
            var parser = new XmlDocExampleCodeParser(FileSystem, Globber);
            return parser.ParseFiles(Pattern);
        }
    }
}
