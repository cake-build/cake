// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Workload.Update;
using Cake.Common.Tools.DotNet;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Workload.Update
{
    public sealed class DotNetWorkloadUpdateTests
    {
        public sealed class TheWorkloadUpdateMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetWorkloadUpdaterFixture();
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
                var fixture = new DotNetWorkloadUpdaterFixture();
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
                var fixture = new DotNetWorkloadUpdaterFixture();
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
                var fixture = new DotNetWorkloadUpdaterFixture();
                fixture.Settings.AdvertisingManifestsOnly = true;
                fixture.Settings.ConfigFile = "./nuget.config";
                fixture.Settings.DisableParallel = true;
                fixture.Settings.FromPreviousSdk = true;
                fixture.Settings.IgnoreFailedSources = true;
                fixture.Settings.IncludePreviews = true;
                fixture.Settings.Interactive = true;
                fixture.Settings.NoCache = true;
                fixture.Settings.Source.Add("http://www.nuget.org/api/v2/package");
                fixture.Settings.Source.Add("http://www.symbolserver.org/");
                fixture.Settings.TempDir = "./src/project";
                fixture.Settings.Verbosity = DotNetVerbosity.Diagnostic;

                // When
                var result = fixture.Run();

                // Then
                var expected = "workload update --advertising-manifests-only --configfile \"/Working/nuget.config\" --disable-parallel --from-previous-sdk --ignore-failed-sources --include-previews --interactive --no-cache --source \"http://www.nuget.org/api/v2/package\" --source \"http://www.symbolserver.org/\" --temp-dir \"/Working/src/project\" --verbosity diagnostic";
                Assert.Equal(expected, result.Args);
            }
        }
    }
}
