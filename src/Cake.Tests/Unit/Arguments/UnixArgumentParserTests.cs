using Cake.Arguments;
using Xunit;

namespace Cake.Tests.Unit.Arguments
{
    public class UnixCommandArgumentNameTests
    {
        [Theory]
        [InlineData("--test")]
        [InlineData("-t")]
        public void Constructor_Should_Store_Valid_Value(string name)
        {
            var arg = new CommandArgumentNameAttribute(name);
            Assert.Equal(name, arg.Name);
        }

        [Fact]
        public void Should_Throw_If_Name_Is_Null()
        {
            var result = Record.Exception(() => new CommandArgumentNameAttribute(null));

            Assert.IsArgumentException(result, "name", "CommandArgumentName can not be empty or null");
        }

        [Fact]
        public void Should_Throw_If_Name_Has_No_Dashes()
        {
            var result = Record.Exception(() => new CommandArgumentNameAttribute("no-dash"));

            Assert.IsArgumentException(result, "name", "CommandArgumentName must start with either one or two dashes");
        }

        [Theory]
        [InlineData("---three-dashes")]
        [InlineData("----four-dashes")]
        public void Should_Throw_If_Name_Has_Too_Many_Dashes(string name)
        {
            var result = Record.Exception(() => new CommandArgumentNameAttribute(name));

            Assert.IsArgumentException(result, "name", "CommandArgumentName must start with either one or two dashes");
        }

        [Theory]
        [InlineData("--in the_middle")]
        [InlineData("-- AtStart")]
        [InlineData("--AtEnd ")]
        public void Should_Throw_If_Name_Contains_WhiteSpace(string name)
        {
            var result = Record.Exception(() => new CommandArgumentNameAttribute(name));

            Assert.IsArgumentException(result, "name", "CommandArgumentName can not contain a space");
        }

        [Theory]
        [InlineData("-")]
        [InlineData("--")]
        public void Should_Throw_If_Name_Is_Missing(string name)
        {
            var result = Record.Exception(() => new CommandArgumentNameAttribute(name));
            Assert.IsArgumentException(result, "name",
                "CommandArgumentName must have a name, only dashes is not allowed");
        }

        [Fact]
        public void Should_Throw_If_ShortName_Has_More_Than_One_Character()
        {
            var result = Record.Exception(() => new CommandArgumentNameAttribute("-too_long"));
            Assert.IsArgumentException(result, "name",
                "CommandArgumentName with a single dash can only have one character following it");
        }

        [Fact]
        public void Should_Not_Throw_If_ShortName_Has_More_Than_One_Character_And_Old_Style()
        {
            var arg = new CommandArgumentNameAttribute("-too_long", true);
            Assert.Equal("-too_long", arg.Name);
        }

        [Fact]
        public void Should_Throw_If_LongName_Has_Only_One_Character()
        {
            var result = Record.Exception(() => new CommandArgumentNameAttribute("--s"));
            Assert.IsArgumentException(result, "name",
                "CommandArgumentName with a double dash must use more than one character");
        }

        [Fact]
        public void Should_Not_Throw_If_LongName_Has_Only_One_Character_And_Old_Style()
        {
            var arg = new CommandArgumentNameAttribute("--s", true);
            Assert.Equal("--s", arg.Name);
        }
    }
}