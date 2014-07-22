using System;
using Cake.Scripting.Roslyn;
using Xunit;

namespace Cake.Tests.Unit.Scripting.Roslyn
{
    public sealed class RoslynScriptSessionTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Roslyn_Session_Is_Null()
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
