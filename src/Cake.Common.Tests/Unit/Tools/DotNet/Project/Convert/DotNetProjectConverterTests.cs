// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Project.Convert;
using Cake.Common.Tools.DotNet;
using Cake.Testing;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Project.Convert;

public sealed class DotNetProjectConverterTests
{
    public sealed class TheConvertMethod
    {
        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started()
        {
            // Given
            var fixture = new DotNetProjectConverterFixture();
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
            var fixture = new DotNetProjectConverterFixture();
            fixture.GivenProcessExitsWithCode(1);

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
        }

        [Fact]
        public void Should_Throw_If_File_Is_Null()
        {
            // Given
            var fixture = new DotNetProjectConverterFixture();
            fixture.File = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "file");
        }

        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given
            var fixture = new DotNetProjectConverterFixture();
            fixture.Settings = null;
            fixture.GivenDefaultToolDoNotExist();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Add_File_Argument()
        {
            // Given
            var fixture = new DotNetProjectConverterFixture();
            fixture.File = "cake.cs";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("project convert \"/Working/cake.cs\"", result.Args);
        }

        [Fact]
        public void Should_Add_Additional_Arguments()
        {
            // Given
            var fixture = new DotNetProjectConverterFixture();
            fixture.File = "cake.cs";
            fixture.Settings.DeleteSource = true;
            fixture.Settings.DryRun = true;
            fixture.Settings.Force = true;
            fixture.Settings.Interactive = true;
            fixture.Settings.Output = "./cake";
            fixture.Settings.Verbosity = DotNetVerbosity.Diagnostic;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("project convert \"/Working/cake.cs\" --delete-source --dry-run --force --interactive --output \"/Working/cake\" --verbosity diagnostic", result.Args);
        }
    }
}
