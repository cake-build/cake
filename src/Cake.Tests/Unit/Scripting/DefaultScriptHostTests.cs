using Cake.Core;
using Cake.Scripting;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit.Scripting
{
    public sealed class DefaultScriptHostTests
    {
        public sealed class TheRunTargetMethod
        {
            [Fact]
            public void Should_Proxy_Call_To_Engine()
            {
                // Given
                var engine = Substitute.For<ICakeEngine>();
                var host = new DefaultScriptHost(engine);

                // When
                host.RunTarget("Target");

                // Then
                engine.Received(1).RunTarget("Target");
            }
        }
    }
}
