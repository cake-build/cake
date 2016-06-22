using System;
using System.Linq;

using Cake.Core.Diagnostics;
using Cake.Diagnostics;
using Cake.Testing;
using Cake.Tests.Diagnostics;

using Xunit;

namespace Cake.Tests.Unit.Diagnostics
{
    public class LogPipelineTests
    {
        [Fact]
        public void Should_Throw_If_Log_Is_Null()
        {
            // Given
            LogPipelineFixture fixture = new LogPipelineFixture();
            fixture.Log = null;

            // When
            Exception result = Record.Exception(() => fixture.CreateLogPipeline());

            // Then
            Assert.IsArgumentNullException(result, "defaultLog");
        }

        [Fact]
        public void Pipeline_Writes_To_Default_Log()
        {
            // Given
            LogPipelineFixture fixture = new LogPipelineFixture();
            var pipeline = fixture.CreateLogPipeline();
            string message = Guid.NewGuid().ToString();

            // When
            pipeline.CakeLog.Write(fixture.Log.Verbosity, LogLevel.Information, message);

            // Then
            Assert.Equal(1, fixture.Log.Entries.Count);
            Assert.Equal(fixture.Log.Verbosity, fixture.Log.Entries[0].Verbosity);
            Assert.Equal(LogLevel.Information, fixture.Log.Entries[0].Level);
            Assert.Equal(message, fixture.Log.Entries[0].Message);
        }

        [Fact]
        public void Pipeline_Writes_To_Multiple_Logs()
        {
            // Given
            LogPipelineFixture fixture = new LogPipelineFixture();
            var pipeline = fixture.CreateLogPipeline();

            FakeLog newLog = new FakeLog();
            pipeline.AddLog(newLog);
            string message = Guid.NewGuid().ToString();

            // When
            pipeline.CakeLog.Write(fixture.Log.Verbosity, LogLevel.Information, message);

            // Then
            Assert.Equal(1, fixture.Log.Entries.Count);
            Assert.Equal(fixture.Log.Verbosity, fixture.Log.Entries[0].Verbosity);
            Assert.Equal(LogLevel.Information, fixture.Log.Entries[0].Level);
            Assert.Equal(message, fixture.Log.Entries[0].Message);

            Assert.Equal(1, newLog.Entries.Count);
            Assert.Equal(fixture.Log.Verbosity, newLog.Entries[0].Verbosity);
            Assert.Equal(LogLevel.Information, newLog.Entries[0].Level);
            Assert.Equal(message, newLog.Entries[0].Message);
        }

        [Theory]
        [InlineData(Verbosity.Quiet, Verbosity.Minimal)]
        [InlineData(Verbosity.Minimal, Verbosity.Normal)]
        [InlineData(Verbosity.Normal, Verbosity.Verbose)]
        [InlineData(Verbosity.Verbose, Verbosity.Diagnostic)]
        public void Should_Drop_Log_Messages_Written_With_A_Lower_Verbosity_Than_Allowed(Verbosity logVerbosity, Verbosity messageVerbosity)
        {
            // Given
            LogPipelineFixture fixture = new LogPipelineFixture();
            CakeLogPipeline pipeline = fixture.CreateLogPipeline();

            pipeline.CakeLog.Verbosity = logVerbosity;

            string message = Guid.NewGuid().ToString();

            // When
            pipeline.CakeLog.Write(messageVerbosity, LogLevel.Information, message);

            // Then
            Assert.Equal(0, fixture.Log.Entries.Count);
        }

        [Theory]
        [InlineData(Verbosity.Minimal, Verbosity.Quiet)]
        [InlineData(Verbosity.Normal, Verbosity.Minimal)]
        [InlineData(Verbosity.Verbose, Verbosity.Normal)]
        [InlineData(Verbosity.Diagnostic, Verbosity.Verbose)]
        public void Should_Write_Log_Messages_Written_With_A_Higher_Verbosity_Than_Allowed(Verbosity logVerbosity, Verbosity messageVerbosity)
        {
            // Given
            LogPipelineFixture fixture = new LogPipelineFixture();
            CakeLogPipeline pipeline = fixture.CreateLogPipeline();

            pipeline.CakeLog.Verbosity = logVerbosity;

            string message = Guid.NewGuid().ToString();

            // When
            pipeline.CakeLog.Write(messageVerbosity, LogLevel.Information, message);

            // Then
            Assert.Equal(1, fixture.Log.Entries.Count);
        }

        [Theory]
        [InlineData(Verbosity.Quiet)]
        [InlineData(Verbosity.Minimal)]
        [InlineData(Verbosity.Normal)]
        [InlineData(Verbosity.Verbose)]
        [InlineData(Verbosity.Diagnostic)]
        public void Should_Inherit_Verbosity_From_Default_Log(Verbosity logVerbosity)
        {
            // Given
            LogPipelineFixture fixture = new LogPipelineFixture();
            fixture.Log.Verbosity = logVerbosity;
            CakeLogPipeline pipeline = fixture.CreateLogPipeline();

            // Then
            Assert.Equal(logVerbosity, pipeline.CakeLog.Verbosity);
        }


        [Fact]
        public void Pipeline_AddLog()
        {
            // Given
            LogPipelineFixture fixture = new LogPipelineFixture();
            var pipeline = fixture.CreateLogPipeline();

            // When
            FakeLog newLog = new FakeLog();
            pipeline.AddLog(newLog);

            // Then
            Assert.Equal(2, pipeline.Listeners.Count());
            Assert.Same(fixture.Log, pipeline.Listeners.First());
            Assert.Same(newLog, pipeline.Listeners.Skip(1).First());
        }

        [Fact]
        public void Pipeline_RemoveLog()
        {
            // Given
            LogPipelineFixture fixture = new LogPipelineFixture();
            var pipeline = fixture.CreateLogPipeline();

            FakeLog newLog = new FakeLog();
            pipeline.AddLog(newLog);
            Assert.Same(newLog, pipeline.Listeners.Skip(1).First());

            // When
            pipeline.RemoveLog(newLog);

            // Then
            Assert.Equal(1, pipeline.Listeners.Count());
            Assert.Same(fixture.Log, pipeline.Listeners.First());
        }
    }
}