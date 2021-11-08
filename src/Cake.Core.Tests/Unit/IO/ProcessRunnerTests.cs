// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tests.Fixtures;
using Cake.Core.Tooling;
using Cake.Testing.Xunit;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class ProcessRunnerTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_FileSystem_Is_Null()
            {
                // Given
                var fixture = new ProcessRunnerFixture();
                fixture.FileSystem = null;

                // Given, When
                var result = Record.Exception(() => fixture.CreateProcessRunner());

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new ProcessRunnerFixture();
                fixture.Environment = null;

                // Given, When
                var result = Record.Exception(() => fixture.CreateProcessRunner());

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given;
                var fixture = new ProcessRunnerFixture();
                fixture.Log = null;

                // Given, When
                var result = Record.Exception(() => fixture.CreateProcessRunner());

                // Then
                AssertEx.IsArgumentNullException(result, "log");
            }

            [Fact]
            public void Should_Throw_If_Tools_Is_Null()
            {
                // Given
                var fixture = new ProcessRunnerFixture();
                fixture.Tools = null;

                // Given, When
                var result = Record.Exception(() => fixture.CreateProcessRunner());

                // Then
                AssertEx.IsArgumentNullException(result, "tools");
            }
        }

        public sealed class TheStartMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Settings_Are_Null()
            {
                // Given
                var fixture = new ProcessRunnerFixture();
                fixture.ProcessSettings = null;

                // When
                var result = Record.Exception(() => fixture.Start());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Filename_Is_Null()
            {
                // Given
                var fixture = new ProcessRunnerFixture();
                fixture.ProcessFilePath = null;

                // When
                var result = Record.Exception(() => fixture.Start());

                // Then
                AssertEx.IsArgumentNullException(result, "filePath");
            }
        }

        public sealed class TheGetProcessStartInfoMethod
        {
            [Fact]
            public void Should_Not_Quote_FileName_On_Unix()
            {
                // Given
                var fixture = new ProcessRunnerFixture(windows: false);

                // When
                var result = fixture.GetProcessStartInfo();

                // Then
                Assert.Equal("/Program Files/Cake.exe", result.FileName);
            }

            [Fact]
            public void Should_Quote_FileName_On_Windows()
            {
                // Given
                var fixture = new ProcessRunnerFixture(windows: true);

                // When
                var result = fixture.GetProcessStartInfo();

                // Then
                Assert.Equal("\"/Program Files/Cake.exe\"", result.FileName);
            }

            [Fact]
            public void Should_Not_Log_If_Setting_Silent()
            {
                // Given
                var fixture = new ProcessRunnerFixture(windows: true);
                fixture.ProcessSettings.Silent = true;

                // When
                var result = fixture.GetProcessStartInfo();

                // Then
                fixture.Log
                    .Received(0);
            }

            [Fact]
            public void Should_Not_Log_Secret_Arguments()
            {
                // Given
                var fixture = new ProcessRunnerFixture(windows: true);
                fixture.GivenSecretArgument();

                // When
                var result = fixture.GetProcessStartInfo();

                // Then
                fixture.Log
                    .Received(1)
                    .Verbose(Verbosity.Diagnostic, "Executing: {0}", "\"/Program Files/Cake.exe\" [REDACTED]");
            }

            public void Should_Coerse_Mono_On_Unix_And_CoreClr()
            {
                // Given
                var fixture = new ProcessRunnerFixture(windows: false);
                fixture.GivenIsCoreClr();

                // When
                var result = fixture.GetProcessStartInfo();

                // Then
                Assert.Equal("/Program Files/mono.exe", result.FileName);
                Assert.Equal("\"/Program Files/Cake.exe\"", result.Arguments);
                fixture.Log
                    .Received(1)
                    .Write(Verbosity.Diagnostic, LogLevel.Verbose, "{0} is a .NET Framework executable, will try execute using Mono.", "/Program Files/Cake.exe");
            }

            public void Should_Not_Coerse_Mono_On_Windows_And_CoreClr()
            {
                // Given
                var fixture = new ProcessRunnerFixture(windows: true);
                fixture.GivenIsCoreClr();

                // When
                var result = fixture.GetProcessStartInfo();

                // Then
                Assert.Equal("\"/Program Files/Cake.exe\"", result.FileName);
            }

            public void Should_Not_Coerse_Mono_On_Unix_And_CoreClr_With_Config_NoMonoCoersion()
            {
                // Given
                var fixture = new ProcessRunnerFixture(windows: false);
                fixture.GivenIsCoreClr();
                fixture.GivenConfigNoMonoCoersion();

                // When
                var result = fixture.GetProcessStartInfo();

                // Then
                Assert.Equal("/Program Files/Cake.exe", result.FileName);
            }

            public void Should_Not_Coerse_Mono_On_Unix_And_CoreClr_If_Mono_Not_Resolved()
            {
                // Given
                var fixture = new ProcessRunnerFixture(windows: false);
                fixture.GivenIsCoreClr();
                fixture.GivenMonoNotResolved();

                // When
                var result = fixture.GetProcessStartInfo();

                // Then
                Assert.Equal("/Program Files/Cake.exe", result.FileName);
                fixture.Log
                    .Received(1)
                    .Write(Verbosity.Diagnostic, LogLevel.Verbose, "{0} is a .NET Framework executable, you might need to install Mono for it to execute successfully.", "/Program Files/Cake.exe");
            }
        }
    }
}