// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.NuGet.Why;
using Cake.Common.Tools.DotNet;
using Cake.Testing;

namespace Cake.Common.Tests.Unit.Tools.DotNet.NuGet.Why;

public sealed class DotNetNuGetWhyerTests
{
    public sealed class TheWhyMethod
    {
        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started()
        {
            // Given
            var fixture = new DotNetNuGetWhyerFixture();
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
            var fixture = new DotNetNuGetWhyerFixture();
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
            var fixture = new DotNetNuGetWhyerFixture();
            fixture.Settings = null;
            fixture.GivenDefaultToolDoNotExist();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Throw_If_Package_Is_Null()
        {
            // Given
            var fixture = new DotNetNuGetWhyerFixture();
            fixture.Package = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentException(result, "package", "Package cannot be null or empty.");
        }

        [Fact]
        public void Should_Throw_If_Package_Is_Empty()
        {
            // Given
            var fixture = new DotNetNuGetWhyerFixture();
            fixture.Package = string.Empty;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentException(result, "package", "Package cannot be null or empty.");
        }

        [Fact]
        public void Should_Add_Package_Argument()
        {
            // Given
            var fixture = new DotNetNuGetWhyerFixture();
            fixture.Project = null;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("nuget why Newtonsoft.Json", result.Args);
        }

        [Fact]
        public void Should_Add_Project_Argument()
        {
            // Given
            var fixture = new DotNetNuGetWhyerFixture();
            fixture.Project = "./src/App/App.csproj";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("nuget why \"/Working/src/App/App.csproj\" Newtonsoft.Json", result.Args);
        }

        [Fact]
        public void Should_Add_Project_From_Settings()
        {
            // Given
            var fixture = new DotNetNuGetWhyerFixture();
            fixture.Project = null;
            fixture.Settings.Project = "./src/App/App.csproj";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("nuget why \"/Working/src/App/App.csproj\" Newtonsoft.Json", result.Args);
        }

        [Fact]
        public void Should_Add_Frameworks()
        {
            // Given
            var fixture = new DotNetNuGetWhyerFixture();
            fixture.Project = null;
            fixture.Settings.Frameworks.Add("net10.0");
            fixture.Settings.Frameworks.Add("net11.0");

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("nuget why Newtonsoft.Json --framework net10.0 --framework net11.0", result.Args);
        }

        [Fact]
        public void Should_Add_Additional_Arguments()
        {
            // Given
            var fixture = new DotNetNuGetWhyerFixture();
            fixture.Project = "./src/App/App.csproj";
            fixture.Settings.Frameworks.Add("net10.0");
            fixture.Settings.Verbosity = DotNetVerbosity.Diagnostic;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("nuget why \"/Working/src/App/App.csproj\" Newtonsoft.Json --framework net10.0 --verbosity diagnostic", result.Args);
        }
    }
}
