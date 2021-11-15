// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNetCore.NuGet.Source;
using Cake.Common.Tools.DotNetCore.NuGet.Source;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.NuGet.Source
{
    public sealed class DotNetCoreNuGetSourcerTests
    {
        public sealed class TheAddSourceMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Name_Is_Null_Or_WhiteSpace(string name)
            {
                // Given
                var fixture = new DotNetCoreNuGetAddSourceFixture();
                fixture.Name = name;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "name", "Value cannot be null or whitespace.");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreNuGetAddSourceFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Settings_Source_Is_Null_Or_WhiteSpace(string source)
            {
                // Given
                var fixture = new DotNetCoreNuGetAddSourceFixture();
                fixture.Settings = new DotNetCoreNuGetSourceSettings { Source = source };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "Source", "Value cannot be null or whitespace.");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCoreNuGetAddSourceFixture();
                fixture.Settings = new DotNetCoreNuGetSourceSettings { Source = "source" };
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
                var fixture = new DotNetCoreNuGetAddSourceFixture();
                fixture.Settings = new DotNetCoreNuGetSourceSettings { Source = "source" };
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetAddSourceFixture();
                fixture.Settings = new DotNetCoreNuGetSourceSettings { Source = "source" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget add source \"source\" --name \"name\"", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetAddSourceFixture();
                fixture.Settings = new DotNetCoreNuGetSourceSettings
                {
                    Source = "source",
                    UserName = "username",
                    Password = "password",
                    StorePasswordInClearText = true,
                    ValidAuthenticationTypes = "basic,negotiate"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget add source \"source\" --name \"name\" --username \"username\" --password \"password\" --store-password-in-clear-text --valid-authentication-types \"basic,negotiate\"", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetAddSourceFixture();
                fixture.Settings = new DotNetCoreNuGetSourceSettings { DiagnosticOutput = true, Source = "source" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics nuget add source \"source\" --name \"name\"", result.Args);
            }
        }

        public sealed class TheDisableSourceMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Name_Is_Null_Or_WhiteSpace(string name)
            {
                // Given
                var fixture = new DotNetCoreNuGetDisableSourceFixture();
                fixture.Name = name;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "name", "Value cannot be null or whitespace.");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreNuGetDisableSourceFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCoreNuGetDisableSourceFixture();
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
                var fixture = new DotNetCoreNuGetDisableSourceFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetDisableSourceFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget disable source \"name\"", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetDisableSourceFixture();
                fixture.Settings = new DotNetCoreNuGetSourceSettings { DiagnosticOutput = true };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics nuget disable source \"name\"", result.Args);
            }
        }

        public sealed class TheEnableSourceMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Name_Is_Null_Or_WhiteSpace(string name)
            {
                // Given
                var fixture = new DotNetCoreNuGetEnableSourceFixture();
                fixture.Name = name;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "name", "Value cannot be null or whitespace.");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreNuGetEnableSourceFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCoreNuGetEnableSourceFixture();
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
                var fixture = new DotNetCoreNuGetEnableSourceFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetEnableSourceFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget enable source \"name\"", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetEnableSourceFixture();
                fixture.Settings = new DotNetCoreNuGetSourceSettings { DiagnosticOutput = true };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics nuget enable source \"name\"", result.Args);
            }
        }

        public sealed class TheHasSourceMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Name_Is_Null_Or_WhiteSpace(string name)
            {
                // Given
                var fixture = new DotNetCoreNuGetHasSourceFixture();
                fixture.Name = name;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "name", "Value cannot be null or whitespace.");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreNuGetHasSourceFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCoreNuGetHasSourceFixture();
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
                var fixture = new DotNetCoreNuGetHasSourceFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetHasSourceFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget list source --format \"detailed\"", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetHasSourceFixture();
                fixture.Settings = new DotNetCoreNuGetSourceSettings { DiagnosticOutput = true };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics nuget list source --format \"detailed\"", result.Args);
            }

            [Theory]
            [InlineData("nuget.org", "Enabled")]
            [InlineData("nuget.org", "Disabled")]
            [InlineData("Package source {0}1", "Enabled")]
            [InlineData("Package source {0}1", "Disabled")]
            public void Should_Return_True_For_Configured_Source(string name, string status)
            {
                // Given
                var fixture = new DotNetCoreNuGetHasSourceFixture();
                fixture.Name = name;
                fixture.GivenProcessOutput(new[]
                {
                    "Registered Sources:",
                    $"  1.  {name} [{status}]",
                    "      https://api.myfeed.org/v3/index.json"
                });

                // When
                var result = fixture.Run();

                // Then
                Assert.True(fixture.HasSource);
            }
        }

        public sealed class TheListSourceMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreNuGetListSourceFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCoreNuGetListSourceFixture();
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
                var fixture = new DotNetCoreNuGetListSourceFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetListSourceFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget list source", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetListSourceFixture();
                fixture.Format = "detailed";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget list source --format \"detailed\"", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetListSourceFixture();
                fixture.Settings = new DotNetCoreNuGetSourceSettings { DiagnosticOutput = true };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics nuget list source", result.Args);
            }
        }

        public sealed class TheRemoveSourceMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Name_Is_Null_Or_WhiteSpace(string name)
            {
                // Given
                var fixture = new DotNetCoreNuGetRemoveSourceFixture();
                fixture.Name = name;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "name", "Value cannot be null or whitespace.");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreNuGetRemoveSourceFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCoreNuGetRemoveSourceFixture();
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
                var fixture = new DotNetCoreNuGetRemoveSourceFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetRemoveSourceFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget remove source \"name\"", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetRemoveSourceFixture();
                fixture.Settings = new DotNetCoreNuGetSourceSettings { DiagnosticOutput = true };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics nuget remove source \"name\"", result.Args);
            }
        }

        public sealed class TheUpdateSourceMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Name_Is_Null_Or_WhiteSpace(string name)
            {
                // Given
                var fixture = new DotNetCoreNuGetUpdateSourceFixture();
                fixture.Name = name;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "name", "Value cannot be null or whitespace.");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreNuGetUpdateSourceFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCoreNuGetUpdateSourceFixture();
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
                var fixture = new DotNetCoreNuGetUpdateSourceFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetUpdateSourceFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget update source \"name\"", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetUpdateSourceFixture();
                fixture.Settings = new DotNetCoreNuGetSourceSettings
                {
                    Source = "source",
                    UserName = "username",
                    Password = "password",
                    StorePasswordInClearText = true,
                    ValidAuthenticationTypes = "basic,negotiate"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget update source \"name\" --source \"source\" --username \"username\" --password \"password\" --store-password-in-clear-text --valid-authentication-types \"basic,negotiate\"", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetCoreNuGetUpdateSourceFixture();
                fixture.Settings = new DotNetCoreNuGetSourceSettings { DiagnosticOutput = true };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics nuget update source \"name\"", result.Args);
            }
        }
    }
}