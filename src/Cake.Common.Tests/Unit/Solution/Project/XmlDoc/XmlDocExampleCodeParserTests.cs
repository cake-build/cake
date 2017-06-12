// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq;
using Cake.Common.Tests.Fixtures;
using Xunit;

namespace Cake.Common.Tests.Unit.Solution.Project.XmlDoc
{
    public sealed class XmlDocExampleCodeParserTests
    {
        public sealed class TheParseMethod
        {
            [Fact]
            public void Should_Throw_If_Xml_Path_Was_Null()
            {
                // Given
                var fixture = new XmlDocExampleCodeParserFixture();
                fixture.XmlFilePath = null;

                // When
                var result = Record.Exception(() => fixture.Parse());

                // Then
                AssertEx.IsArgumentNullException(result, "xmlFilePath");
            }

            [Fact]
            public void Should_Throw_If_Xml_Is_Missing()
            {
                // Given
                var fixture = new XmlDocExampleCodeParserFixture();
                fixture.XmlFilePath = "/Working/missing.xml";

                // When
                var result = Record.Exception(() => fixture.Parse());

                // Then
                Assert.IsType<FileNotFoundException>(result);
                Assert.Equal("Supplied xml file not found.", result?.Message);
            }

            [Fact]
            public void Should_Return_Correct_Number_Of_Examples()
            {
                // Given
                var fixture = new XmlDocExampleCodeParserFixture();

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(2, result.Count());
            }
        }

        public sealed class TheParseFilesMethod
        {
            [Fact]
            public void Should_Throw_If_Pattern_Was_Null()
            {
                // Given
                var fixture = new XmlDocExampleCodeParserFixture();
                fixture.Pattern = null;

                // When
                var result = Record.Exception(() => fixture.ParseFiles());

                // Then
                AssertEx.IsArgumentNullException(result, "pattern");
            }

            [Fact]
            public void Should_Throw_If_Pattern_Is_Empty()
            {
                // Given
                var fixture = new XmlDocExampleCodeParserFixture();
                fixture.Pattern = "";

                // When
                var result = Record.Exception(() => fixture.ParseFiles());

                // Then
                AssertEx.IsArgumentNullException(result, "pattern");
            }

            [Fact]
            public void Should_Return_Correct_Number_Of_Examples()
            {
                // Given
                var fixture = new XmlDocExampleCodeParserFixture();

                // When
                var result = fixture.ParseFiles();

                // Then
                Assert.Equal(4, result.Count());
            }
        }
    }
}