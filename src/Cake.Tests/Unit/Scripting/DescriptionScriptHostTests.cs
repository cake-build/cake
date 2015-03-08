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
                var context = Substitute.For<ICakeContext>();
                var console = Substitute.For<IConsole>();
                var host = new DescriptionScriptHost(engine, context, console);

                // When
                host.RunTarget("Target");

                // Then
                engine.Received(0).RunTarget(context, "Target");
            }
        }
    }
}
