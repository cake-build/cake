// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNetCore.Build;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.BuildServer;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.BuildServer
{
    public sealed class DotNetCoreBuildServerTests
    {
        public sealed class TheShutdownMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreBuildServerFixture();
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCoreBuildServerFixture();
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
                var fixture = new DotNetCoreBuildServerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetCoreBuildServerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("build-server shutdown", result.Args);
            }

            [Theory]
            [InlineData(true, null, null, "build-server shutdown --msbuild")]
            [InlineData(null, true, null, "build-server shutdown --razor")]
            [InlineData(null, null, true, "build-server shutdown --vbcscompiler")]
            [InlineData(true, true, true, "build-server shutdown --msbuild --razor --vbcscompiler")]
            public void Should_Add_Settings_Arguments(bool? msBuild, bool? razor, bool? vbcscompiler, string expected)
            {
                // Given
                var fixture = new DotNetCoreBuildServerFixture();
                fixture.Settings.MSBuild = msBuild;
                fixture.Settings.Razor = razor;
                fixture.Settings.VBCSCompiler = vbcscompiler;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetCoreBuildServerFixture();
                fixture.Settings.DiagnosticOutput = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics build-server shutdown", result.Args);
            }
        }
    }
}