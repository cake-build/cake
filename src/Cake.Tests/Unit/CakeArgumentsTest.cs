using System.Collections.Generic;
using Xunit;

namespace Cake.Tests.Unit
{
    public sealed class CakeArgumentsTests
    {
        public sealed class TheSetArgumentsMethod
        {
            [Fact]
            public void Should_Set_Arguments()
            {
                // Given
                var arguments = new CakeArguments();                

                // When
                arguments.SetArguments(new Dictionary<string, string> { { "A", "B" } });

                // Then
                Assert.Equal(1, arguments.Arguments.Count);
            }

            [Fact]
            public void Should_Replace_Arguments_If_New_Ones_Are_Set()
            {
                // Given
                var arguments = new CakeArguments();
                arguments.SetArguments(new Dictionary<string, string> { { "A", "B" } });

                // When
                arguments.SetArguments(new Dictionary<string, string> { { "C", "D" }, { "D", "E" } });

                // Then
                Assert.Equal(2, arguments.Arguments.Count);
            }
        }

        public sealed class HasArguments
        {
            [Theory]
            [InlineData("A", true)]
            [InlineData("B", false)]
            public void Should_Return_Whether_Or_Not_An_Argument_Exist(string key, bool expected)
            {
                // Given
                var arguments = new CakeArguments();
                arguments.SetArguments(new Dictionary<string, string> { { "A", "B" } });

                // When
                var result = arguments.HasArgument(key);

                // Then
                Assert.Equal(expected, result);
            }

            [Theory]
            [InlineData("a", true)]
            [InlineData("b", false)]
            public void Should_Be_Case_Insensitive(string key, bool expected)
            {
                // Given
                var arguments = new CakeArguments();
                arguments.SetArguments(new Dictionary<string, string> { { "A", "B" } });

                // When
                var result = arguments.HasArgument(key);

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class GetArguments
        {
            [Theory]
            [InlineData("A", "B")]
            [InlineData("B", null)]
            public void Should_Return_Argument_Exist(string key, string expected)
            {
                // Given
                var arguments = new CakeArguments();
                arguments.SetArguments(new Dictionary<string, string> { { "A", "B" } });

                // When
                var result = arguments.GetArgument(key);

                // Then
                Assert.Equal(expected, result);
            }

            [Theory]
            [InlineData("a", "B")]
            [InlineData("b", null)]
            public void Should_Be_Case_Insensitive(string key, string expected)
            {
                // Given
                var arguments = new CakeArguments();
                arguments.SetArguments(new Dictionary<string, string> { { "A", "B" } });

                // When
                var result = arguments.GetArgument(key);

                // Then
                Assert.Equal(expected, result);
            }
        }
    }
}
