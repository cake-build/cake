using System;
using System.Runtime.InteropServices.ComTypes;
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
        Assert.Equal(source, result.Source);
      }
    }
  }
}
