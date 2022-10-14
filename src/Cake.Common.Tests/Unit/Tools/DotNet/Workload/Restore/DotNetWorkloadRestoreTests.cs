// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Workload.Restore;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Workload.Restore;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Workload.Restore
{
    public sealed class DotNetWorkloadRestoreTests
    {
        public sealed class TheBuildMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetWorkloadRestorerFixture();
                fixture.Project = "./src/*";
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
                var fixture = new DotNetWorkloadRestorerFixture();
                fixture.Project = "./src/*";
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
                var fixture = new DotNetWorkloadRestorerFixture();
                fixture.Project = "./src/*";
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
                var fixture = new DotNetWorkloadRestorerFixture();
                fixture.Settings = new DotNetWorkloadRestoreSettings();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "project");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetWorkloadRestorerFixture();
                fixture.Project = "./src/*";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("workload restore \"./src/*\"", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetWorkloadRestorerFixture();
                fixture.Project = "./src/*";
                fixture.Settings.ConfigFile = "./nuget.config";
                fixture.Settings.DisableParallel = true;
                fixture.Settings.IgnoreFailedSources = true;
                fixture.Settings.IncludePreviews = true;
                fixture.Settings.Interactive = true;
                fixture.Settings.NoCache = true;
                fixture.Settings.SkipManifestUpdate = true;
                fixture.Settings.Source.Add("http://www.nuget.org/api/v2/package");
                fixture.Settings.Source.Add("http://www.symbolserver.org/");
                fixture.Settings.TempDir = "./src/project";
                fixture.Settings.Verbosity = DotNetVerbosity.Diagnostic;

                // When
                var result = fixture.Run();

                // Then
                var expected = "workload restore \"./src/*\" --configfile \"/Working/nuget.config\" --disable-parallel --ignore-failed-sources --include-previews --interactive --no-cache --skip-manifest-update --source \"http://www.nuget.org/api/v2/package\" --source \"http://www.symbolserver.org/\" --temp-dir \"/Working/src/project\" --verbosity diagnostic";
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./src/*", "workload restore \"./src/*\"")]
            [InlineData("./src/cake build/", "workload restore \"./src/cake build/\"")]
            [InlineData("./src/cake build/cake cli", "workload restore \"./src/cake build/cake cli\"")]
            public void Should_Quote_Project_Path(string text, string expected)
            {
                // Given
                var fixture = new DotNetWorkloadRestorerFixture();
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
                var fixture = new DotNetWorkloadRestorerFixture();
                fixture.Project = "./src/*";
                fixture.Settings.DiagnosticOutput = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics workload restore \"./src/*\"", result.Args);
            }
        }
    }
}
