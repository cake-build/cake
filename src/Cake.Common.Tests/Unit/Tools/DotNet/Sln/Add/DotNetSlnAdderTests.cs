// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Tools.DotNet.Sln.Add;
using Cake.Common.Tools.DotNet;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Sln.Add
{
    public sealed class DotNetSlnAdderTests
    {
        public sealed class TheAddMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetSlnAdderFixture();
                fixture.ProjectPath = new[] { (FilePath)"./lib1.csproj" };
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DotNetSlnAdderFixture();
                fixture.ProjectPath = new[] { (FilePath)"./lib1.csproj" };
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Throw_If_ProjectPath_Is_Null()
            {
                // Given
                var fixture = new DotNetSlnAdderFixture();
                fixture.ProjectPath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "projectPath");
            }

            [Fact]
            public void Should_Throw_If_ProjectPath_Is_Empty()
            {
                // Given
                var fixture = new DotNetSlnAdderFixture();
                fixture.ProjectPath = new FilePath[] { };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "projectPath");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetSlnAdderFixture();
                fixture.ProjectPath = new[] { (FilePath)"./lib1.csproj" };
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_InRoot_And_SolutionFolder_Are_Used_Together()
            {
                // Given
                var fixture = new DotNetSlnAdderFixture();
                fixture.ProjectPath = new[] { (FilePath)"./lib1.csproj" };
                fixture.Settings.InRoot = true;
                fixture.Settings.SolutionFolder = "mylibs";

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("InRoot and SolutionFolder cannot be used together.", result.Message);
            }

            [Fact]
            public void Should_Add_Solution_Argument()
            {
                // Given
                var fixture = new DotNetSlnAdderFixture();
                fixture.Solution = (FilePath)"test.sln";
                fixture.ProjectPath = new[] { (FilePath)"./lib1.csproj" };

                // When
                var result = fixture.Run();

                // Then
                Assert.NotNull(result);
                Assert.Equal("sln \"/Working/test.sln\" add \"/Working/lib1.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Not_Add_Solution_Argument()
            {
                // Given
                var fixture = new DotNetSlnAdderFixture();
                fixture.Solution = null;
                fixture.ProjectPath = new[] { (FilePath)"./lib1.csproj" };

                // When
                var result = fixture.Run();

                // Then
                Assert.NotNull(result);
                Assert.Equal("sln add \"/Working/lib1.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Add_ProjectPath_Argument()
            {
                // Given
                var fixture = new DotNetSlnAdderFixture();
                fixture.ProjectPath = new[] { (FilePath)"./lib1.csproj" };

                // When
                var result = fixture.Run();

                // Then
                Assert.NotNull(result);
                Assert.Equal("sln add \"/Working/lib1.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Add_All_ProjectPath()
            {
                // Given
                var fixture = new DotNetSlnAdderFixture();
                fixture.ProjectPath = new[] { (FilePath)"./lib1.csproj", "./lib2.csproj", "./lib3.csproj" };

                // When
                var result = fixture.Run();

                // Then
                Assert.NotNull(result);
                Assert.Equal("sln add \"/Working/lib1.csproj\" \"/Working/lib2.csproj\" \"/Working/lib3.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Add_InRoot_Argument()
            {
                // Given
                var fixture = new DotNetSlnAdderFixture();
                fixture.ProjectPath = new[] { (FilePath)"./lib1.csproj" };
                fixture.Settings.InRoot = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.NotNull(result);
                Assert.Equal("sln add --in-root \"/Working/lib1.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Add_SolutionFolder_Argument()
            {
                // Given
                var fixture = new DotNetSlnAdderFixture();
                fixture.ProjectPath = new[] { (FilePath)"./lib1.csproj" };
                fixture.Settings.SolutionFolder = "mylibs";

                // When
                var result = fixture.Run();

                // Then
                Assert.NotNull(result);
                Assert.Equal("sln add --solution-folder \"/Working/mylibs\" \"/Working/lib1.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetSlnAdderFixture();
                fixture.Solution = (FilePath)"test.sln";
                fixture.ProjectPath = new[] { (FilePath)"./lib1.csproj" };
                fixture.Settings.Verbosity = DotNetVerbosity.Detailed;

                // When
                var result = fixture.Run();

                // Then
                Assert.NotNull(result);
                Assert.Equal("sln \"/Working/test.sln\" add \"/Working/lib1.csproj\" --verbosity detailed", result.Args);
            }
        }
    }
}
