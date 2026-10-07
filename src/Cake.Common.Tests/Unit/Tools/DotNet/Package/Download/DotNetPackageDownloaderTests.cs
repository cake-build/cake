// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Package.Download;
using Cake.Common.Tools.DotNet;
using Cake.Testing;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Package.Download;

public sealed class DotNetPackageDownloaderTests
{
    public sealed class TheDownloadMethod
    {
        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started()
        {
            // Given
            var fixture = new DotNetPackageDownloaderFixture();
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
            var fixture = new DotNetPackageDownloaderFixture();
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
            var fixture = new DotNetPackageDownloaderFixture();
            fixture.Settings = null;
            fixture.GivenDefaultToolDoNotExist();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Throw_If_Packages_Are_Null()
        {
            // Given
            var fixture = new DotNetPackageDownloaderFixture();
            fixture.Packages = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentException(result, "packages", "Packages cannot be empty.");
        }

        [Fact]
        public void Should_Throw_If_Packages_Are_Empty()
        {
            // Given
            var fixture = new DotNetPackageDownloaderFixture();
            fixture.Packages = [];

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentException(result, "packages", "Packages cannot be empty.");
        }

        [Fact]
        public void Should_Add_Package_Argument()
        {
            // Given
            var fixture = new DotNetPackageDownloaderFixture();
            fixture.Packages = ["Newtonsoft.Json"];

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("package download \"Newtonsoft.Json\"", result.Args);
        }

        [Fact]
        public void Should_Add_Multiple_Packages()
        {
            // Given
            var fixture = new DotNetPackageDownloaderFixture();
            fixture.Packages = ["Newtonsoft.Json@13.0.3", "Cake.Core"];

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("package download \"Newtonsoft.Json@13.0.3\" \"Cake.Core\"", result.Args);
        }

        [Fact]
        public void Should_Add_Output_Argument()
        {
            // Given
            var fixture = new DotNetPackageDownloaderFixture();
            fixture.Settings.Output = "./packages";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("package download \"Newtonsoft.Json\" --output \"/Working/packages\"", result.Args);
        }

        [Fact]
        public void Should_Add_Additional_Arguments()
        {
            // Given
            var fixture = new DotNetPackageDownloaderFixture();
            fixture.Packages = ["Newtonsoft.Json@13.0.3"];
            fixture.Settings.Output = "./packages";
            fixture.Settings.Sources.Add("https://api.nuget.org/v3/index.json");
            fixture.Settings.Prerelease = true;
            fixture.Settings.ConfigFile = "./nuget.config";
            fixture.Settings.Interactive = true;
            fixture.Settings.AllowInsecureConnections = true;
            fixture.Settings.Verbosity = DotNetVerbosity.Diagnostic;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("package download \"Newtonsoft.Json@13.0.3\" --output \"/Working/packages\" --source \"https://api.nuget.org/v3/index.json\" --prerelease --configfile \"/Working/nuget.config\" --interactive --allow-insecure-connections --verbosity diagnostic", result.Args);
        }
    }
}
