using Cake.Common.Tests.Fixtures;
using Cake.Core;
using System;
using Cake.Core.IO;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

namespace Cake.Common.Tests.Unit
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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("fileSystem", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();

                // When
                var result = Record.Exception(() => new AssemblyInfoParser(fileSystem, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("environment", ((ArgumentNullException)result).ParamName);
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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("assemblyInfoPath", ((ArgumentNullException)result).ParamName);
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
                Assert.Equal("Assembly info file '/Working/output.cs' do not exist.", result.Message);
            }

            [Theory]
            [InlineData("1.2.3.4", "1.2.3.4")]
            [InlineData("1.2.*.*", "1.2.*.*")]
            [InlineData(null, "1.0.0.0")]
            public void Should_Read_AssemblyVersion(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture(value);                

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.AssemblyVersion);
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
        }
    }
}
