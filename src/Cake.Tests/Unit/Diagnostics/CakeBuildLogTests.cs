using Cake.Core.Diagnostics;
using Cake.Diagnostics;
using Cake.Tests.Fakes;
using Xunit;
using Xunit.Extensions;

namespace Cake.Tests.Unit.Diagnostics
{
    public sealed class CakeBuildLogTests
    {
        public sealed class TheWriteMethod
        {
            [Theory]
            [InlineData(Verbosity.Quiet, Verbosity.Minimal)]
            [InlineData(Verbosity.Minimal, Verbosity.Normal)]
            [InlineData(Verbosity.Normal, Verbosity.Verbose)]
            [InlineData(Verbosity.Verbose, Verbosity.Diagnostic)]
            public void Should_Drop_Log_Messages_Written_With_A_Lower_Verbosity_Than_Allowed(Verbosity logVerbosity, Verbosity messageVerbosity)
            {
                // Given
                var log = new FakeLog();
                var adapter = new CakeBuildLog(log)
                {
                    Verbosity = logVerbosity
                };

                // When
                adapter.Write(messageVerbosity, LogLevel.Information, "Hello World");

                // Then
                Assert.Equal(0, log.Messages.Count);
            }

            [Theory]
            [InlineData(Verbosity.Minimal, Verbosity.Quiet)]
            [InlineData(Verbosity.Normal, Verbosity.Minimal)]
            [InlineData(Verbosity.Verbose, Verbosity.Normal)]
            [InlineData(Verbosity.Diagnostic, Verbosity.Verbose)]
            public void Should_Write_Log_Messages_Written_With_A_Higher_Verbosity_Than_Allowed(Verbosity logVerbosity, Verbosity messageVerbosity)
            {
                // Given
                var log = new FakeLog();
                var adapter = new CakeBuildLog(log)
                {
                    Verbosity = logVerbosity
                };

                // When
                adapter.Write(messageVerbosity, LogLevel.Information, "Hello World");

                // Then
                Assert.Equal(1, log.Messages.Count);
            }
        }
    }
}
