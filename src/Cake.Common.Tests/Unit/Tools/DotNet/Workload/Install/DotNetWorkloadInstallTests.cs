// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Workload.Install;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Workload.Install
{
    public sealed class DotNetWorkloadInstallTests
    {
        public sealed class TheWorkloadInstallMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetWorkloadInstallerFixture();
                fixture.WorkloadIds = new string[] { "maui" };
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
                var fixture = new DotNetWorkloadInstallerFixture();
                fixture.WorkloadIds = new string[] { "maui" };
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_WorkloadIds_Argument()
            {
                // Given
                var fixture = new DotNetWorkloadInstallerFixture();
                fixture.WorkloadIds = new string[] { "maui-android", "maui-ios" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("workload install maui-android maui-ios", result.Args);
            }

            [Fact]
            public void Should_Throw_If_WorkloadIds_Is_Empty()
            {
                // Given
                var fixture = new DotNetWorkloadInstallerFixture();
                fixture.WorkloadIds = new string[] { };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "workloadIds");
            }

            [Fact]
            public void Should_Throw_If_WorkloadIds_Is_Null()
            {
                // Given
                var fixture = new DotNetWorkloadInstallerFixture();
                fixture.WorkloadIds = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "workloadIds");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetWorkloadInstallerFixture();
                fixture.WorkloadIds = new string[] { "maui" };
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetWorkloadInstallerFixture();
                fixture.WorkloadIds = new string[] { "maui" };
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
                fixture.Settings.Verbosity = Common.Tools.DotNet.DotNetVerbosity.Diagnostic;

                // When
                var result = fixture.Run();

                // Then
                var expected = "workload install maui --configfile \"/Working/nuget.config\" --disable-parallel --ignore-failed-sources --include-previews --interactive --no-cache --skip-manifest-update";
                expected += " --source \"http://www.nuget.org/api/v2/package\" --source \"http://www.symbolserver.org/\" --temp-dir \"/Working/src/project\" --verbosity diagnostic";
                Assert.Equal(expected, result.Args);
            }
        }
    }
}
