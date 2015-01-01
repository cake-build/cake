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
                var result = Record.Exception(() => new CakeAliasCategoryAttribute(null));

                // Then
                Assert.IsArgumentNullException(result, "name");
            }
        }
    }
}
