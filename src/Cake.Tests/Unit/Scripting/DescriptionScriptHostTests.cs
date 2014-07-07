using Cake.Core;
using Cake.Scripting;
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
                var engine = Substitute.For<ICakeEngine>();
                var host = new DescriptionScriptHost(engine);

                // When
                host.RunTarget("Target");

                // Then
                engine.Received(0).RunTarget("Target");
            }
        }
    }
}
