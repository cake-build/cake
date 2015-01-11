using Cake.Common.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build
{
    public sealed class BuildSystemAliasesTests
    {
        public sealed class TheAppVeyorMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => BuildSystemAliases.AppVeyor(null));

                // Then
                Assert.IsArgumentNullException(result, "context");
            }
        }
    }
}
