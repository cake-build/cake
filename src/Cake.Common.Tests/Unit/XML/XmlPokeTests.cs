// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tests.Properties;
using Cake.Common.Xml;
using Cake.Testing.Xunit;
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

            [RuntimeFact(TestRuntime.Clr)]
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

            [RuntimeFact(TestRuntime.CoreClr)]
            public void Should_Throw_On_Net_Core_Change_Attribute_From_Xml_File_With_Dtd()
            {
                // Given
                var fixture = new XmlPokeFixture(xmlWithDtd: true);
                fixture.Settings.DtdProcessing = XmlDtdProcessing.Parse;

                // When
                var result = Record.Exception(() => fixture.Poke("/plist/dict/string", "Cake Version"));

                // Then
                AssertEx.IsCakeException(result, "DtdProcessing is not available on .NET Core.");
            }
        }
    }
}