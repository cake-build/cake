using Cake.Core;
using Cake.Scripting.Hosts;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit.Scripting.Hosts
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
                var console = Substitute.For<IConsole>();
                var host = new DescriptionScriptHost(engine, console);

                // When
                host.RunTarget("Target");

                // Then
                engine.Received(0).RunTarget("Target");
            }
        }
    }
}
