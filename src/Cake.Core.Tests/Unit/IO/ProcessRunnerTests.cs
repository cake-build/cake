using System;
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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("environment", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();

                // Given, When
                var result = Record.Exception(() => new ProcessRunner(environment, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("log", ((ArgumentNullException)result).ParamName);
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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("settings", ((ArgumentNullException)result).ParamName);
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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("filePath", ((ArgumentNullException)result).ParamName);
            }
        }
    }
}
