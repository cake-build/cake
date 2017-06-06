// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNetCore.Restore;
using Cake.Common.Tools.DotNetCore.Restore;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.Restore
{
    public sealed class DotNetCoreRestorerTests
    {
        public sealed class TheRestoreMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreRestorerFixture();
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
                var fixture = new DotNetCoreRestorerFixture();
                fixture.Root = "./src/*";
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
                var fixture = new DotNetCoreRestorerFixture();
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
                var fixture = new DotNetCoreRestorerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore", result.Args);
            }

            [Fact]
            public void Should_Add_Path()
            {
                // Given
                var fixture = new DotNetCoreRestorerFixture();
                fixture.Root = "./src/*";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore \"./src/*\"", result.Args);
            }

            [Theory]
            [InlineData("./src/project.json", "restore \"src/project.json\"")]
            [InlineData("./test/project.json", "restore \"test/project.json\"")]
            [InlineData("./test/Unit Tests/project.json", "restore \"test/Unit Tests/project.json\"")]
            public void Should_Add_Project_Files_If_Provided(string text, string expected)
            {
                // Given
                var fixture = new DotNetCoreRestorerFixture();
                fixture.ProjectFiles.Add(text);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./src", "restore \"src\"")]
            [InlineData("./test", "restore \"test\"")]
            [InlineData("./test/Unit Tests", "restore \"test/Unit Tests\"")]
            public void Should_Add_Directory_Paths_If_Provided(string text, string expected)
            {
                // Given
                var fixture = new DotNetCoreRestorerFixture();
                fixture.DirectoryPaths.Add(text);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./src/*", "restore \"./src/*\"")]
            [InlineData("./src/cake build/", "restore \"./src/cake build/\"")]
            [InlineData("./src/cake build/cake cli", "restore \"./src/cake build/cake cli\"")]
            public void Should_Quote_Root_Path(string text, string expected)
            {
                // Given
                var fixture = new DotNetCoreRestorerFixture();
                fixture.Root = text;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Settings()
            {
                // Given
                var fixture = new DotNetCoreRestorerFixture();
                fixture.Settings.Sources = new[] { "https://www.example.com/source1", "https://www.example.com/source2" };
                fixture.Settings.FallbackSources = new[] { "https://www.example.com/fallback1", "https://www.example.com/fallback2" };
#pragma warning disable 0618
                fixture.Settings.Quiet = true;
#pragma warning restore 0618
                fixture.Settings.NoCache = true;
                fixture.Settings.DisableParallel = true;
                fixture.Settings.ForceEnglishOutput = true;
                fixture.Settings.IgnoreFailedSources = true;
                fixture.Settings.InferRuntimes = new[] { "runtime1", "runtime2" };
                fixture.Settings.ConfigFile = "./NuGet.config";
                fixture.Settings.PackagesDirectory = "./packages/";
                fixture.Settings.Verbosity = DotNetCoreRestoreVerbosity.Information;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore" +
                             " --packages \"/Working/packages\"" +
                             " --source \"https://www.example.com/source1\"" +
                             " --source \"https://www.example.com/source2\"" +
                             " --fallbacksource \"https://www.example.com/fallback1\"" +
                             " --fallbacksource \"https://www.example.com/fallback2\"" +
                             " --configfile \"/Working/NuGet.config\"" +
                             " --infer-runtimes \"runtime1\"" +
                             " --infer-runtimes \"runtime2\"" +
                             " --no-cache --disable-parallel --ignore-failed-sources --force-english-output" +
                             " --verbosity Information", result.Args);
            }

            [Fact]
            public void Quiet_Does_Not_Set_Verbosity()
            {
                // Given
                var fixture = new DotNetCoreRestorerFixture();
                fixture.Settings.Sources = new[] { "https://www.example.com/source1", "https://www.example.com/source2" };
#pragma warning disable 0618
                fixture.Settings.Quiet = true;
#pragma warning restore 0618

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore" +
                             " --source \"https://www.example.com/source1\"" +
                             " --source \"https://www.example.com/source2\"", result.Args);
            }
        }
    }
}