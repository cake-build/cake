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
                fixture.Settings = new DNURestoreSettings() { Version = "default" };
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
                fixture.Settings = new DNURestoreSettings() { Version = "default" };
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
                fixture.Settings = new DNURestoreSettings() { Version = "default" };
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
                fixture.Settings = new DNURestoreSettings() { Version = "default" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("exec default dnu restore", result.Args);
            }

            [Fact]
            public void Should_Add_FilePath_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings() { Version = "default" };
                fixture.FilePath = "./project.json";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("exec default dnu restore \"/Working/project.json\"", result.Args);
            }

            [Fact]
            public void Should_Add_Sources_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    Version = "default",
                    Sources = new[] { "https://www.example.com/nugetfeed", "https://www.example.com/nugetfeed2" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("exec default dnu restore --source \"https://www.example.com/nugetfeed;https://www.example.com/nugetfeed2\"", result.Args);
            }

            [Fact]
            public void Should_Add_FallbackSources_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    Version = "default",
                    FallbackSources = new[] { "https://www.example.com/fallbacknugetfeed" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("exec default dnu restore --fallbacksource \"https://www.example.com/fallbacknugetfeed\"", result.Args);
            }

            [Fact]
            public void Should_Add_Proxy_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    Version = "default",
                    Proxy = "exampleproxy"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("exec default dnu restore --proxy \"exampleproxy\"", result.Args);
            }

            [Fact]
            public void Should_Add_NoCache_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    Version = "default",
                    NoCache = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("exec default dnu restore --no-cache", result.Args);
            }

            [Fact]
            public void Should_Add_Packages_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    Version = "default",
                    Packages = "./packages"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("exec default dnu restore --packages \"/Working/packages\"", result.Args);
            }

            [Fact]
            public void Should_Add_IgnoreFailedSources_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    Version = "default",
                    IgnoreFailedSources = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("exec default dnu restore --ignore-failed-sources", result.Args);
            }

            [Fact]
            public void Should_Add_Quiet_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    Version = "default",
                    Quiet = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("exec default dnu restore --quiet", result.Args);
            }

            [Fact]
            public void Should_Add_Parallel_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    Version = "default",
                    Parallel = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("exec default dnu restore --parallel", result.Args);
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
                    Version = "default",
                    Locked = locked
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("exec default dnu restore " + arg, result.Args);
            }

            [Fact]
            public void Should_Add_Runtime_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.Settings = new DNURestoreSettings
                {
                    Version = "default",
                    Runtimes = new[] { "runtime1", "runtime2" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("exec default dnu restore --runtime \"runtime1;runtime2\"", result.Args);
            }

            [Fact]
            public void Should_Add_Arguments_And_Settings_If_Set()
            {
                // Given
                var fixture = new DNURestorerFixture();
                fixture.FilePath = "./project.json";
                fixture.Settings = new DNURestoreSettings
                {
                    Version = "default",
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
                Assert.Equal("exec default dnu restore \"/Working/project.json\"" +
                             " --source \"https://www.example.com/nugetfeed;https://www.example.com/nugetfeed2\"" +
                             " --fallbacksource \"https://www.example.com/fallbacknugetfeed\"" +
                             " --proxy \"exampleproxy\"" +
                             " --no-cache" +
                             " --packages \"/Working/packages\"" +
                             " --ignore-failed-sources" +
                             " --quiet" +
                             " --parallel" +
                             " --lock" +
                             " --runtime \"runtime1;runtime2\"",
                    result.Args);
            }
        }
    }
}