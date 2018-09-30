using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Cake.DotNetTool.Module.Tests
{
    /// <summary>
    /// DotNetToolPackageInstaller unit tests
    /// </summary>
    public sealed class DotNetToolPackageInstallerTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new DotNetToolPackageInstallerFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("environment", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Process_Runner_Is_Null()
            {
                // Given
                var fixture = new DotNetToolPackageInstallerFixture();
                fixture.ProcessRunner = null;

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("processRunner", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Content_Resolver_Is_Null()
            {
                // Given
                var fixture = new DotNetToolPackageInstallerFixture();
                fixture.ContentResolver = null;

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("contentResolver", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fixture = new DotNetToolPackageInstallerFixture();
                fixture.Log = null;

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("log", ((ArgumentNullException)result).ParamName);
            }
        }

        public sealed class TheCanInstallMethod
        {
            public void Should_Throw_If_URI_Is_Null()
            {
                // Given
                var fixture = new DotNetToolPackageInstallerFixture();
                fixture.Package = null;

                // When
                var result = Record.Exception(() => fixture.CanInstall());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("package", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Be_Able_To_Install_If_Scheme_Is_Correct()
            {
                // Given
                var fixture = new DotNetToolPackageInstallerFixture();
                fixture.Package = new PackageReference("dotnet:?package=Octopus.DotNet.Cli");

                // When
                var result = fixture.CanInstall();

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Not_Be_Able_To_Install_If_Scheme_Is_Incorrect()
            {
                // Given
                var fixture = new DotNetToolPackageInstallerFixture();
                fixture.Package = new PackageReference("homebrew:?package=windirstat");

                // When
                var result = fixture.CanInstall();

                // Then
                Assert.False(result);
            }
        }

        public sealed class TheInstallMethod
        {
            [Fact]
            public void Should_Throw_If_Uri_Is_Null()
            {
                // Given
                var fixture = new DotNetToolPackageInstallerFixture();
                fixture.Package = null;

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("package", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Install_Path_Is_Null()
            {
                // Given
                var fixture = new DotNetToolPackageInstallerFixture();
                fixture.InstallPath = null;

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException)result).ParamName);
            }
        }
    }
}
