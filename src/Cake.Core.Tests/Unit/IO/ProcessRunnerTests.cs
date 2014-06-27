using System;
using System.Diagnostics;
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
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new ProcessRunner(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("log", ((ArgumentNullException)result).ParamName);
            }
        }
        
        public sealed class TheStartMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Start_Info_Is_Null()
            {
                // Given
                var log = Substitute.For<ICakeLog>();
                var runner = new ProcessRunner(log);

                // When
                var result = Record.Exception(() => runner.Start(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("info", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Filename_Is_Null()
            {
                // Given
                var log = Substitute.For<ICakeLog>();
                var runner = new ProcessRunner(log);
                var info = new ProcessStartInfo();

                // When
                var result = Record.Exception(() => runner.Start(info));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Cannot start process since no filename has been set.", result.Message);
            }
        }
    }
}
