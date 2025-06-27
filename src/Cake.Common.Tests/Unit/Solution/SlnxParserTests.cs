using System.Linq;
using Cake.Common.Solution;
using Cake.Common.Tests.Fixtures.Solution;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Solution;

public class SlnxParserTests
{
    [Fact]
    public void Should_Parse_Slnx_File_Correctly()
    {
        // Arrange
        var environment = FakeEnvironment.CreateUnixEnvironment();
        var fileSystem = new FileSystem();

        // Create valid slnx
        var slnxContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
                <Solution Version=""1.0"">
                  <Project Path=""ConsoleApp/ConsoleApp.csproj"" />
                </Solution>";

        var slnxPath = SlnxParserFixture.WithSolutionFile(slnxContent);
        var parser = new SolutionParser(fileSystem, environment);

        // Act
        var result = parser.Parse(slnxPath);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("17.0", result.VisualStudioVersion);
        Assert.Equal("17.0", result.MinimumVisualStudioVersion);
        Assert.Single(result.Projects);

        var project = result.Projects.First();
        Assert.Equal("ConsoleApp/ConsoleApp.csproj", project.Path.FullPath);
        Assert.Equal("ConsoleApp", project.Name);
        Assert.Equal("slnx", project.Type);
    }

    [Fact]
    public void Should_Throw_If_Slnx_File_Does_Not_Exist()
    {
        var fixture = new SlnxParserFixture();
        var nonExistentPath = new FilePath("/Working/doesnotexist.slnx");

        // Act & Assert
        var ex = Assert.Throws<CakeException>(() => SlnxParser.Parse(nonExistentPath, fixture.FileSystem));
        Assert.Equal("Solution file '/Working/doesnotexist.slnx' does not exist.", ex.Message);
    }
}