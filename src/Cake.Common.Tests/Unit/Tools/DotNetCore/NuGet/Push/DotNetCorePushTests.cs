// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNetCore.NuGet.Delete;
using Cake.Common.Tests.Fixtures.Tools.DotNetCore.NuGet.Push;
using Cake.Common.Tools.DotNetCore.NuGet.Push;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.NuGet.Push
{
    public sealed class DotNetCorePushTests
    {
        public sealed class ThePushMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCorePusherFixture();
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("       ")]
            public void Should_Throw_If_PackageName_Is_Empty(string packageName)
            {
                // Given
                var fixture = new DotNetCorePusherFixture();
                fixture.PackageName = packageName;
                fixture.Settings = new DotNetCoreNuGetPushSettings();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packageName");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCorePusherFixture();
                fixture.PackageName = "foo.nupkg";
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET Core CLI: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DotNetCorePusherFixture();
                fixture.PackageName = "foo.nupkg";
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET Core CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                const string packageName = "foo.nupkg";

                var fixture = new DotNetCorePusherFixture();
                fixture.PackageName = packageName;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(string.Format("nuget push {0}", packageName), result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                const string packageName = "foo.nupkg";
                const int timeout = 60;
                const string source = "http://www.nuget.org/api/v2/package";
                const string apiKey = "key1234";
                const string symbolSource = "http://www.symbolserver.org/";
                const string symbolApiKey = "key5678";

                var fixture = new DotNetCorePusherFixture();
                fixture.Settings.Source = source;
                fixture.Settings.ApiKey = apiKey;
                fixture.Settings.SymbolSource = symbolSource;
                fixture.Settings.SymbolApiKey = symbolApiKey;
                fixture.Settings.Timeout = timeout;
                fixture.Settings.DisableBuffering = true;
                fixture.Settings.IgnoreSymbols = true;
                fixture.Settings.ForceEnglishOutput = true;
                fixture.PackageName = packageName;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(string.Format("nuget push {0} --source {1} --api-key {2} --symbol-source {3} --symbol-api-key {4} --timeout {5} --disable-buffering --no-symbols --force-english-output", packageName, source, apiKey, symbolSource, symbolApiKey, timeout), result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                const string packageName = "foo.nupkg";

                var fixture = new DotNetCorePusherFixture();
                fixture.Settings.DiagnosticOutput = true;
                fixture.PackageName = packageName;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(string.Format("--diagnostics nuget push {0}", packageName), result.Args);
            }
        }
    }
}