using Cake.Common.Solution.Project.Properties;
using Cake.Common.Tests.Fixtures;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Solution.Project.Properties
{
    public sealed class AssemblyInfoParserTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();

                // When
                var result = Record.Exception(() => new AssemblyInfoParser(null, environment));

                // Then
                Assert.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();

                // When
                var result = Record.Exception(() => new AssemblyInfoParser(fileSystem, null));

                // Then
                Assert.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheParseMethod
        {
            [Fact]
            public void Should_Throw_If_AssemblyInfo_Path_Is_Null()
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();

                // When
                var result = Record.Exception(() => fixture.Parse(null));

                // Then
                Assert.IsArgumentNullException(result, "assemblyInfoPath");
            }

            [Fact]
            public void Should_Throw_If_AssemblyInfo_File_Do_Not_Exist()
            {
                // Given
                var fixture = new AssemblyInfoParserFixture(createAssemblyInfo: false);

                // When
                var result = Record.Exception(() => fixture.Parse());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Assembly info file '/Working/output.cs' does not exist.", result.Message);
            }

            [Theory]
            [InlineData(true, true)]
            [InlineData(false, false)]
            [InlineData(null, false)]
            public void Should_Read_ClsCompliance(bool value, bool expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture(clsCompliant: value);

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.ClsCompliant);
            }

            [Theory]
            [InlineData("CompanyA", "CompanyA")]
            [InlineData(null, "")]
            public void Should_Read_Company(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture(company: value);

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.Company);
            }

            [Theory]
            [InlineData(true, true)]
            [InlineData(false, false)]
            [InlineData(null, false)]
            public void Should_Read_ComVisible(bool value, bool expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture(comVisible: value);

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.ComVisible);
            }

            [Theory]
            [InlineData("Debug", "Debug")]
            [InlineData("Release", "Release")]
            [InlineData(null, "")]
            public void Should_Read_Configuration(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture(configuration: value);

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.Configuration);
            }

            [Theory]
            [InlineData("Copyright (c) Patrik Svensson, Mattias Karlsson and contributors", "Copyright (c) Patrik Svensson, Mattias Karlsson and contributors")]
            [InlineData(null, "")]
            public void Should_Read_Copyright(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture(copyright: value);

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.Copyright);
            }

            [Theory]
            [InlineData("Assembly Description", "Assembly Description")]
            [InlineData(null, "")]
            public void Should_Read_Description(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture(description: value);

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.Description);
            }

            [Theory]
            [InlineData("1.2.3.4", "1.2.3.4")]
            [InlineData("1.2.*.*", "1.2.*.*")]
            [InlineData(null, "1.0.0.0")]
            public void Should_Read_AssemblyFileVersion(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture(fileVersion: value);

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.AssemblyFileVersion);
            }

            [Theory]
            [InlineData("D394B7DB-0DDC-4D11-AD69-C408212E1E80", "D394B7DB-0DDC-4D11-AD69-C408212E1E80")]
            [InlineData(null, "")]
            public void Should_Read_Guid(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture(guid: value);

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.Guid);
            }

            [Theory]
            [InlineData("1.2.3.4", "1.2.3.4")]
            [InlineData("1.2.*.*", "1.2.*.*")]
            [InlineData(null, "1.0.0.0")]
            public void Should_Read_AssemblyInformationalVersion(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture(informationalVersion: value);

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.AssemblyInformationalVersion);
            }

            [Theory]
            [InlineData("Cake.Common.Tests", "Cake.Common.Tests")]
            [InlineData(null, "")]
            public void Should_Read_InternalsVisibleTo(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture(internalsVisibleTo: value);

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.InternalsVisibileTo);
            }

            [Theory]
            [InlineData("Cake", "Cake")]
            [InlineData(null, "")]
            public void Should_Read_Product(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture(product: value);

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.Product);
            }

            [Theory]
            [InlineData("Cake.Common", "Cake.Common")]
            [InlineData(null, "")]
            public void Should_Read_Title(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture(title: value);

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.Title);
            }

            [Theory]
            [InlineData("Trademark Cake", "Trademark Cake")]
            [InlineData(null, "")]
            public void Should_Read_Trademark(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture(trademark: value);

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.Trademark);
            }

            [Theory]
            [InlineData("1.2.3.4", "1.2.3.4")]
            [InlineData("1.2.*.*", "1.2.*.*")]
            [InlineData(null, "1.0.0.0")]
            public void Should_Read_AssemblyVersion(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture(version: value);

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.AssemblyVersion);
            }
        }
    }
}