using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Core.Diagnostics;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Diagnostics
{
    public sealed class LoggingAliasesTests
    {
        public sealed class TheErrorMethod
        {
            [Fact]
            public void Should_Write_Error_Message_To_Log()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Log.Returns(Substitute.For<ICakeLog>());
                const string format = "Hello {0}";
                var args = new object[] { 1 };

                // When
                context.Error(format, args);

                // Then
                context.Log.Received(1).Write(Verbosity.Quiet, LogLevel.Error, format, args);
            }
        }

        public sealed class TheWarningMethod
        {
            [Fact]
            public void Should_Write_Warning_Message_To_Log()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Log.Returns(Substitute.For<ICakeLog>());
                const string format = "Hello {0}";
                var args = new object[] { 1 };

                // When
                context.Warning(format, args);

                // Then
                context.Log.Received(1).Write(Verbosity.Minimal, LogLevel.Warning, format, args);
            }
        }

        public sealed class TheInformationMethod
        {
            [Fact]
            public void Should_Write_Informational_Message_To_Log()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Log.Returns(Substitute.For<ICakeLog>());
                const string format = "Hello {0}";
                var args = new object[] { 1 };

                // When
                context.Information(format, args);

                // Then
                context.Log.Received(1).Write(Verbosity.Normal, LogLevel.Information, format, args);
            }
        }

        public sealed class TheVerboseMethod
        {
            [Fact]
            public void Should_Write_Verbose_Message_Log()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Log.Returns(Substitute.For<ICakeLog>());
                const string format = "Hello {0}";
                var args = new object[] { 1 };

                // When
                context.Verbose(format, args);

                // Then
                context.Log.Received(1).Write(Verbosity.Verbose, LogLevel.Verbose, format, args);
            }
        }

        public sealed class TheDebugMethod
        {
            [Fact]
            public void Should_Write_Debug_Message_To_Log()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Log.Returns(Substitute.For<ICakeLog>());
                const string format = "Hello {0}";
                var args = new object[] { 1 };

                // When
                context.Debug(format, args);

                // Then
                context.Log.Received(1).Write(Verbosity.Diagnostic, LogLevel.Debug, format, args);
            }
        }
    }
}
