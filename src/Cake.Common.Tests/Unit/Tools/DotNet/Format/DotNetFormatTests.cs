// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Format;
using Cake.Common.Tools.DotNet.Format;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Format
{
    public sealed class DotNetFormatTests
    {
        public sealed class TheFormatMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetFormatterFixture();
                fixture.Root = "./src/project";
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Root_Is_Null()
            {
                // Given
                var fixture = new DotNetFormatterFixture();
                fixture.Settings = new DotNetFormatSettings();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "root");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetFormatterFixture();
                fixture.Root = "./src/project";
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
                var fixture = new DotNetFormatterFixture();
                fixture.Root = "./src/project";
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
                var fixture = new DotNetFormatterFixture();
                fixture.Root = "./src/project";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("format \"./src/project\"", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetFormatterFixture();
                fixture.Settings.Diagnostics.Add("CS123");
                fixture.Settings.Diagnostics.Add("CA555");
                fixture.Settings.Severity = DotNetFormatSeverity.Warning;
                fixture.Settings.NoRestore = true;
                fixture.Settings.VerifyNoChanges = true;
                fixture.Settings.Include.Add("./src/");
                fixture.Settings.Include.Add("./tests/");
                fixture.Settings.Exclude.Add("./src/submodule-a/");
                fixture.Settings.IncludeGenerated = true;
                fixture.Settings.Verbosity = Common.Tools.DotNet.DotNetVerbosity.Diagnostic;
                fixture.Settings.BinaryLog = "./temp/b.log";
                fixture.Settings.Report = "./temp/report.json";
                fixture.Root = "./src/project";

                // When
                var result = fixture.Run();

                // Then
                var expected = "format \"./src/project\" --diagnostics CS123 CA555 --severity warn --no-restore --verify-no-changes --include ./src/ ./tests/ --exclude ./src/submodule-a/ --include-generated";
                expected += " --binarylog \"/Working/temp/b.log\" --report \"/Working/temp/report.json\" --verbosity diagnostic";
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./src/project", "format \"./src/project\"")]
            [InlineData("./src/cake build/", "format \"./src/cake build/\"")]
            [InlineData("./src/cake build/cake cli", "format \"./src/cake build/cake cli\"")]
            public void Should_Quote_Root_Path(string text, string expected)
            {
                // Given
                var fixture = new DotNetFormatterFixture();
                fixture.Root = text;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Subcommand()
            {
                // Given
                var fixture = new DotNetFormatterFixture();
                fixture.Root = "./src/project";
                fixture.Subcommand = "style";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("format style \"./src/project\"", result.Args);
            }
        }
    }
}
