using Cake.Core;
using Cake.Scripting;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit.Scripting
{
    public sealed class DryRunExecutionStrategyTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new DryRunExecutionStrategy(null));

                // Then
                Assert.IsArgumentNullException(result, "log");
            }
        }

        public sealed class TheExecuteMethod
        {
            [Fact]
            public void Should_Write_Correct_Output_To_The_Log()
            {
                // Given
                var log = new FakeLog();
                var context = Substitute.For<ICakeContext>();
                var strategy = new DryRunExecutionStrategy(log);

                // When
                strategy.Execute(new ActionTask("First"), context);
                strategy.Execute(new ActionTask("Second"), context);

                // Then
                Assert.Equal(2, log.Messages.Count);
                Assert.Equal("1. First", log.Messages[0]);
                Assert.Equal("2. Second", log.Messages[1]);
            }
        }
    }
}