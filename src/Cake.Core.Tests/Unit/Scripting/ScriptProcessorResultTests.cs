using System;
using System.Linq;
using Cake.Core.IO;
using Cake.Core.Scripting;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting
{
    public sealed class ScriptProcessorResultTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Code_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => 
                    new ScriptProcessorResult(null, "/Working", Enumerable.Empty<FilePath>()));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("code", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Root_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() =>
                    new ScriptProcessorResult("", null, Enumerable.Empty<FilePath>()));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("root", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Root_Is_Relative()
            {
                // Given, When
                var result = Record.Exception(() =>
                    new ScriptProcessorResult("", "Working", Enumerable.Empty<FilePath>()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Script root cannot be relative.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_References_Are_Null()
            {
                // Given, When
                var result = Record.Exception(() =>
                    new ScriptProcessorResult("", "/Working", null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("references", ((ArgumentNullException)result).ParamName);
            }
        }
    }
}
