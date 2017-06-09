// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Cake.Common.Solution;
using Cake.Common.Tests.Fixtures.Solution;
using Cake.Common.Tests.Properties;
using Xunit;

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
        public sealed class TheParseMethod
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
            public void Should_Properly_Parse_Projects()
            {
                // Given
                var fixture = new SolutionParserFixture();
                var slnFilePath = fixture.WithSolutionFile(Resources.Solution_WithProjectsAndFolders);
                var solutionParser = new SolutionParser(fixture.FileSystem, fixture.Environment);

                // When
                var result = solutionParser.Parse(slnFilePath);

                // Then
                Assert.NotNull(result);
                Assert.NotNull(result.Projects);
                Assert.Equal(5, result.Projects.Count);
                var onlyProjects = result.Projects.Where(x => !(x is SolutionFolder)).ToList();
                Assert.Equal(3, onlyProjects.Count);
            }

            [Fact]
            public void Should_Properly_Parse_Folders()
            {
                // Given
                var fixture = new SolutionParserFixture();
                var slnFilePath = fixture.WithSolutionFile(Resources.Solution_WithProjectsAndFolders);
                var solutionParser = new SolutionParser(fixture.FileSystem, fixture.Environment);

                // When
                var result = solutionParser.Parse(slnFilePath);

                // Then
                Assert.NotNull(result);
                Assert.NotNull(result.Projects);
                var folders = result.Projects.OfType<SolutionFolder>().ToList();
                Assert.Equal(2, folders.Count);
            }

            [Fact]
            public void Should_Properly_Parse_Relation_Between_Project_And_Folder()
            {
                // Given
                var fixture = new SolutionParserFixture();
                var slnFilePath = fixture.WithSolutionFile(Resources.Solution_WithProjectsAndFolders);
                var solutionParser = new SolutionParser(fixture.FileSystem, fixture.Environment);

                // When
                var result = solutionParser.Parse(slnFilePath);

                // Then
                Assert.NotNull(result);
                Assert.NotNull(result.Projects);
                var folders = result.Projects.OfType<SolutionFolder>().ToList();
                var srcFolder = folders.First(x => x.Name == "src");
                Assert.Equal(1, srcFolder.Items.Count);
                var dummyProject = result.Projects.First(x => x.Name == "dummy");
                Assert.Contains(dummyProject, srcFolder.Items);
                Assert.Equal(srcFolder, dummyProject.Parent);
            }
        }
    }
}