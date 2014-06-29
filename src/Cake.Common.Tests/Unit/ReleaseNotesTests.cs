using System;
using System.Linq;
using Xunit;

namespace Cake.Common.Tests.Unit
{
    public sealed class ReleaseNotesTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Version_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new ReleaseNotes(null, Enumerable.Empty<string>()));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("version", ((ArgumentNullException)result).ParamName);
            }
        }
    }
}
