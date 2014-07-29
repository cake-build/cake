using System;
using Cake.Core.Annotations;
using Xunit;

namespace Cake.Core.Tests.Unit.Annotations
{
    public sealed class CakeAliasCategoryAttributeTests
    {
        public sealed class TheConstuctor
        {
            [Fact]
            public void Should_Throw_If_Category_Name_Is_Null()
            {
                // Given, When
                var exception = Record.Exception(() => new CakeAliasCategoryAttribute(null));

                // Then
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("name", ((ArgumentNullException)exception).ParamName);
            }
        }
    }
}
