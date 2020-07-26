using Cake.Features.Introspection;
using Cake.Testing;
using Cake.Tests.Fakes;
using Xunit;

namespace Cake.Tests.Unit.Features
{
    public sealed class InfoFeatureTests
    {
        [Fact]
        public void Should_Return_Success()
        {
            // Given
            var console = new FakeConsole();
            var resolver = new FakeVersionResolver("1.2.3", "3.2.1");
            var feature = new InfoFeature(console, resolver);

            // When
            var result = feature.Run();

            // Then
            Assert.Equal(0, result);
        }

        [Fact]
        public void Should_Output_Version_To_Console()
        {
            // Given
            var console = new FakeConsole();
            var resolver = new FakeVersionResolver("1.2.3", "3.2.1");
            var feature = new InfoFeature(console, resolver);

            // When
            var result = feature.Run();

            // Then
            Assert.Contains("Version: 1.2.3", console.Messages);
            Assert.Contains("Details: 3.2.1", console.Messages);
        }
    }
}
