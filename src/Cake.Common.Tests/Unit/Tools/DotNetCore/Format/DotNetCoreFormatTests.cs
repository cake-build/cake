// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNetCore.Format;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Format;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.Format
{
    public sealed class DotNetCoreFormatTests
    {
        public sealed class TheFormatMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreFormatterFixture();
                fixture.Root = "./src/project";
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCoreFormatterFixture();
                fixture.Root = "./src/*";
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
                var fixture = new DotNetCoreFormatterFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET Core CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Settings()
            {
                // Given
                var fixture = new DotNetCoreFormatterFixture();
                fixture.Settings.Diagnostics = new[] { "ID0001" };
                fixture.Settings.Severity = DotNetCoreSeverity.Info;
                fixture.Settings.NoRestore = true;
                fixture.Settings.VerifyNoChanges = true;
                fixture.Settings.Include = new[] { "Program.cs", "Utility\\Logging.cs" };
                fixture.Settings.Exclude = new[] { "Program.cs", "Utility\\Logging.cs" };
                fixture.Settings.IncludeGenerated = true;
                fixture.Settings.BinaryLog = "./src/log";
                fixture.Settings.Report = "./src/report.json";
                fixture.Settings.Verbosity = DotNetCoreVerbosity.Minimal;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("format" +
                    " --diagnostics ID0001" +
                    " --severity info" +
                    " --no-restore" +
                    " --verify-no-changes" +
                    " --include Program.cs Utility\\Logging.cs" +
                    " --exclude Program.cs Utility\\Logging.cs" +
                    " --include-generated" +
                    " --binarylog \"/Working/src/log\"" +
                    " --report \"/Working/src/report.json\"" +
                    " --verbosity minimal", result.Args);
            }

            [Fact]
            public void Should_Add_Style_Settings()
            {
                // Given
                var fixture = new DotNetCoreFormatterFixture();
                fixture.Subcommand = "style";
                fixture.Settings.Diagnostics = new[] { "ID0001" };
                fixture.Settings.Severity = DotNetCoreSeverity.Info;
                fixture.Settings.NoRestore = true;
                fixture.Settings.VerifyNoChanges = true;
                fixture.Settings.Include = new[] { "Program.cs", "Utility\\Logging.cs" };
                fixture.Settings.Exclude = new[] { "Program.cs", "Utility\\Logging.cs" };
                fixture.Settings.IncludeGenerated = true;
                fixture.Settings.BinaryLog = "./src/log";
                fixture.Settings.Report = "./src/report.json";
                fixture.Settings.Verbosity = DotNetCoreVerbosity.Minimal;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("format" +
                    " style" +
                    " --diagnostics ID0001" +
                    " --severity info" +
                    " --no-restore" +
                    " --verify-no-changes" +
                    " --include Program.cs Utility\\Logging.cs" +
                    " --exclude Program.cs Utility\\Logging.cs" +
                    " --include-generated" +
                    " --binarylog \"/Working/src/log\"" +
                    " --report \"/Working/src/report.json\"" +
                    " --verbosity minimal", result.Args);
            }

            [Fact]
            public void Should_Add_Whithspace_Settings()
            {
                // Given
                var fixture = new DotNetCoreFormatterFixture();
                fixture.Root = "./src";
                fixture.Subcommand = "whithspace";
                fixture.Settings.Diagnostics = new[] { "ID0001" };
                fixture.Settings.Severity = DotNetCoreSeverity.Info;
                fixture.Settings.NoRestore = true;
                fixture.Settings.VerifyNoChanges = true;
                fixture.Settings.Include = new[] { "Program.cs", "Utility\\Logging.cs" };
                fixture.Settings.Exclude = new[] { "Program.cs", "Utility\\Logging.cs" };
                fixture.Settings.IncludeGenerated = true;
                fixture.Settings.BinaryLog = "./src/log";
                fixture.Settings.Report = "./src/report.json";
                fixture.Settings.Verbosity = DotNetCoreVerbosity.Minimal;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("format" +
                    " whithspace" +
                    " \"./src\"" +
                    " --diagnostics ID0001" +
                    " --severity info" +
                    " --no-restore" +
                    " --verify-no-changes" +
                    " --include Program.cs Utility\\Logging.cs" +
                    " --exclude Program.cs Utility\\Logging.cs" +
                    " --include-generated" +
                    " --binarylog \"/Working/src/log\"" +
                    " --report \"/Working/src/report.json\"" +
                    " --verbosity minimal", result.Args);
            }
        }
    }
}
