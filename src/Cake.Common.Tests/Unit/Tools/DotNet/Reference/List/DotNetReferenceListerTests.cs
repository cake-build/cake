// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Reference.List;
using Cake.Common.Tools.DotNet;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Reference.List
{
    public sealed class DotNetReferenceListerTests
    {
        public sealed class TheListMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetReferenceListerFixture();
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
                var fixture = new DotNetReferenceListerFixture();
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
                var fixture = new DotNetReferenceListerFixture();
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Not_Add_Project_Argument()
            {
                // Given
                var fixture = new DotNetReferenceListerFixture();
                fixture.Project = null;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("list reference", result.Args);
            }

            [Fact]
            public void Should_Add_Project_Argument()
            {
                // Given
                var fixture = new DotNetReferenceListerFixture();
                fixture.Project = "ToDo.csproj";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("list \"ToDo.csproj\" reference", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetReferenceListerFixture();
                fixture.Project = "ToDo.csproj";
                fixture.Settings.Verbosity = DotNetVerbosity.Diagnostic;

                // When
                var result = fixture.Run();

                // Then
                var expected = "list \"ToDo.csproj\" reference --verbosity diagnostic";
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Return_Correct_List_Of_References()
            {
                // Given
                var fixture = new DotNetReferenceListerFixture();
                fixture.GivenProjectReferencesResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Collection(fixture.References,
                    item =>
                    {
                        Assert.Equal(item, "..\\..\\Common\\Common.AspNetCore\\Common.AspNetCore.csproj");
                    },
                    item =>
                    {
                        Assert.Equal(item, "..\\..\\Common\\Common.Messaging\\Common.Messaging.csproj");
                    },
                    item =>
                    {
                        Assert.Equal(item, "..\\..\\Common\\Common.Utilities\\Common.Utilities.csproj");
                    });
            }

            [Fact]
            public void Should_Return_Empty_List_Of_References()
            {
                // Given
                var fixture = new DotNetReferenceListerFixture();
                fixture.GivenEmptyProjectReferencesResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Empty(fixture.References);
            }
        }
    }
}
