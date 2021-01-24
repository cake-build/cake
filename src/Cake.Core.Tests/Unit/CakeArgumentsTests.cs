using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeArgumentsTests
    {
        [Fact]
        public void Should_Return_Null_If_Argument_Is_Missing()
        {
            // Given
            var arguments = new CakeArguments(new List<Tuple<string, string>>
            {
                Tuple.Create("FOO", "BAR"),
                Tuple.Create("BAZ", "CORGI"),
            }.ToLookup(x => x.Item1, x => x.Item2));

            // When
            var result = arguments.GetArgument("WALDO");

            // Then
            Assert.Null(result);
        }

        [Fact]
        public void Should_Return_Argument_If_One_With_The_Specified_Name_Exist()
        {
            // Given
            var arguments = new CakeArguments(new List<Tuple<string, string>>
            {
                Tuple.Create("FOO", "BAR"),
                Tuple.Create("BAZ", "CORGI"),
            }.ToLookup(x => x.Item1, x => x.Item2));

            // When
            var result = arguments.GetArgument("BAZ");

            // Then
            Assert.Equal("CORGI", result);
        }

        [Fact]
        public void Should_Return_The_Last_Argument_If_Multiple_Ones_With_The_Same_Name_Exist()
        {
            // Given
            var arguments = new CakeArguments(new List<Tuple<string, string>>
            {
                Tuple.Create("FOO", "BAR"),
                Tuple.Create("FOO", "BAZ"),
            }.ToLookup(x => x.Item1, x => x.Item2));

            // When
            var result = arguments.GetArgument("FOO");

            // Then
            Assert.Equal("BAZ", result);
        }
    }
}