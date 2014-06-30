using Cake.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit.Scripting
{
    public sealed class DescriptionScriptHostTests
    {
        public sealed class TheRunTargetMethod
        {
            [Fact]
            public void Should_Not_Call_To_Engine()
            {
                // Given
                var fixture = new DescriptionScriptHostFixture();
                var host = fixture.CreateHost();

                // When
                host.RunTarget("Target");

                // Then
                fixture.Engine.Received(0).RunTarget("Target");
            }
        }
    }
}