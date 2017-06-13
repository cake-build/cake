// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNetCore.Build;
using Cake.Common.Tests.Fixtures.Tools.DotNetCore.Clean;
using Cake.Common.Tools.DotNetCore.Build;
using Cake.Common.Tools.DotNetCore.Clean;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.Clean
{
    public sealed class DotNetCoreCleanTests
    {
        public sealed class TheCleanMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreCleanerFixture();
                fixture.Project = "./src/project";
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Project_Is_Null()
            {
                // Given
                var fixture = new DotNetCoreCleanerFixture();
                fixture.Settings = new DotNetCoreCleanSettings();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "project");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCoreCleanerFixture();
                fixture.Project = "./src/project";
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET Core CLI: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DotNetCoreCleanerFixture();
                fixture.Project = "./src/project";
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET Core CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetCoreCleanerFixture();
                fixture.Project = "./src/project";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("clean \"./src/project\"", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetCoreCleanerFixture();
                fixture.Settings.Framework = "net451";
                fixture.Settings.Configuration = "Release";
                fixture.Project = "./src/project";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("clean \"./src/project\" --framework net451 --configuration Release", result.Args);
            }

            [Fact]
            public void Should_Add_OutputPath_Arguments()
            {
                // Given
                var fixture = new DotNetCoreCleanerFixture();
                fixture.Settings.OutputDirectory = "./artifacts/";
                fixture.Project = "./src/project";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("clean \"./src/project\" --output \"/Working/artifacts\"", result.Args);
            }

            [Theory]
            [InlineData("./src/project", "clean \"./src/project\"")]
            [InlineData("./src/cake build/", "clean \"./src/cake build/\"")]
            [InlineData("./src/cake build/cake cli", "clean \"./src/cake build/cake cli\"")]
            public void Should_Quote_Project_Path(string text, string expected)
            {
                // Given
                var fixture = new DotNetCoreCleanerFixture();
                fixture.Project = text;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetCoreCleanerFixture();
                fixture.Project = "./src/project";
                fixture.Settings.DiagnosticOutput = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics clean \"./src/project\"", result.Args);
            }
        }
    }
}