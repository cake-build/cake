// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class ProcessRunnerTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var log = Substitute.For<ICakeLog>();

                // Given, When
                var result = Record.Exception(() => new ProcessRunner(null, log));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();

                // Given, When
                var result = Record.Exception(() => new ProcessRunner(environment, null));

                // Then
                AssertEx.IsArgumentNullException(result, "log");
            }
        }

        public sealed class TheStartMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Settings_Are_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                var log = Substitute.For<ICakeLog>();
                var runner = new ProcessRunner(environment, log);

                // When
                var result = Record.Exception(() => runner.Start("./app.exe", null));

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Filename_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                var log = Substitute.For<ICakeLog>();
                var runner = new ProcessRunner(environment, log);
                var info = new ProcessSettings();

                // When
                var result = Record.Exception(() => runner.Start(null, info));

                // Then
                AssertEx.IsArgumentNullException(result, "filePath");
            }
        }
    }
}