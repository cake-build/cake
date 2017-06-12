// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Common.Solution.Project;
using Cake.Common.Tests.Fixtures.Solution.Project;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Solution.Project
{
    public sealed class ProjectParserTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();

                // When
                var result = Record.Exception(() => new ProjectParser(null, environment));

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();

                // When
                var result = Record.Exception(() => new ProjectParser(fileSystem, null));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheParseMethod
        {
            [Fact]
            public void Should_Return_Parser_Result()
            {
                // Given
                var fixture = new ProjectParserFixture();

                // When
                var result = fixture.Parse();

                // Then
                Assert.NotNull(result);
            }

            [Fact]
            public void Should_Parse_Framework_Version()
            {
                // Given
                var fixture = new ProjectParserFixture();

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(result.TargetFrameworkVersion, "v4.5");
            }

            [Fact]
            public void Should_Parse_Framework_Profile()
            {
                // Given
                var fixture = new ProjectParserFixture();

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal("Profile111", result.TargetFrameworkProfile);
            }

            [Fact]
            public void Should_Return_Null_When_Profile_Not_Defined()
            {
                // Given
                var fixture = new ProjectParserFixture();

                // When
                var result = fixture.ParseIncomplete();

                // Then
                Assert.NotNull(result);
                Assert.Null(result.TargetFrameworkProfile);
            }

            [Fact]
            public void Should_Parse_Assembly_Name()
            {
                // Given
                var fixture = new ProjectParserFixture();

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(result.AssemblyName, "Cake.Common");
            }

            [Fact]
            public void Should_Parse_Namespace()
            {
                // Given
                var fixture = new ProjectParserFixture();

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(result.RootNameSpace, "Cake.Common");
            }

            [Fact]
            public void Should_Parse_Output_Type()
            {
                // Given
                var fixture = new ProjectParserFixture();

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(result.OutputType, "Library");
            }

            [Fact]
            public void Should_Parse_Configuration()
            {
                // Given
                var fixture = new ProjectParserFixture();

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(result.Configuration, "Debug");
            }

            [Fact]
            public void Should_Parse_Platform()
            {
                // Given
                var fixture = new ProjectParserFixture();

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(result.Platform, "AnyCPU");
            }

            [Fact]
            public void Should_Parse_OutputAssembly()
            {
                // Given
                var fixture = new ProjectParserFixture();

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(@"bin/Debug", result.OutputPath.FullPath);
            }

            [Fact]
            public void Should_Return_Correct_File_Count()
            {
                // Given
                var fixture = new ProjectParserFixture();

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(2, result.Files.Count);
            }

            [Fact]
            public void Should_Return_Valid_Guid()
            {
                // Given
                var fixture = new ProjectParserFixture();

                // When
                var result = fixture.Parse();

                // Then
                Guid projectGuid;
                var parseResult = Guid.TryParseExact(result.ProjectGuid, "B", out projectGuid);
                Assert.True(parseResult);
            }

            [Fact]
            public void Should_Return_References()
            {
                // Given
                var fixture = new ProjectParserFixture();

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(1, result.References.Count);
                Assert.Equal("System.Collections.Immutable, Version=1.1.37.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL",
                    result.References.First().Include);
                Assert.Equal("/Working/../packages/System.Collections.Immutable.1.1.37/lib/dotnet/System.Collections.Immutable.dll",
                    result.References.First().HintPath.FullPath);
                Assert.Equal(true, result.References.First().Private);
            }

            [Fact]
            public void Should_Return_Project_References()
            {
                // Given
                var fixture = new ProjectParserFixture();

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(1, result.ProjectReferences.Count);
                Assert.Equal("/Working/../Cake.Common/Cake.Common.csproj", result.ProjectReferences.First().FilePath.FullPath);
                Assert.Equal("..\\Cake.Common\\Cake.Common.csproj", result.ProjectReferences.First().RelativePath);
                Assert.Equal("{ABC3F1CB-F84E-43ED-A120-0CCFE344D250}", result.ProjectReferences.First().Project);
                Assert.Equal("Cake.Common", result.ProjectReferences.First().Name);
            }
        }
    }
}