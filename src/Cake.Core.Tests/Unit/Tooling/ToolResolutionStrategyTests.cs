// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Tests.Fixtures;
using Cake.Testing;
using Xunit;

namespace Cake.Core.Tests.Unit.Tooling
{
    public sealed class ToolResolutionStrategyTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();
                fixture.FileSystem = null;

                // When
                var result = Record.Exception(() => fixture.Resolve("tool.exe"));

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.Resolve("tool.exe"));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Globber_Is_Null()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();
                fixture.Globber = null;

                // When
                var result = Record.Exception(() => fixture.Resolve("tool.exe"));

                // Then
                AssertEx.IsArgumentNullException(result, "globber");
            }
        }

        public sealed class TheResolveMethod
        {
            [Fact]
            public void Should_Throw_If_Tool_Repository_Is_Null()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();
                fixture.Repository = null;

                // When
                var result = Record.Exception(() => fixture.Resolve("tool.exe"));

                // Then
                AssertEx.IsArgumentNullException(result, "repository");
            }

            [Fact]
            public void Should_Throw_If_Tool_Name_Is_Null()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();

                // When
                var result = Record.Exception(() => fixture.Resolve(null));

                // Then
                AssertEx.IsArgumentNullException(result, "tool");
            }

            [Theory]
            [InlineData("")]
            [InlineData(" ")]
            [InlineData("\t")]
            public void Should_Throw_If_Tool_Name_Is_Empty(string tool)
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();

                // When
                var result = Record.Exception(() => fixture.Resolve(tool));

                // Then
                AssertEx.IsArgumentException(result, "tool", "Tool name cannot be empty.");
            }

            [Fact]
            public void Should_Prefer_To_Resolve_Tool_From_Repository_If_Possible()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();
                fixture.Repository.Register("./tool.exe");
                fixture.FileSystem.CreateFile("/Working/tools/tool.exe");
                fixture.FileSystem.CreateFile("/Working/temp/tool.exe");
                fixture.Environment.SetEnvironmentVariable("PATH", "/Working/temp");

                // When
                var result = fixture.Resolve("tool.exe");

                // Then
                Assert.Equal("/Working/tool.exe", result.FullPath);
            }

            [Fact]
            public void Should_Resolve_Tool_From_Tools_Directory_If_Not_Present_In_Repository()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();
                fixture.FileSystem.CreateFile("/Working/tools/tool.exe");
                fixture.FileSystem.CreateFile("/Working/temp/tool.exe");
                fixture.Environment.SetEnvironmentVariable("PATH", "/Working/temp");

                // When
                var result = fixture.Resolve("tool.exe");

                // Then
                Assert.Equal("/Working/tools/tool.exe", result.FullPath);
            }

            [Fact]
            public void Should_Resolve_Tool_From_Tools_Directory_Specified_In_Configuration_If_Not_Present_In_Repository()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();
                fixture.Configuration.SetValue(Constants.Paths.Tools, "./stuff");
                fixture.FileSystem.CreateFile("/Working/stuff/tool.exe");
                fixture.FileSystem.CreateFile("/Working/temp/tool.exe");
                fixture.Environment.SetEnvironmentVariable("PATH", "/Working/temp");

                // When
                var result = fixture.Resolve("tool.exe");

                // Then
                Assert.Equal("/Working/stuff/tool.exe", result.FullPath);
            }

            [Fact]
            public void Should_Resolve_Tool_From_Environment_Variable_If_Not_Present_In_Tools_Directory()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();
                fixture.FileSystem.CreateFile("/Working/temp/tool.exe");
                fixture.Environment.SetEnvironmentVariable("PATH", "/Working/temp");

                // When
                var result = fixture.Resolve("tool.exe");

                // Then
                Assert.Equal("/Working/temp/tool.exe", result.FullPath);
            }

            [Fact]
            public void Should_Return_Null_If_Tool_Could_Not_Be_Resolved()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();

                // When
                var result = fixture.Resolve("tool.exe");

                // Then
                Assert.Null(result);
            }
        }
    }
}