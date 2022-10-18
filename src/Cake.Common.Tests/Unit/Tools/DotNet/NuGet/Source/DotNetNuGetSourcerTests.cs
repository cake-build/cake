// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.NuGet.Source;
using Cake.Common.Tools.DotNet.NuGet.Source;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.NuGet.Source
{
    public sealed class DotNetNuGetSourcerTests
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
                var fixture = new DotNetNuGetAddSourceFixture();
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
                var fixture = new DotNetNuGetAddSourceFixture();
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
                var fixture = new DotNetNuGetAddSourceFixture();
                fixture.Settings = new DotNetNuGetSourceSettings { Source = source };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "Source", "Value cannot be null or whitespace.");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetNuGetAddSourceFixture();
                fixture.Settings = new DotNetNuGetSourceSettings { Source = "source" };
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
                var fixture = new DotNetNuGetAddSourceFixture();
                fixture.Settings = new DotNetNuGetSourceSettings { Source = "source" };
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
                var fixture = new DotNetNuGetAddSourceFixture();
                fixture.Settings = new DotNetNuGetSourceSettings { Source = "source" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget add source \"source\" --name \"name\"", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetNuGetAddSourceFixture();
                fixture.Settings = new DotNetNuGetSourceSettings
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
                var fixture = new DotNetNuGetAddSourceFixture();
                fixture.Settings = new DotNetNuGetSourceSettings { DiagnosticOutput = true, Source = "source" };

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
                var fixture = new DotNetNuGetDisableSourceFixture();
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
                var fixture = new DotNetNuGetDisableSourceFixture();
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
                var fixture = new DotNetNuGetDisableSourceFixture();
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
                var fixture = new DotNetNuGetDisableSourceFixture();
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
                var fixture = new DotNetNuGetDisableSourceFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget disable source \"name\"", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetNuGetDisableSourceFixture();
                fixture.Settings = new DotNetNuGetSourceSettings { DiagnosticOutput = true };

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
                var fixture = new DotNetNuGetEnableSourceFixture();
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
                var fixture = new DotNetNuGetEnableSourceFixture();
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
                var fixture = new DotNetNuGetEnableSourceFixture();
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
                var fixture = new DotNetNuGetEnableSourceFixture();
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
                var fixture = new DotNetNuGetEnableSourceFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget enable source \"name\"", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetNuGetEnableSourceFixture();
                fixture.Settings = new DotNetNuGetSourceSettings { DiagnosticOutput = true };

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
                var fixture = new DotNetNuGetHasSourceFixture();
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
                var fixture = new DotNetNuGetHasSourceFixture();
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
                var fixture = new DotNetNuGetHasSourceFixture();
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
                var fixture = new DotNetNuGetHasSourceFixture();
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
                var fixture = new DotNetNuGetHasSourceFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget list source --format \"detailed\"", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetNuGetHasSourceFixture();
                fixture.Settings = new DotNetNuGetSourceSettings { DiagnosticOutput = true };

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
                var fixture = new DotNetNuGetHasSourceFixture();
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
                var fixture = new DotNetNuGetListSourceFixture();
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
                var fixture = new DotNetNuGetListSourceFixture();
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
                var fixture = new DotNetNuGetListSourceFixture();
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
                var fixture = new DotNetNuGetListSourceFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget list source", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetNuGetListSourceFixture();
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
                var fixture = new DotNetNuGetListSourceFixture();
                fixture.Settings = new DotNetNuGetSourceSettings { DiagnosticOutput = true };

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
                var fixture = new DotNetNuGetRemoveSourceFixture();
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
                var fixture = new DotNetNuGetRemoveSourceFixture();
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
                var fixture = new DotNetNuGetRemoveSourceFixture();
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
                var fixture = new DotNetNuGetRemoveSourceFixture();
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
                var fixture = new DotNetNuGetRemoveSourceFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget remove source \"name\"", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetNuGetRemoveSourceFixture();
                fixture.Settings = new DotNetNuGetSourceSettings { DiagnosticOutput = true };

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
                var fixture = new DotNetNuGetUpdateSourceFixture();
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
                var fixture = new DotNetNuGetUpdateSourceFixture();
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
                var fixture = new DotNetNuGetUpdateSourceFixture();
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
                var fixture = new DotNetNuGetUpdateSourceFixture();
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
                var fixture = new DotNetNuGetUpdateSourceFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget update source \"name\"", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetNuGetUpdateSourceFixture();
                fixture.Settings = new DotNetNuGetSourceSettings
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
                var fixture = new DotNetNuGetUpdateSourceFixture();
                fixture.Settings = new DotNetNuGetSourceSettings { DiagnosticOutput = true };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics nuget update source \"name\"", result.Args);
            }
        }
    }
}