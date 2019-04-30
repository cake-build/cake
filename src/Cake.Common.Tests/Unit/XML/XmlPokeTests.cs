// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Text;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tests.Properties;
using Cake.Common.Xml;
using Xunit;

namespace Cake.Common.Tests.Unit.XML
{
    public sealed class XmlPokeTests
    {
        public sealed class ValidateParameters
        {
            [Fact]
            public void Should_Throw_If_FilePath_Is_Null()
            {
                // Given
                var fixture = new XmlPokeFixture(false);

                // When
                var result = Record.Exception(() => fixture.Poke("gibblygook", ""));

                // Then
                AssertEx.IsArgumentNullException(result, "filePath");
            }

            [Fact]
            public void Should_Throw_If_File_Does_Not_Exists()
            {
                // Given
                var fixture = new XmlPokeFixture(false);
                fixture.XmlPath = "/Working/web.config";

                // When
                var result = Record.Exception(() => fixture.Poke("gibblygook", ""));

                // Then
                Assert.IsType<FileNotFoundException>(result);
            }

            [Fact]
            public void Should_Throw_If_No_Xpath()
            {
                // Given
                var fixture = new XmlPokeFixture();

                // When
                var result = Record.Exception(() => fixture.Poke(null, ""));

                // Then
                AssertEx.IsArgumentNullException(result, "xpath");
            }

            [Fact]
            public void Should_Throw_If_Xml_File_Has_Dtd()
            {
                // Given
                var fixture = new XmlPokeFixture(xmlWithDtd: true);

                // When
                var result = Record.Exception(() => fixture.Poke("/plist/dict/string/text()", ""));

                // Then
                Assert.IsType<System.Xml.XmlException>(result);
            }

            [Fact]
            public void Should_Throw_If_Xml_String_Has_Dtd()
            {
                // Given
                var fixture = new XmlPokeFixture(xmlExists: false);

                // When
                var result = Record.Exception(() => fixture.PokeString(Resources.XmlPoke_Xml_Dtd, "/plist/dict/string/text()", ""));

                // Then
                Assert.IsType<System.Xml.XmlException>(result);
            }
        }

        public sealed class Transform
        {
            [Fact]
            public void Should_Change_Attribute()
            {
                // Given
                var fixture = new XmlPokeFixture();

                // When
                fixture.Poke("/configuration/appSettings/add[@key = 'server']/@value", "productionhost.somecompany.com");

                // Then
                Assert.True(fixture.TestIsValue(
                    "/configuration/appSettings/add[@key = 'server']/@value",
                    "productionhost.somecompany.com"));
            }

            [Fact]
            public void Should_Remove_Attribute()
            {
                // Given
                var fixture = new XmlPokeFixture();

                // When
                fixture.Poke("/configuration/appSettings/add[@key = 'server']", null);

                // Then
                Assert.True(fixture.TestIsRemoved(
                    "/configuration/appSettings/add[@key = 'server']"));
            }

            [Fact]
            public void Should_Have_Encoding_UTF8_With_BOM()
            {
                // Given
                var fixture = new XmlPokeFixture();
                fixture.Settings.Encoding = new UTF8Encoding(true);

                // When
                fixture.Poke("/configuration/appSettings/add[@key = 'server']", null);

                // Then
                Assert.True(fixture.TestIsUTF8WithBOM());
            }

            [Fact]
            public void Should_Have_Encoding_UTF8_Without_BOM()
            {
                // Given
                var fixture = new XmlPokeFixture();
                fixture.Settings.Encoding = new UTF8Encoding(false);

                // When
                fixture.Poke("/configuration/appSettings/add[@key = 'server']", null);

                // Then
                Assert.False(fixture.TestIsUTF8WithBOM());
            }

            [Fact]
            public void Should_Change_Attribute_From_Xml_File_With_Dtd()
            {
                // Given
                var fixture = new XmlPokeFixture(xmlWithDtd: true);
                fixture.Settings.DtdProcessing = XmlDtdProcessing.Parse;

                // When
                fixture.Poke("/plist/dict/string", "Cake Version");

                // Then
                Assert.True(fixture.TestIsValue(
                    "/plist/dict/string/text()",
                    "Cake Version"));
            }

            [Fact]
            public void Should_Have_Declaration()
            {
                // Given
                var fixture = new XmlPokeFixture();

                // When
                var resultXml = fixture.PokeString(Resources.XmlPoke_Xml, "/configuration/appSettings/add[@key = 'server']", null);

                // Then
                Assert.Contains("<?xml", resultXml);
            }

            [Fact]
            public void Should_Not_Have_Declaration()
            {
                // Given
                var fixture = new XmlPokeFixture();

                // When
                var resultXml = fixture.PokeString(Resources.XmlPoke_Xml_Without_Declaration, "/configuration/appSettings/add[@key = 'server']", null);

                // Then
                Assert.DoesNotContain("<?xml", resultXml);
            }
        }

        public sealed class Formatting
        {
            [Fact]
            public void Should_Preserve_Whitespace()
            {
                // Given
                var fixture = new XmlPokeFixture();
                fixture.Settings.PreserveWhitespace = true;

                // When
                fixture.Poke("/configuration/appSettings/add[@key = 'server']/@value", "testhost.somecompany.com");

                // Then
                var expectedXml =
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine +
                    "<configuration>" + Environment.NewLine +
                    "    <appSettings>" + Environment.NewLine +
                    "        <add key=\"server\" value=\"testhost.somecompany.com\" />" + Environment.NewLine +
                    "        <add key=\"test\" value=\"true\" />" + Environment.NewLine +
                    "    </appSettings>" + Environment.NewLine +
                    "</configuration>";
                Assert.Equal(expectedXml, fixture.GetFullXml());
            }

            [Fact]
            public void Should_Ignore_Whitespace()
            {
                // Given
                var fixture = new XmlPokeFixture();
                fixture.Settings.PreserveWhitespace = false;

                // When
                fixture.Poke("/configuration/appSettings/add[@key = 'server']/@value", "testhost.somecompany.com");

                // Then
                var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration><appSettings><add key=\"server\" value=\"testhost.somecompany.com\" /><add key=\"test\" value=\"true\" /></appSettings></configuration>";
                Assert.Equal(expectedXml, fixture.GetFullXml());
            }

            [Fact]
            public void Should_Indent()
            {
                // Given
                var fixture = new XmlPokeFixture();
                fixture.Settings.PreserveWhitespace = false;
                fixture.Settings.Indent = true;

                // When
                fixture.Poke("/configuration/appSettings/add[@key = 'server']/@value", "testhost.somecompany.com");

                // Then
                var expectedXml =
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine +
                    "<configuration>" + Environment.NewLine +
                    "  <appSettings>" + Environment.NewLine +
                    "    <add key=\"server\" value=\"testhost.somecompany.com\" />" + Environment.NewLine +
                    "    <add key=\"test\" value=\"true\" />" + Environment.NewLine +
                    "  </appSettings>" + Environment.NewLine +
                    "</configuration>";
                Assert.Equal(expectedXml, fixture.GetFullXml());
            }

            [Fact]
            public void Should_Indent_With_Tab()
            {
                // Given
                var fixture = new XmlPokeFixture();
                fixture.Settings.PreserveWhitespace = false;
                fixture.Settings.Indent = true;
                fixture.Settings.IndentChars = "\t";

                // When
                fixture.Poke("/configuration/appSettings/add[@key = 'server']/@value", "testhost.somecompany.com");

                // Then
                var expectedXml =
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine +
                    "<configuration>" + Environment.NewLine +
                    "\t<appSettings>" + Environment.NewLine +
                    "\t\t<add key=\"server\" value=\"testhost.somecompany.com\" />" + Environment.NewLine +
                    "\t\t<add key=\"test\" value=\"true\" />" + Environment.NewLine +
                    "\t</appSettings>" + Environment.NewLine +
                    "</configuration>";
                Assert.Equal(expectedXml, fixture.GetFullXml());
            }

            [Fact]
            public void Should_Put_Attributes_On_New_Line()
            {
                // Given
                var fixture = new XmlPokeFixture();
                fixture.Settings.PreserveWhitespace = false;
                fixture.Settings.Indent = true;
                fixture.Settings.NewLineOnAttributes = true;

                // When
                fixture.Poke("/configuration/appSettings/add[@key = 'server']/@value", "testhost.somecompany.com");

                // Then
                var expectedXml =
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine +
                    "<configuration>" + Environment.NewLine +
                    "  <appSettings>" + Environment.NewLine +
                    "    <add" + Environment.NewLine +
                    "      key=\"server\"" + Environment.NewLine +
                    "      value=\"testhost.somecompany.com\" />" + Environment.NewLine +
                    "    <add" + Environment.NewLine +
                    "      key=\"test\"" + Environment.NewLine +
                    "      value=\"true\" />" + Environment.NewLine +
                    "  </appSettings>" + Environment.NewLine +
                    "</configuration>";
                Assert.Equal(expectedXml, fixture.GetFullXml());
            }
        }
    }
}