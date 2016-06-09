// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Tools.DNU.Restorer;
using Cake.Common.Tools.DNU.Restore;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DNU.Restore
{
    public sealed class DNURestorerTests
    {
        public sealed class TheRestoreMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_DNU_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new DNURestorerFixture();
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
                var fixture = new DNURestorerFixture();
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
                var fixture = new DNURestorerFixture();
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
                var fixture = new DNURestorerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore", result.Args);
            }

            [Fact]
            public void Should_Add_FilePath_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.FilePath = "./project.json";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore \"/Working/project.json\"", result.Args);
            }

            [Fact]
            public void Should_Add_Sources_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    Sources = new[] { "https://www.example.com/nugetfeed", "https://www.example.com/nugetfeed2" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore --source \"https://www.example.com/nugetfeed\" --source \"https://www.example.com/nugetfeed2\"", result.Args);
            }

            [Fact]
            public void Should_Add_FallbackSources_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    FallbackSources = new[] { "https://www.example.com/fallbacknugetfeed" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore --fallbacksource \"https://www.example.com/fallbacknugetfeed\"", result.Args);
            }

            [Fact]
            public void Should_Add_Proxy_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    Proxy = "exampleproxy"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore --proxy \"exampleproxy\"", result.Args);
            }

            [Fact]
            public void Should_Add_NoCache_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    NoCache = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore --no-cache", result.Args);
            }

            [Fact]
            public void Should_Add_Packages_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    Packages = "./packages"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore --packages \"/Working/packages\"", result.Args);
            }

            [Fact]
            public void Should_Add_IgnoreFailedSources_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    IgnoreFailedSources = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore --ignore-failed-sources", result.Args);
            }

            [Fact]
            public void Should_Add_Quiet_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    Quiet = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore --quiet", result.Args);
            }

            [Fact]
            public void Should_Add_Parallel_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    Parallel = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore --parallel", result.Args);
            }

            [Theory]
            [InlineData(DNULocked.Lock, "--lock")]
            [InlineData(DNULocked.Unlock, "--unlock")]
            public void Should_Add_Locked_If_Set(DNULocked locked, string arg)
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    Locked = locked
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore " + arg, result.Args);
            }

            [Fact]
            public void Should_Add_Runtime_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    Runtimes = new[] { "runtime1", "runtime2" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore --runtime \"runtime1\" --runtime \"runtime2\"", result.Args);
            }

            [Fact]
            public void Should_Add_Arguments_And_Settings_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.FilePath = "./project.json";
                fixture.Settings = new DNURestoreSettings
                {
                    Sources = new[] { "https://www.example.com/nugetfeed", "https://www.example.com/nugetfeed2" },
                    FallbackSources = new[] { "https://www.example.com/fallbacknugetfeed" },
                    Proxy = "exampleproxy",
                    NoCache = true,
                    Packages = "./packages",
                    IgnoreFailedSources = true,
                    Quiet = true,
                    Parallel = true,
                    Locked = DNULocked.Lock,
                    Runtimes = new[] { "runtime1", "runtime2" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore \"/Working/project.json\"" +
                             " --source \"https://www.example.com/nugetfeed\"" +
                             " --source \"https://www.example.com/nugetfeed2\"" +
                             " --fallbacksource \"https://www.example.com/fallbacknugetfeed\"" +
                             " --proxy \"exampleproxy\"" +
                             " --no-cache" +
                             " --packages \"/Working/packages\"" +
                             " --ignore-failed-sources" +
                             " --quiet" +
                             " --parallel" +
                             " --lock" +
                             " --runtime \"runtime1\" --runtime \"runtime2\"",
                    result.Args);
            }
        }
    }
}
