using System.IO;

using Cake.Common.Tests.Fixtures;

using Xunit;

namespace Cake.Common.Tests.Unit.XML
{
    public sealed class XmlPokeTests
    {
        public sealed class ValidateParameters
        {
            [Fact]
            public void Should_Throw_If_FilePath_Is_Null()
            {
                // Given
                var fixture = new XmlPokeFixture(false);

                // When
                var result = Record.Exception(() => fixture.Poke("gibblygook", ""));

                // Then
                Assert.IsArgumentNullException(result, "filePath");
            }

            [Fact]
            public void Should_Throw_If_File_Doesnt_Exists()
            {
                // Given
                var fixture = new XmlPokeFixture(false);
                fixture.XmlPath = "/Working/web.config";

                // When
                var result = Record.Exception(() => fixture.Poke("gibblygook", ""));

                // Then
                Assert.IsType<FileNotFoundException>(result);
            }

            [Fact]
            public void Should_Throw_If_No_Xpath()
            {
                // Given
                var fixture = new XmlPokeFixture();

                // When
                var result = Record.Exception(() => fixture.Poke(null, ""));

                // Then
                Assert.IsArgumentNullException(result, "xpath");
            }
        }

        public sealed class Transform
        {
            [Fact]
            public void Should_Change_Attribute()
            {
                // Given
                var fixture = new XmlPokeFixture();

                // When
                fixture.Poke("/configuration/appSettings/add[@key = 'server']/@value", "productionhost.somecompany.com");

                // Then
                fixture.TestIsValue(
                    "/configuration/appSettings/add[@key = 'server']/@value",
                    "productionhost.somecompany.com");
            }

            [Fact]
            public void Should_Remove_Attribute()
            {
                // Given
                var fixture = new XmlPokeFixture();

                // When
                fixture.Poke("/configuration/appSettings/add[@key = 'server']", null);

                // Then
                fixture.TestIsRemoved(
                    "/configuration/appSettings/add[@key = 'server']");
            }
        }
    }
}
