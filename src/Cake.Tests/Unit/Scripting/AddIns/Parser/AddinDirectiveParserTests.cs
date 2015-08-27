using System;
using Cake.Core.Scripting.Processors.Parsers;
using Cake.Tests.Fixtures;
using Xunit;

namespace Cake.Tests.Unit.Scripting.AddIns.Parser
{
  public class AddinDirectiveParserTests
  {
    public class TheParseMethod
    {
      [Fact]
      public void Should_Return_False_If_Arguments_Are_Null()
      {
        // Given
        var fixture = new AddInDirectiveParserFixture();
        var parser = new AddInDirectiveParser(fixture.Environment, fixture.FileSystem, fixture.Log);

        // When
        var result = parser.Parse(null);

        // Then
        Assert.False(result.Valid);
      }

      [Fact]
      public void Can_Parse_Empty_Parameters()
      {
        // Given
        var fixture = new AddInDirectiveParserFixture();
        var parser = new AddInDirectiveParser(fixture.Environment, fixture.FileSystem, fixture.Log);

        // When
        var result = parser.Parse(new string[] { });

        // Then
        Assert.NotNull(result);
        Assert.False(result.Valid);
      }

      [Theory]
      [InlineData("#addin xxx", "xxx")]
      [InlineData("#addin yyy", "yyy")]
      public void Can_Parse_AddinId(string input, string addinId)
      {
        // Given
        var fixture = new AddInDirectiveParserFixture();
        var parser = new AddInDirectiveParser(fixture.Environment, fixture.FileSystem, fixture.Log);
        var tokens = input.Split(new[] { ' ' }, StringSplitOptions.None);

        // When
        var result = parser.Parse(tokens);

        // Then
        Assert.NotNull(result);
        Assert.True(result.Valid);
        Assert.Equal(addinId, result.AddInId);
      }

      [Theory]
      [InlineData("#addin xxx http://myserver.org/", "xxx", "http://myserver.org/")]
      [InlineData("#addin yyy http://yourserver.org/", "yyy", "http://yourserver.org/")]
      public void Can_Parse_Source(string input, string addinId, string source)
      {
        // Given
        var fixture = new AddInDirectiveParserFixture();
        var parser = new AddInDirectiveParser(fixture.Environment, fixture.FileSystem, fixture.Log);
        var tokens = input.Split(new[] { ' ' }, StringSplitOptions.None);

        // When
        var result = parser.Parse(tokens);

        // Then
        Assert.NotNull(result);
        Assert.True(result.Valid);
        Assert.Equal(addinId, result.AddInId);
        Assert.NotNull(result.InstallArguments);
        Assert.True(result.InstallArguments.Render().Contains(source));
      }

      [Theory]
      [InlineData("#addin ninject -o c:\\foo", "install \"ninject\" -ExcludeVersion -NonInteractive -NoCache -OutputDirectory \"c:/Addins\"")]
      [InlineData("#addin ninject -output c:\\foo", "install \"ninject\" -ExcludeVersion -NonInteractive -NoCache -OutputDirectory \"c:/Addins\"")]
      [InlineData("#addin ninject -OutputFolder c:\\foo", "install \"ninject\" -ExcludeVersion -NonInteractive -NoCache -OutputDirectory \"c:/Addins\"")]
      public void Should_Ignore_Passed_Output_Folder(string input, string expected)
      {
        // Given
        var fixture = new AddInDirectiveParserFixture();
        var parser = new AddInDirectiveParser(fixture.Environment, fixture.FileSystem, fixture.Log);
        var tokens = input.Split(new[] { ' ' }, StringSplitOptions.None);

        // When
        var result = parser.Parse(tokens);

        // Then
        Assert.NotNull(result);
        Assert.NotNull(result.InstallArguments);
        Assert.Equal(expected, result.InstallArguments.Render());
      }

      [Theory]
      [InlineData("#addin ninject -ExcludeVersion", "install \"ninject\" -ExcludeVersion -NonInteractive -NoCache -OutputDirectory \"c:/Addins\"")]
      [InlineData("#addin ninject -NonInteractive", "install \"ninject\" -NonInteractive -ExcludeVersion -NoCache -OutputDirectory \"c:/Addins\"")]
      [InlineData("#addin ninject -NoCache", "install \"ninject\" -NoCache -ExcludeVersion -NonInteractive -OutputDirectory \"c:/Addins\"")]
      [InlineData("#addin ninject -ExcludeVersion -NonInteractive -NoCache", "install \"ninject\" -ExcludeVersion -NonInteractive -NoCache -OutputDirectory \"c:/Addins\"")]
      public void Should_Only_Have_One_Of_Compulsory_Arguments(string input, string expected)
      {
        // Given
        var fixture = new AddInDirectiveParserFixture();
        var parser = new AddInDirectiveParser(fixture.Environment, fixture.FileSystem, fixture.Log);
        var tokens = input.Split(new[] { ' ' }, StringSplitOptions.None);

        // When
        var result = parser.Parse(tokens);

        // Then
        Assert.NotNull(result);
        Assert.NotNull(result.InstallArguments);
        Assert.Equal(expected, result.InstallArguments.Render());
      }

      [Theory]
      [InlineData("#addin ninject -o c:\\foo", "install \"ninject\" -ExcludeVersion -NonInteractive -NoCache -OutputDirectory \"c:/Addins\"")]
      [InlineData("#addin yyy -Source http://yourserver.org/ -Pre", "install \"yyy\" -Source http://yourserver.org/ -Pre -ExcludeVersion -NonInteractive -NoCache -OutputDirectory \"c:/Addins\"")]
      [InlineData("#addin yyy http://yourserver.org/", "install \"yyy\" -Source \"http://yourserver.org/\" -ExcludeVersion -NonInteractive -NoCache -OutputDirectory \"c:/Addins\"")]
      public void Should_Return_Consistent_Result(string input, string expected)
      {
        // Given
        var fixture = new AddInDirectiveParserFixture();
        var parser = new AddInDirectiveParser(fixture.Environment, fixture.FileSystem, fixture.Log);
        var tokens = input.Split(new[] { ' ' }, StringSplitOptions.None);

        // When
        var result = parser.Parse(tokens);

        // Then
        Assert.NotNull(result);
        Assert.NotNull(result.InstallArguments);
        Assert.Equal(expected, result.InstallArguments.Render());
      }

      [Theory]
      [InlineData("#addin ninject -netVersion net45", "install \"ninject\" -ExcludeVersion -NonInteractive -NoCache -OutputDirectory \"c:/Addins\"", "net45")]
      [InlineData("#addin yyy -Source http://yourserver.org/ -Pre  -netVersion net40", "install \"yyy\" -Source http://yourserver.org/ -Pre -ExcludeVersion -NonInteractive -NoCache -OutputDirectory \"c:/Addins\"", "net40")]
      [InlineData("#addin yyy http://yourserver.org/  -netVersion net35", "install \"yyy\" -Source \"http://yourserver.org/\" -ExcludeVersion -NonInteractive -NoCache -OutputDirectory \"c:/Addins\"", "net35")]
      public void Should_Populate_NetVersion_Argument(string input, string expected, string netVersion)
      {
        // Given
        var fixture = new AddInDirectiveParserFixture();
        var parser = new AddInDirectiveParser(fixture.Environment, fixture.FileSystem, fixture.Log);
        var tokens = input.Split(new[] { ' ' }, StringSplitOptions.None);

        // When
        var result = parser.Parse(tokens);

        // Then
        Assert.NotNull(result);
        Assert.NotNull(result.InstallArguments);
        Assert.Equal(expected, result.InstallArguments.Render());
        Assert.Equal(netVersion, result.NetVersion);
      }

      [Fact]
      public void Will_Populate_Default_NetVersion()
      {
        // Given
        var fixture = new AddInDirectiveParserFixture();
        var parser = new AddInDirectiveParser(fixture.Environment, fixture.FileSystem, fixture.Log);
        var tokens = "#addin ninject".Split(new[] { ' ' }, StringSplitOptions.None);

        // When
        var result = parser.Parse(tokens);

        // Then
        Assert.NotNull(result);
        Assert.Equal("net45", result.NetVersion);
      }
    }
  }
}
