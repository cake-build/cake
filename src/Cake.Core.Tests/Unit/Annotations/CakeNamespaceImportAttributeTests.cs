using System;
using Cake.Core.Annotations;
using Xunit;

namespace Cake.Core.Tests.Unit.Annotations
{
    public sealed class CakeNamespaceImportAttributeTests
    {
        public sealed class TheConstuctor
        {
            [Fact]
            public void Should_Throw_If_Namespace_Is_Null()
            {
                // Given, When
                var exception = Record.Exception(() => new CakeNamespaceImportAttribute(null));

                // Then
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("namespace", ((ArgumentNullException)exception).ParamName);
            }
        }
    }
}
