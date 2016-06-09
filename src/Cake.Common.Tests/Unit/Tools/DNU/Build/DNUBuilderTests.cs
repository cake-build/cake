// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Tools.DNU.Build;
using Cake.Common.Tools.DNU.Build;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DNU.Build
{
    public sealed class DNUBuilderTests
    {
        public sealed class TheBuildMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DNUBuilderFixture();
                fixture.Path = "./src/*";
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Path_Are_Null()
            {
                // Given
                var fixture = new DNUBuilderFixture();
                fixture.Settings = new DNUBuildSettings();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Throw_If_DNU_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new DNUBuilderFixture();
                fixture.Path = "./src/*";
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "DNU: Could not locate executable.");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DNUBuilderFixture();
                fixture.Path = "./src/*";
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "DNU: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DNUBuilderFixture();
                fixture.Path = "./src/*";
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "DNU: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DNUBuilderFixture();
                fixture.Path = "./src/*";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("build \"./src/*\"", result.Args);
            }

            [Fact]
            public void Should_Add_Frameworks_If_Set()
            {
                // Given
                var fixture = new DNUBuilderFixture();
                fixture.Path = "./src/*";
                fixture.Settings = new DNUBuildSettings
                {
                    Frameworks = new[] { "dnx451", "dnxcore50" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("build \"./src/*\" --framework \"dnx451\" --framework \"dnxcore50\"", result.Args);
            }

            [Fact]
            public void Should_Add_Configurations_If_Set()
            {
                // Given
                var fixture = new DNUBuilderFixture();
                fixture.Path = "./src/*";
                fixture.Settings = new DNUBuildSettings
                {
                    Configurations = new[] { "Debug", "Release" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("build \"./src/*\" --configuration \"Debug\" --configuration \"Release\"", result.Args);
            }

            [Fact]
            public void Should_Add_Proxy_If_Set()
            {
                // Given
                var fixture = new DNUBuilderFixture();
                fixture.Path = "./src/*";
                fixture.Settings = new DNUBuildSettings
                {
                    OutputDirectory = "./artifacts/"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("build \"./src/*\" --out \"/Working/artifacts\"", result.Args);
            }

            [Fact]
            public void Should_Add_Quiet_If_Set()
            {
                // Given
                var fixture = new DNUBuilderFixture();
                fixture.Path = "./src/*";
                fixture.Settings = new DNUBuildSettings
                {
                    Quiet = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("build \"./src/*\" --quiet", result.Args);
            }

            [Fact]
            public void Should_Add_Arguments_And_Settings_If_Set()
            {
                // Given
                var fixture = new DNUBuilderFixture();
                fixture.Path = "./src/*";
                fixture.Settings = new DNUBuildSettings
                {
                    Frameworks = new[] { "dnx451", "dnxcore50" },
                    Configurations = new[] { "Debug", "Release" },
                    OutputDirectory = "./artifacts/",
                    Quiet = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("build \"./src/*\"" +
                             " --framework \"dnx451\" --framework \"dnxcore50\"" +
                             " --configuration \"Debug\" --configuration \"Release\"" +
                             " --out \"/Working/artifacts\"" +
                             " --quiet",
                    result.Args);
            }
        }
    }
}
