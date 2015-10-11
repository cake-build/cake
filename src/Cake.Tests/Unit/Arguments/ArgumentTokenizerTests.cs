using System.Linq;
using Cake.Arguments;
using Xunit;

namespace Cake.Tests.Unit.Arguments
{
    public sealed class ArgumentTokenizerTests
    {
        public sealed class TheTokenizeMethod
        {
            [Fact]
            public void Should_Return_Zero_Results_If_Input_String_Is_Null()
            {
                // Given
                var input = string.Empty;

                // When
                var result = ArgumentTokenizer.Tokenize(input).ToArray();

                // Then
                Assert.Equal(0, result.Length);
            }

            [Fact]
            public void Should_Parse_Single_Quoted_Argument_Without_Space_In_It()
            {
                // Given
                const string input = "\"C:\\cake-walk\\cake.exe\"";

                // When
                var result = ArgumentTokenizer.Tokenize(input).ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.Equal("\"C:\\cake-walk\\cake.exe\"", result[0]);
            }

            [Fact]
            public void Should_Parse_Single_Quoted_Argument_With_Space_In_It()
            {
                // Given
                const string input = "\"C:\\cake walk\\cake.exe\"";

                // When
                var result = ArgumentTokenizer.Tokenize(input).ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.Equal("\"C:\\cake walk\\cake.exe\"", result[0]);
            }

            [Fact]
            public void Should_Parse_Multiple_Quoted_Arguments()
            {
                // Given
                const string input = "\"C:\\cake-walk\\cake.exe\" \"build.cake\" \"-dryrun\"";

                // When
                var result = ArgumentTokenizer.Tokenize(input).ToArray();

                // Then
                Assert.Equal(3, result.Length);
                Assert.Equal("\"C:\\cake-walk\\cake.exe\"", result[0]);
                Assert.Equal("\"build.cake\"", result[1]);
                Assert.Equal("\"-dryrun\"", result[2]);
            }

            [Fact]
            public void Should_Parse_Multiple_Mixed_Arguments()
            {
                // Given
                const string input = "\"C:\\cake-walk\\cake.exe\" build.cake -verbosity \"diagnostic\"";

                // When
                var result = ArgumentTokenizer.Tokenize(input).ToArray();

                // Then
                Assert.Equal(4, result.Length);
                Assert.Equal("\"C:\\cake-walk\\cake.exe\"", result[0]);
                Assert.Equal("build.cake", result[1]);
                Assert.Equal("-verbosity", result[2]);
                Assert.Equal("\"diagnostic\"", result[3]);
            }
        }
    }
}