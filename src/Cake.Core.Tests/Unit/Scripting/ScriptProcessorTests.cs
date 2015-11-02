using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting
{
    public sealed class ScriptProcessorTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.FileSystem = null;

                // When
                var result = Record.Exception(() => fixture.CreateProcessor());

                // Then
                Assert.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.CreateProcessor());

                // Then
                Assert.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.Log = null;

                // When
                var result = Record.Exception(() => fixture.CreateProcessor());

                // Then
                Assert.IsArgumentNullException(result, "log");
            }

            [Fact]
            public void Should_Throw_If_NuGet_Package_Installer_Is_Null()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.Installer = null;

                // When
                var result = Record.Exception(() => fixture.CreateProcessor());

                // Then
                Assert.IsArgumentNullException(result, "installer");
            }
        }

        public sealed class TheInstallAddinsMethod
        {
            [Fact]
            public void Should_Throw_If_Addins_Could_Not_Be_Found()
            {
                // Given
                var fixture = new ScriptProcessorFixture();

                // When
                var result = Record.Exception(() => fixture.InstallAddins());

                // Then
                Assert.IsCakeException(result, "Failed to install addin 'Addin'.");
            }

            [Fact]
            public void Should_Install_Addins_Referenced_By_Scripts()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.GivenAddinFilesAreDownloaded();

                // When
                fixture.InstallAddins();

                // Then
                fixture.Installer.Received(1).InstallPackage(
                    Arg.Is<NuGetPackage>(package =>
                        package.PackageId == "Addin" &&
                        package.Source == "http://example.com"), 
                    Arg.Any<DirectoryPath>());
            }

            [Fact]
            public void Should_Not_Install_Addins_Present_On_Disk()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.GivenAddinFilesAlreadyHaveBeenDownloaded();

                // When
                fixture.InstallAddins();

                // Then
                fixture.Installer.Received(0)
                    .InstallPackage(Arg.Any<NuGetPackage>(), Arg.Any<DirectoryPath>());
            }
        }

        public sealed class TheInstallToolsMethod
        {
            [Fact]
            public void Should_Throw_If_Tools_Could_Not_Be_Found()
            {
                // Given
                var fixture = new ScriptProcessorFixture();

                // When
                var result = Record.Exception(() => fixture.InstallTools());

                // Then
                Assert.IsCakeException(result, "Failed to install tool 'Tool'.");
            }

            [Fact]
            public void Should_Install_Tools_Referenced_By_Scripts()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.GivenToolFilesAreDownloaded();

                // When
                fixture.InstallTools();

                // Then
                fixture.Installer.Received(1).InstallPackage(
                    Arg.Is<NuGetPackage>(package =>
                        package.PackageId == "Tool" &&
                        package.Source == "http://example.com"),
                    Arg.Any<DirectoryPath>());
            }

            [Fact]
            public void Should_Not_Install_Addins_Present_On_Disc()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.GivenToolFilesAlreadyHaveBeenDownloaded();

                // When
                fixture.InstallTools();

                // Then
                fixture.Installer.Received(0)
                    .InstallPackage(Arg.Any<NuGetPackage>(), Arg.Any<DirectoryPath>());
            }
        }
    }
}