// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.IO;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Xml;
using Xunit;

namespace Cake.Common.Tests.Unit.XML
{
    public sealed class XmlPeekAliasesTests
    {
        public sealed class TheXmlPeekMethod
        {
            [Fact]
            public void Should_Throw_If_FilePath_Is_Null()
            {
                // Given
                var fixture = new XmlPeekAliasesFixture(false);

                // When
                var result = Record.Exception(() => fixture.Peek("gibblygook"));

                // Then
                Assert.IsArgumentNullException(result, "filePath");
            }

            [Fact]
            public void Should_Throw_If_File_Does_Not_Exists()
            {
                // Given
                var fixture = new XmlPeekAliasesFixture(false);
                fixture.XmlPath = "/Working/web.config";

                // When
                var result = Record.Exception(() => fixture.Peek("gibblygook"));

                // Then
                Assert.IsType<FileNotFoundException>(result);
            }

            [Fact]
            public void Should_Throw_If_No_Xpath()
            {
                // Given
                var fixture = new XmlPeekAliasesFixture();

                // When
                var result = Record.Exception(() => fixture.Peek(null));

                // Then
                Assert.IsArgumentNullException(result, "xpath");
            }

            [Fact]
            public void Should_Throw_If_File_Has_Dtd()
            {
                // Given
                var fixture = new XmlPeekAliasesFixture(xmlWithDtd: true);

                // When
                var result = Record.Exception(() => fixture.Peek("CFBundleDisplayName"));

                // Then
                Assert.IsType<System.Xml.XmlException>(result);
            }

            [Fact]
            public void Should_Get_Attribute_Value()
            {
                // Given
                var fixture = new XmlPeekAliasesFixture();

                // When
                var result = fixture.Peek("/configuration/appSettings/add[@key = 'server']/@value");

                // Then
                Assert.Equal("testhost.somecompany.com", result);
            }

            [Fact]
            public void Should_Get_Node_Value()
            {
                // Given
                var fixture = new XmlPeekAliasesFixture();

                // When
                var result = fixture.Peek("/configuration/test/text()");

                // Then
                Assert.Equal("test value", result);
            }

            [Fact]
            public void Should_Get_Node_Value_From_File_With_Dtd()
            {
                // Given
                var fixture = new XmlPeekAliasesFixture(xmlWithDtd:true);
                fixture.Settings.DtdProcessing = XmlDtdProcessing.Parse;

                // When
                var result = fixture.Peek("/plist/dict/key/text()");

                // Then
                Assert.Equal("CFBundleDisplayName", result);
            }
        }
    }
}
