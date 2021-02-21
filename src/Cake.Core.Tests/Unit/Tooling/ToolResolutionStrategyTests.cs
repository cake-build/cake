// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tests.Fixtures;
using Cake.Core.Tooling;
using Cake.Testing;
using Cake.Testing.Xunit;
using NSubstitute;
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
                var result = Record.Exception(() => fixture.Resolve((string)null));

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
            public void Should_Throw_If_ToolExeNames_Is_Null()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();

                // When
                var result = Record.Exception(() => fixture.Resolve((IEnumerable<string>)null));

                // Then
                AssertEx.IsArgumentNullException(result, "toolExeNames");
            }

            [Theory]
            [InlineData("")]
            [InlineData(" ")]
            [InlineData("\t")]
            public void Should_Throw_If_ToolExeNames_Is_Empty(string tool)
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();

                // When
                var result = Record.Exception(() => fixture.Resolve(new[] { tool }));

                // Then
                AssertEx.IsArgumentException(result, "toolExeNames", "Tool names cannot be empty.");
            }

            [Fact]
            public void Should_Prefer_Tool_From_Repository()
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
            public void Should_Return_Tool_From_Tools_Directory_If_Not_Present_In_Repository()
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
            public void Should_Return_Tool_From_Tools_Directory_Specified_In_Configuration_If_Not_Present_In_Repository()
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
            public void Should_Return_Tool_From_Environment_Variable_If_Not_Present_In_Tools_Directory()
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

            [Fact]
            public void Should_Return_Tool_From_Environment_Variable_Gracefully_Proceed_If_FileSystem_Throw_Exception()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();
                fixture.Environment.SetEnvironmentVariable("PATH", "/Working/fail:/Working/temp");
                fixture.FileSystem.CreateFile("/Working/temp/tool.exe");

                var fileSystem = Substitute.For<IFileSystem>();
                fileSystem.GetFile(Arg.Any<FilePath>()).Returns(call =>
                {
                    var path = call.Arg<FilePath>();
                    if (path.FullPath == "/Working/fail/tool.exe")
                    {
                        throw new Exception("Error!");
                    }
                    return fixture.FileSystem.GetFile(path);
                });

                var strategy = new ToolResolutionStrategy(fileSystem, fixture.Environment, fixture.Globber, fixture.Configuration, new NullLog());

                // When
                var result = strategy.Resolve(fixture.Repository, "tool.exe");

                // Then
                Assert.Equal("/Working/temp/tool.exe", result.FullPath);
                fileSystem.Received().GetFile(Arg.Is<FilePath>(p => p.FullPath == "/Working/fail/tool.exe"));
                fileSystem.Received().GetFile(Arg.Is<FilePath>(p => p.FullPath == "/Working/temp/tool.exe"));
            }

            [Fact]
            public void Should_Prefer_ToolExeNames_From_Repository()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();
                fixture.Repository.Register("./dotnet-tool.exe");
                fixture.FileSystem.CreateFile("/Working/tools/tool.exe");
                fixture.FileSystem.CreateFile("/Working/temp/tool.exe");
                fixture.Environment.SetEnvironmentVariable("PATH", "/Working/temp");

                // When
                var result = fixture.Resolve(new[] { "tool.exe", "dotnet-tool", "dotnet-tool.exe" });

                // Then
                Assert.Equal("/Working/dotnet-tool.exe", result.FullPath);
            }

            [Theory]
            [InlineData("tool.bat")]
            [InlineData("tool.cmd")]
            [InlineData("tool.exe")]
            public void Should_Prefer_ToolExeNames_With_Unix_Platform_Affinity_When_Unix_Environment(string windowsTool)
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();
                fixture.Repository.Register($"./{windowsTool}");
                fixture.Repository.Register("./tool");

                // When
                var result = fixture.Resolve(new[] { windowsTool, "tool" });

                // Then
                Assert.Equal("/Working/tool", result.FullPath);
            }

            [WindowsTheory]
            [InlineData("tool.bat")]
            [InlineData("tool.cmd")]
            [InlineData("tool.exe")]
            public void Should_Prefer_ToolExeNames_With_Windows_Platform_Affinity_When_Windows_Environment(string windowsTool)
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture(FakeEnvironment.CreateWindowsEnvironment());
                fixture.Repository.Register("./tool");
                fixture.Repository.Register($"./{windowsTool}");

                // When
                var result = fixture.Resolve(new[] { "tool", windowsTool });

                // Then
                Assert.Equal($"C:/Working/{windowsTool}", result.FullPath);
            }

            [Fact]
            public void Should_Return_ToolExeNames_From_Tools_Directory_If_Not_Present_In_Repository()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();
                fixture.FileSystem.CreateFile("/Working/tools/dotnet-tool.exe");
                fixture.FileSystem.CreateFile("/Working/temp/tool.exe");
                fixture.Environment.SetEnvironmentVariable("PATH", "/Working/temp");

                // When
                var result = fixture.Resolve(new[] { "tool.exe", "dotnet-tool", "dotnet-tool.exe" });

                // Then
                Assert.Equal("/Working/tools/dotnet-tool.exe", result.FullPath);
            }

            [Fact]
            public void Should_Return_ToolExeNames_From_Tools_Directory_Specified_In_Configuration_If_Not_Present_In_Repository()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();
                fixture.Configuration.SetValue(Constants.Paths.Tools, "./stuff");
                fixture.FileSystem.CreateFile("/Working/stuff/dotnet-tool.exe");
                fixture.FileSystem.CreateFile("/Working/temp/tool.exe");
                fixture.Environment.SetEnvironmentVariable("PATH", "/Working/temp");

                // When
                var result = fixture.Resolve(new[] { "tool.exe", "dotnet-tool", "dotnet-tool.exe" });

                // Then
                Assert.Equal("/Working/stuff/dotnet-tool.exe", result.FullPath);
            }

            [Fact]
            public void Should_Return_ToolExeNames_From_Environment_Variable_If_Not_Present_In_Tools_Directory()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();
                fixture.FileSystem.CreateFile("/Working/temp/dotnet-tool.exe");
                fixture.Environment.SetEnvironmentVariable("PATH", "/Working/temp");

                // When
                var result = fixture.Resolve(new[] { "tool.exe", "dotnet-tool", "dotnet-tool.exe" });

                // Then
                Assert.Equal("/Working/temp/dotnet-tool.exe", result.FullPath);
            }

            [Fact]
            public void Should_Return_Null_If_ToolExeNames_Could_Not_Be_Resolved()
            {
                // Given
                var fixture = new ToolResolutionStrategyFixture();

                // When
                var result = fixture.Resolve(new[] { "tool.exe", "dotnet-tool", "dotnet-tool.exe" });

                // Then
                Assert.Null(result);
            }
        }
    }
}