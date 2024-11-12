// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Tools.DotNet.Reference.Remove;
using Cake.Common.Tools.DotNet;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Reference.Remove
{
    public sealed class DotNetReferenceRemoverTests
    {
        public sealed class TheRemoveMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetReferenceRemoverFixture();
                fixture.ProjectReferences = new[] { (FilePath)"./test/unit.tests.csproj" };
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
                var fixture = new DotNetReferenceRemoverFixture();
                fixture.ProjectReferences = new[] { (FilePath)"./test/unit.tests.csproj" };
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetReferenceRemoverFixture();
                fixture.ProjectReferences = new[] { (FilePath)"./test/unit.tests.csproj" };
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_ProjectReferences_Is_Null()
            {
                // Given
                var fixture = new DotNetReferenceRemoverFixture();
                fixture.ProjectReferences = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "projectReferences");
            }

            [Fact]
            public void Should_Throw_If_ProjectReferences_Is_Empty()
            {
                // Given
                var fixture = new DotNetReferenceRemoverFixture();
                fixture.ProjectReferences = Array.Empty<FilePath>();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "projectReferences");
            }

            [Fact]
            public void Should_Not_Add_Project_Argument()
            {
                // Given
                var fixture = new DotNetReferenceRemoverFixture();
                fixture.ProjectReferences = new[] { (FilePath)"./lib1.csproj" };
                fixture.Project = null;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("remove reference \"/Working/lib1.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Add_All_Project_References()
            {
                // Given
                var fixture = new DotNetReferenceRemoverFixture();
                fixture.ProjectReferences = new[] { (FilePath)"./lib1.csproj", "./lib2/*.csproj" };
                fixture.Project = null;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("remove reference \"/Working/lib1.csproj\" \"/Working/lib2/*.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Add_Project_Argument()
            {
                // Given
                var fixture = new DotNetReferenceRemoverFixture();
                fixture.ProjectReferences = new[] { (FilePath)"./lib1.csproj" };
                fixture.Project = "ToDo.csproj";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("remove \"ToDo.csproj\" reference \"/Working/lib1.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetReferenceRemoverFixture();
                fixture.ProjectReferences = new[] { (FilePath)"./lib1.csproj" };
                fixture.Project = "ToDo.csproj";
                fixture.Settings.Framework = "net8.0";
                fixture.Settings.Verbosity = DotNetVerbosity.Diagnostic;

                // When
                var result = fixture.Run();

                // Then
                var expected = "remove \"ToDo.csproj\" reference \"/Working/lib1.csproj\" --framework net8.0 --verbosity diagnostic";
                Assert.Equal(expected, result.Args);
            }
        }
    }
}
