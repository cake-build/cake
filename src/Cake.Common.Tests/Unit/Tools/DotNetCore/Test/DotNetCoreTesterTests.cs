// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Tools.DotNetCore.Test;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.Test
{
    public sealed class DotNetCoreTesterTests
    {
        public sealed class TheTestMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreTesterFixture();
                fixture.Project = "./src/*";
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCoreTesterFixture();
                fixture.Project = "./src/*";
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, ".NET Core CLI: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DotNetCoreTesterFixture();
                fixture.Project = "./src/*";
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, ".NET Core CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetCoreTesterFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("test", result.Args);
            }

            [Fact]
            public void Should_Add_Path()
            {
                // Given
                var fixture = new DotNetCoreTesterFixture();
                fixture.Project = "./test/Project.Tests/";
                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("test ./test/Project.Tests/", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Settings()
            {
                // Given
                var fixture = new DotNetCoreTesterFixture();
                fixture.Settings.BuildBasePath = "./temp/";
                fixture.Settings.NoBuild = true;
                fixture.Settings.Framework = "dnxcore50";
                fixture.Settings.Runtime = "runtime1";
                fixture.Settings.Configuration = "Release";
                fixture.Settings.OutputDirectory = "./artifacts/";
                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("test --output \"/Working/artifacts\" --build-base-path \"/Working/temp\" --runtime runtime1 --framework dnxcore50 --configuration Release --no-build", result.Args);
            }
        }
    }
}
