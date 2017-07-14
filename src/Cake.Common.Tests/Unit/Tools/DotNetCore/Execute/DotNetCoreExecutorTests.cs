// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNetCore.Execute;
using Cake.Common.Tools.DotNetCore.Execute;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.Execute
{
    public sealed class DotNetCoreExecutorTests
    {
        public sealed class TheExecuteMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreExecutorFixture();
                fixture.AssemblyPath = "./bin/Debug/app.dll";
                fixture.Arguments = "--args";
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
                var fixture = new DotNetCoreExecutorFixture();
                fixture.AssemblyPath = "./bin/Debug/app.dll";
                fixture.Arguments = "--args";
                fixture.Settings = new DotNetCoreExecuteSettings();
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
                var fixture = new DotNetCoreExecutorFixture();
                fixture.AssemblyPath = "./bin/Debug/app.dll";
                fixture.Arguments = "--args";
                fixture.Settings = new DotNetCoreExecuteSettings();
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
                var fixture = new DotNetCoreExecutorFixture();
                fixture.AssemblyPath = "./bin/Debug/app.dll";
                fixture.Settings = new DotNetCoreExecuteSettings();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/bin/Debug/app.dll", result.Args);
            }

            [Fact]
            public void Should_Add_Famework_Version()
            {
                // Given
                var fixture = new DotNetCoreExecutorFixture();
                fixture.AssemblyPath = "./bin/Debug/app.dll";
                fixture.Settings.FrameworkVersion = "1.0.3";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--fx-version 1.0.3 /Working/bin/Debug/app.dll", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetCoreExecutorFixture();
                fixture.AssemblyPath = "./bin/Debug/app.dll";
                fixture.Settings.DiagnosticOutput = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics /Working/bin/Debug/app.dll", result.Args);
            }
        }
    }
}
