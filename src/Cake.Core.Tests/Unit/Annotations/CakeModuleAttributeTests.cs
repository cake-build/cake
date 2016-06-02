using Cake.Core.Annotations;
using Xunit;

namespace Cake.Core.Tests.Unit.Annotations
{
    public sealed class CakeModuleAttributeTests
    {
        [Fact]
        public void Should_Throw_If_Module_Type_Is_Null()
        {
            // Given, When
            var result = Record.Exception(() => new CakeModuleAttribute(null));

            // Then
            Assert.IsArgumentNullException(result, "moduleType");
        }
    }
}
