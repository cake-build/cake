// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Sln.List;
using Cake.Common.Tools.DotNet;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Sln.List
{
    public sealed class DotNetSlnListerTests
    {
        public sealed class TheListMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetSlnListerFixture();
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
                var fixture = new DotNetSlnListerFixture();
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
                var fixture = new DotNetSlnListerFixture();
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Not_Add_Solution_Argument()
            {
                // Given
                var fixture = new DotNetSlnListerFixture();
                fixture.Solution = null;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("sln list", result.Args);
            }

            [Fact]
            public void Should_Add_Solution_Argument()
            {
                // Given
                var fixture = new DotNetSlnListerFixture();
                fixture.Solution = "ToDo.sln";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("sln \"/Working/ToDo.sln\" list", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetSlnListerFixture();
                fixture.Solution = "ToDo.sln";
                fixture.Settings.Verbosity = DotNetVerbosity.Diagnostic;

                // When
                var result = fixture.Run();

                // Then
                var expected = "sln \"/Working/ToDo.sln\" list --verbosity diagnostic";
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Return_Correct_List_Of_Projects()
            {
                // Given
                var fixture = new DotNetSlnListerFixture();
                fixture.GivenProjectsResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Collection(fixture.Projects,
                    item =>
                    {
                        Assert.Equal(item, "Common\\Common.AspNetCore\\Common.AspNetCore.csproj");
                    },
                    item =>
                    {
                        Assert.Equal(item, "Common\\Common.Messaging\\Common.Messaging.csproj");
                    },
                    item =>
                    {
                        Assert.Equal(item, "Common\\Common.Utilities\\Common.Utilities.csproj");
                    });
            }

            [Fact]
            public void Should_Return_StandardError_ExitCode()
            {
                const string expectedStandardError = "Specified solution file C:\\Cake\\Cake.Core\\ does not exist, or there is no solution file in the directory.";

                // Given
                var fixture = new DotNetSlnListerFixture();
                fixture.StandardError = expectedStandardError;
                fixture.GivenErrorResult();

                // When
                fixture.Run();

                // Then
                Assert.Equal(expectedStandardError, fixture.StandardError);
            }
        }
    }
}
