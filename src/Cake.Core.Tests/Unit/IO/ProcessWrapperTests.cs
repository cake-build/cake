using System.Diagnostics;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class ProcessWrapperTests
    {
        public sealed class The_StandardOutputReceived_Method
        {
            [Fact]
            public void Should_Pass_StandardOutput_Through_Handler()
            {
                // Given
                var receivedMessage = string.Empty;
                var fixture = new ProcessWrapperFixture();
                fixture.StandartOutputHandler = (s) => receivedMessage = s;
                var wrapper = fixture.CreateProcessWrapper();

                // When
                wrapper.StandardOutputReceived("message");

                // Then
                Assert.Equal("message", receivedMessage);
            }
        }

        public sealed class The_StandardErrorReceived_Method
        {
            [Fact]
            public void Should_Pass_StandardError_Through_Handler()
            {
                // Given
                var receivedMessage = string.Empty;
                var fixture = new ProcessWrapperFixture();
                fixture.StandardErrorHandler = (s) => receivedMessage = s;
                var wrapper = fixture.CreateProcessWrapper();

                // When
                wrapper.StandardErrorReceived("message");

                // Then
                Assert.Equal("message", receivedMessage);
            }
        }
    }
}
