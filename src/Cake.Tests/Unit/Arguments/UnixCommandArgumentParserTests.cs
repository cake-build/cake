using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Arguments;
using Xunit;

namespace Cake.Tests.Unit.Arguments
{
    public class UnixCommandArgumentParserTests
    {
        [Fact]
        public void Should_Have_Nothing_When_No_Arguments()
        {
            // Arrange
            var empty_argument = "";
            var command_parser = new UnixCommandParser();

            // Act
            var range = command_parser.Parse(empty_argument);

            // Assert
            Assert.Empty(range);
        }

        [Fact]
        public void Should_Have_An_Empty_Argument()
        {
            // Arrange
            var argument = "--no-argument";
            var command_parser = new UnixCommandParser();

            // Act
            var range = command_parser.Parse(argument);

            // Assert
            Assert.Equal(1, range.Count);
            Assert.Equal(argument, range.Single().Key);
            Assert.Null(range.Single().Value);
        }

        [Fact]
        public void Should_Have_Multiple_Empty_Arguments()
        {
            // Arrange
            var argument = "--first --second";
            var command_parser = new UnixCommandParser();

            // Act
            var range = command_parser.Parse(argument);

            // Assert
            Assert.Equal(2, range.Count);
            Assert.True(range.All(x => x.Value == null));
        }

        [Fact]
        public void Should_Not_Have_Arguments_With_No_Key()
        {
            // Arrange
            // Introduced extra spaces.
            var argument = "--first     --second";
            var command_parser = new UnixCommandParser();

            // Act
            var range = command_parser.Parse(argument);

            // Assert
            Assert.Equal(2, range.Count);
            Assert.True(range.All(x => x.Value == null));
        }

        [Fact]
        public void Should_Support_Duplicate_Keys_With_No_Value()
        {
            // Arrange
            // Introduced extra spaces.
            var argument = "--k1 --k1";
            var command_parser = new UnixCommandParser();

            // Act
            var range = command_parser.Parse(argument);

            // Assert
            Assert.Equal(2, range.Count);
            Assert.True(range.All(x => x.Key == "--k1"));
        }

        [Fact]
        public void Should_Support_Duplicate_Keys_With_Different_Values()
        {
            // Arrange
            // Introduced extra spaces.
            var argument = "--k1 a --k1 b";
            var command_parser = new UnixCommandParser();

            // Act
            var range = command_parser.Parse(argument);

            // Assert
            Assert.Equal(2, range.Count);
            Assert.True(range.All(x => x.Key == "--k1"));
            Assert.Equal("a", range.First().Value);
            Assert.Equal("b", range.Last().Value);
        }

        [Fact]
        public void Should_Have_Argument_With_No_Quoted_Value()
        {
            // Arrange
            var argument = "--key value";
            var command_parser = new UnixCommandParser();

            // Act
            var range = command_parser.Parse(argument);

            // Assert
            Assert.Equal(1, range.Count);
            Assert.Equal("--key", range.Single().Key);
            Assert.Equal("value", range.Single().Value);
        }

        [Fact]
        public void Should_Have_Argument_With_NoQuoted_Value_Even_With_Extra_Spaces()
        {
            // Arrange
            var argument = "--key   value";
            var command_parser = new UnixCommandParser();

            // Act
            var range = command_parser.Parse(argument);

            // Assert
            Assert.Equal(1, range.Count);
            Assert.Equal("--key", range.Single().Key);
            Assert.Equal("value", range.Single().Value);
        }

        [Fact]
        public void Should_Support_Argument_Value_With_Space_Single_Quote()
        {
            // Arrange
            var argument = "--key 'abc def'";
            var command_parser = new UnixCommandParser();

            // Act
            var range = command_parser.Parse(argument);

            // Assert
            Assert.Equal(1, range.Count);
            Assert.Equal("--key", range.Single().Key);
            Assert.Equal("abc def", range.Single().Value);
        }

        [Fact]
        public void Should_Support_Argument_Value_With_Space_Double_Quote()
        {
            // Arrange
            var argument = "--key \"abc def\"";
            var command_parser = new UnixCommandParser();

            // Act
            var range = command_parser.Parse(argument);

            // Assert
            Assert.Equal(1, range.Count);
            Assert.Equal("--key", range.Single().Key);
            Assert.Equal("abc def", range.Single().Value);
        }

        [Fact]
        public void Should_Support_Argument_Value_With_Double_Quote_And_Single_Quote()
        {
            // Arrange
            var argument = "--k1 \"abc def\" --k2 'ghi jkl'";
            var command_parser = new UnixCommandParser();

            // Act
            var range = command_parser.Parse(argument);

            // Assert
            Assert.Equal(2, range.Count);
            Assert.Equal("--k1", range.First().Key);
            Assert.Equal("abc def", range.First().Value);

            Assert.Equal("--k2", range.Last().Key);
            Assert.Equal("ghi jkl", range.Last().Value);
        }

        [Fact]
        public void Should_Support_Escaped_Arguments()
        {
            // Arrange
            var argument = "--k1 'contains another \\'quoted value\\''";
            var command_parser = new UnixCommandParser();

            // Act
            var range = command_parser.Parse(argument);

            // Assert
            Assert.Equal(1, range.Count);
            Assert.Equal("--k1", range.Single().Key);
            Assert.Equal("contains another 'quoted value'", range.Single().Value);
        }

        [Fact]
        public void Should_Support_Double_Escaped_Arguments()
        {
            // Arrange
            var argument = "--k1 'contains another \\'quoted value\\''";
            var command_parser = new UnixCommandParser();

            // Act
            var range = command_parser.Parse(argument);

            // Assert
            Assert.Equal(1, range.Count);
            Assert.Equal("--k1", range.Single().Key);
            Assert.Equal("contains another 'quoted value'", range.Single().Value);
        }

        [Theory]
        [InlineData("'no terminator")]
        [InlineData("\"no terminator")]
        public void Should_Throw_On_Unbalanced_Quotes(string arg_value)
        {
            // Arrange
            var argument = "--k1 " + arg_value;
            var command_parser = new UnixCommandParser();

            // Act
            var result = Record.Exception(() => command_parser.Parse(argument));

            // Assert
            Assert.IsExceptionWithMessage<Exception>(result, "Unbalanced quoted argument: Missing quote terminator");
        }

        [Theory]
        [InlineData("'no terminator\\")]
        [InlineData("\"no terminator\\")]
        public void Should_Throw_On_Escaping_EOF(string arg_value)
        {
            // Arrange
            var argument = "--k1 " + arg_value;
            var command_parser = new UnixCommandParser();

            // Act
            var result = Record.Exception(() => command_parser.Parse(argument));

            // Assert
            Assert.IsExceptionWithMessage<Exception>(result, "Unbalanced quoted argument: EOF can't be escaped");
        }

        [Theory]
        [InlineData("this_value_has_no_key")]
        [InlineData("--valid value second_value_has_no_key")]
        public void Should_Throw_On_Missing_Key(string argument)
        {
            // Arrange
            var command_parser = new UnixCommandParser();

            // Act
            var result = Record.Exception(() => command_parser.Parse(argument));

            // Assert
            Assert.IsExceptionWithMessage<Exception>(result, "An \"--key\" should preceed the value.");
        }
    }
}
