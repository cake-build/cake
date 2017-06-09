// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETCORE
using System.IO;
using System.Linq;
using System.Text;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tests.Properties;
using Cake.Common.Xml;
using Cake.Core;
using Cake.Core.IO;
using Xunit;

namespace Cake.Common.Tests.Unit.XML
{
    public sealed class XmlTransformationTests
    {
        public sealed class TheTransformMethod
        {
            [Fact]
            public void Should_Throw_If_Xml_Path_Was_Null()
            {
                // Given
                var fixture = new XmlTransformationFixture();
                fixture.XmlPath = null;

                // When
                var result = Record.Exception(() => fixture.Transform());

                // Then
                AssertEx.IsArgumentNullException(result, "xmlPath");
            }

            [Fact]
            public void Should_Throw_If_Xsl_Path_Was_Null()
            {
                // Given
                var fixture = new XmlTransformationFixture();
                fixture.XslPath = null;

                // When
                var result = Record.Exception(() => fixture.Transform());

                // Then
                AssertEx.IsArgumentNullException(result, "xslPath");
            }

            [Fact]
            public void Should_Throw_If_Result_Path_Was_Null()
            {
                // Given
                var fixture = new XmlTransformationFixture();
                fixture.ResultPath = null;

                // When
                var result = Record.Exception(() => fixture.Transform());

                // Then
                AssertEx.IsArgumentNullException(result, "resultPath");
            }

            [Fact]
            public void Should_Throw_If_Settings_Was_Null()
            {
                // Given
                var fixture = new XmlTransformationFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Transform());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Xml_Not_Exists()
            {
                // Given
                var fixture = new XmlTransformationFixture(xmlExists: false)
                {
                    XmlPath = "/Working/non_existsing.xml"
                };

                // When
                var result = Record.Exception(() => fixture.Transform());

                // Then
                AssertEx.IsExceptionWithMessage<FileNotFoundException>(result, "XML File not found.");
            }

            [Fact]
            public void Should_Throw_If_Xsl_Not_Exists()
            {
                // Given
                var fixture = new XmlTransformationFixture(xslExists: false)
                {
                    XslPath = "/Working/non_existsing.xsl"
                };

                // When
                var result = Record.Exception(() => fixture.Transform());

                // Then
                AssertEx.IsExceptionWithMessage<FileNotFoundException>(result, "Xsl File not found.");
            }

            [Fact]
            public void Should_Throw_If_Result_Exists_And_Overwrite_False()
            {
                // Given
                var fixture = new XmlTransformationFixture(resultExist: true);
                fixture.Settings.Overwrite = false;

                // When
                var result = Record.Exception(() => fixture.Transform());

                // Then
                AssertEx.IsExceptionWithMessage<CakeException>(result, "Result file found and overwrite set to false.");
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
            [Fact]
            public void Should_Transform_Xml_File_And_Xsl_File_To_Result_File()
            {
                // Given
                var fixture = new XmlTransformationFixture
                {
                    ResultPath = "/Working/breakfast_menu.htm"
                };

                // When
                fixture.Transform();

                // Then
                var resultFile = fixture.FileSystem.GetFile(fixture.ResultPath);
                Assert.Equal(true, resultFile.Exists);
                string resultString;
                using (var resultStream = resultFile.OpenRead())
                {
                    using (var streamReader = new StreamReader(resultStream, Encoding.UTF8))
                    {
                        resultString = streamReader.ReadToEnd();
                    }
                }
                Assert.Equal(Resources.XmlTransformation_Htm, resultString, ignoreLineEndingDifferences: true);
            }

            [Fact]
            public void Should_Transform_Xml_String_And_Xsl_String_To_Result_String()
            {
                // Given
                var xml = Resources.XmlTransformation_Xml;
                var xsl = Resources.XmlTransformation_Xsl;
                var htm = Resources.XmlTransformation_Htm_NoXmlDeclaration;

                // When
                var result = XmlTransformation.Transform(xsl, xml);

                // Then
                Assert.Equal(htm, result, ignoreLineEndingDifferences: true);
            }

            [Fact]
            public void Should_Transform_Xml_String_And_Xsl_String_To_Result_String_With_Xml_Declaration()
            {
                // Given
                var xml = Resources.XmlTransformation_Xml;
                var xsl = Resources.XmlTransformation_Xsl;
                var htm = Resources.XmlTransformation_Htm;
                var settings = new XmlTransformationSettings
                {
                    Encoding = new UTF8Encoding(false)
                };

                // When
                var result = XmlTransformation.Transform(xsl, xml, settings);

                // Then
                Assert.Equal(htm, result, ignoreLineEndingDifferences: true);
            }

            [Fact]
            public void Should_Transform_Xml_String_And_Xsl_String_To_Result_String_With_Utf32Xml_Declaration()
            {
                // Given
                var xml = Resources.XmlTransformation_Xml;
                var xsl = Resources.XmlTransformation_Xsl;
                var settings = new XmlTransformationSettings
                {
                    Encoding = new UTF32Encoding(false, false, true)
                };

                // When
                var result = string.Concat(XmlTransformation.Transform(xsl, xml, settings).Take(39));

                // Then
                Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-32\"?>", result);
            }

            [Fact]
            public void Should_Throw_If_Xml_Was_Null()
            {
                // Given
                var xsl = Resources.XmlTransformation_Xsl;

                // When
                var result = Record.Exception(() => XmlTransformation.Transform(xsl, null));

                // Then
                AssertEx.IsArgumentNullException(result, "xml");
            }

            [Fact]
            public void Should_Throw_If_Xsl_Was_Null()
            {
                // Given
                var xml = Resources.XmlTransformation_Xml;

                // When
                var result = Record.Exception(() => XmlTransformation.Transform(null, xml));

                // Then
                AssertEx.IsArgumentNullException(result, "xsl");
            }

            [Fact]
            public void Should_Throw_If_String_Settings_Was_Null()
            {
                // Given
                var xml = Resources.XmlTransformation_Xml;
                var xsl = Resources.XmlTransformation_Xsl;

                // When
                var result = Record.Exception(() => XmlTransformation.Transform(xsl, xml, null));

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }
        }
    }
}
#endif