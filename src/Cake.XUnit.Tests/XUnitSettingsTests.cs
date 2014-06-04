using System;
using Xunit;

namespace Cake.XUnit.Tests
{
    public sealed class XUnitSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_No_Assmebly_Paths_Were_Provided()
            {
                // Given, When
                var result = Record.Exception(() => new XUnitSettings(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("assemblyPath", ((ArgumentNullException) result).ParamName);
            }
        }
    }
}
