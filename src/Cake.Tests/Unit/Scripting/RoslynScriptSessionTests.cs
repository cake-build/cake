using System;
using Cake.Scripting;
using Xunit;

namespace Cake.Tests.Unit.Scripting
{
    public sealed class RoslynScriptSessionTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Engine_Is_Null()
            {
                // When
                var result = Record.Exception(() => new RoslynScriptSession(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("roslynSession", ((ArgumentNullException)result).ParamName);
            }
        }
    }
}
