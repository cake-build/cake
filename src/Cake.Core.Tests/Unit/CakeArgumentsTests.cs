using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeArgumentsTests
    {
        public sealed class HasArguments
        {
            [Theory]
            [InlineData("A", true)]
            [InlineData("B", false)]
            public void Should_Return_Whether_Or_Not_An_Argument_Exist(string key, bool expected)
            {
                // Given
                var dictionary = new Dictionary<string, string> {{"A", "B"}};
                var arguments = new CakeArguments(dictionary);

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
                var dictionary = new Dictionary<string, string> { { "A", "B" } };
                var arguments = new CakeArguments(dictionary);

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
                var dictionary = new Dictionary<string, string> { { "A", "B" } };
                var arguments = new CakeArguments(dictionary);

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
                var dictionary = new Dictionary<string, string> { { "A", "B" } };
                var arguments = new CakeArguments(dictionary);

                // When
                var result = arguments.GetArgument(key);

                // Then
                Assert.Equal(expected, result);
            }
        }
    }
}
