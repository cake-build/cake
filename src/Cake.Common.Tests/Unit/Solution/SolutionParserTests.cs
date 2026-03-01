// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Solution;
using Cake.Common.Tests.Fixtures.Solution;
using Cake.Common.Tests.Properties;

namespace Cake.Common.Tests.Unit.Solution
{
    public sealed class SolutionParserTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_FileSystem_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new SolutionParser(null, null));

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new SolutionParserFixture();

                // When
                var result = Record.Exception(() => new SolutionParser(fixture.FileSystem, null));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheParseMethodForSln
        {
            [Fact]
            public void Should_Throw_If_SolutionPath_Is_Null()
            {
                // Given
                var fixture = new SolutionParserFixture();
                var solutionParser = new SolutionParser(fixture.FileSystem, fixture.Environment);

                // When
                var result = Record.Exception(() => solutionParser.Parse(null));

                // Then
                AssertEx.IsArgumentNullException(result, "solutionPath");
            }

            [Fact]
            public async Task Should_Properly_Parse_Projects()
            {
                // Given
                var fixture = new SolutionParserFixture();
                var slnFilePath = fixture.WithSolutionFile(Resources.Solution_WithProjectsAndFolders);
                var solutionParser = new SolutionParser(fixture.FileSystem, fixture.Environment);

                // When
                var result = solutionParser.Parse(slnFilePath);

                // Then
                await Verify(result);
            }

            [Fact]
            public async Task Should_Properly_Parse_Folders()
            {
                // Given
                var fixture = new SolutionParserFixture();
                var slnFilePath = fixture.WithSolutionFile(Resources.Solution_WithProjectsAndFolders);
                var solutionParser = new SolutionParser(fixture.FileSystem, fixture.Environment);

                // When
                var result = solutionParser.Parse(slnFilePath);

                // Then
                await Verify(result);
            }

            [Fact]
            public async Task Should_Properly_Parse_Relation_Between_Project_And_Folder()
            {
                // Given
                var fixture = new SolutionParserFixture();
                var slnFilePath = fixture.WithSolutionFile(Resources.Solution_WithProjectsAndFolders);
                var solutionParser = new SolutionParser(fixture.FileSystem, fixture.Environment);

                // When
                var result = solutionParser.Parse(slnFilePath);

                // Then
                await Verify(result);
            }

            [Fact]
            public async Task Should_Properly_Parse_Projects_With_Empty_Lines()
            {
                // Given
                var fixture = new SolutionParserFixture();
                var slnFilePath = fixture.WithSolutionFile(Resources.Solution_WithProjectsAndFoldersAndMissingLine);
                var solutionParser = new SolutionParser(fixture.FileSystem, fixture.Environment);

                // When
                var result = solutionParser.Parse(slnFilePath);

                // Then
                await Verify(result);
            }

            [Fact]
            public async Task Should_Properly_Parse_Projects_With_Absolute_Path()
            {
                // Given
                var fixture = new SolutionParserFixture();
                var slnFilePath = fixture.WithSolutionFile(Resources.Solution_WithProjectUsingAbsolutePath);
                var solutionParser = new SolutionParser(fixture.FileSystem, fixture.Environment);

                // When
                var result = solutionParser.Parse(slnFilePath);

                // Then
                await Verify(result);
            }
        }

        public sealed class TheParseMethodForSlnx
        {
            [Fact]
            public async Task Should_Properly_Parse_Projects()
            {
                // Given
                var fixture = new SolutionParserFixture();
                var slnFilePath = fixture.WithXmlSolutionFile(Resources.SolutionXml_WithProjectsAndFolders);
                var solutionParser = new SolutionParser(fixture.FileSystem, fixture.Environment);

                // When
                var result = solutionParser.Parse(slnFilePath);

                // Then
                await Verify(result);
            }

            [Fact]
            public async Task Should_Properly_Parse_Folders()
            {
                // Given
                var fixture = new SolutionParserFixture();
                var slnFilePath = fixture.WithXmlSolutionFile(Resources.SolutionXml_WithProjectsAndFolders);
                var solutionParser = new SolutionParser(fixture.FileSystem, fixture.Environment);

                // When
                var result = solutionParser.Parse(slnFilePath);

                // Then
                await Verify(result);
            }

            [Fact]
            public async Task Should_Properly_Parse_Relation_Between_Project_And_Folder()
            {
                // Given
                var fixture = new SolutionParserFixture();
                var slnFilePath = fixture.WithXmlSolutionFile(Resources.SolutionXml_WithProjectsAndFolders);
                var solutionParser = new SolutionParser(fixture.FileSystem, fixture.Environment);

                // When
                var result = solutionParser.Parse(slnFilePath);

                // Then
                await Verify(result);
            }

            [Fact]
            public async Task Should_Properly_Parse_Projects_With_Empty_Lines()
            {
                // Given
                var fixture = new SolutionParserFixture();
                var slnFilePath = fixture.WithXmlSolutionFile(Resources.SolutionXml_WithProjectsAndFoldersAndAdditionalLines);
                var solutionParser = new SolutionParser(fixture.FileSystem, fixture.Environment);

                // When
                var result = solutionParser.Parse(slnFilePath);

                // Then
                await Verify(result);
            }

            [Fact]
            public async Task Should_Properly_Parse_Projects_With_Absolute_Path()
            {
                // Given
                var fixture = new SolutionParserFixture();
                var slnFilePath = fixture.WithXmlSolutionFile(Resources.SolutionXml_WithProjectUsingAbsolutePath);
                var solutionParser = new SolutionParser(fixture.FileSystem, fixture.Environment);

                // When
                var result = solutionParser.Parse(slnFilePath);

                // Then
                await Verify(result);
            }

            [Fact]
            public async Task Should_Properly_Parse_Projects_With_Different_Type_Id()
            {
                // Given
                var fixture = new SolutionParserFixture();
                var slnFilePath = fixture.WithXmlSolutionFile(Resources.SolutionXml_WithProjectWithDifferentTypeId);
                var solutionParser = new SolutionParser(fixture.FileSystem, fixture.Environment);

                // When
                var result = solutionParser.Parse(slnFilePath);

                // Then
                await Verify(result);
            }

            [Fact]
            public async Task Should_Properly_Parse_Solution_With_Nested_Solution_Folders()
            {
                // Given
                var fixture = new SolutionParserFixture();
                var slnFilePath = fixture.WithXmlSolutionFile(Resources.SolutionXml_WithNestedSolutionFolders);
                var solutionParser = new SolutionParser(fixture.FileSystem, fixture.Environment);

                // When
                var result = solutionParser.Parse(slnFilePath);

                // Then
                await Verify(result);
            }
        }
    }
}