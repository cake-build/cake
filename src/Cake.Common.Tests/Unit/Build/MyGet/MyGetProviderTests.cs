using Cake.Common.Build.MyGet;
using Cake.Common.Tests.Fakes;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.MyGet
{
    public sealed class MyGetProviderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var writer = new FakeBuildSystemServiceMessageWriter();
                var result = Record.Exception(() => new MyGetProvider(null, writer));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Writer_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new MyGetProvider(new FakeEnvironment(PlatformFamily.Unknown), null));

                // Then
                AssertEx.IsArgumentNullException(result, "writer");
            }
        }

        public sealed class IsRunningOnMyGet
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            [InlineData(null)]
            public void Should_Return_True_If_Running_On_MyGet(bool? capitalCase)
            {
                // Given
                var fixture = new MyGetFixture();
                fixture.IsRunningOnMyGet(capitalCase);
                var provider = fixture.CreateMyGetProvider();

                // When
                var result = provider.IsRunningOnMyGet;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_MyGet()
            {
                // Given
                var fixture = new MyGetFixture();
                var provider = fixture.CreateMyGetProvider();

                // When
                var result = provider.IsRunningOnMyGet;

                // Then
                Assert.False(result);
            }
        }

        public sealed class BuildProblem
        {
            [Theory]
            [InlineData("Test build problem", "Test build problem")]
            [InlineData("", "")]
            [InlineData(null, "")]
            [InlineData("[Special characters:\r\n\"\'test|split\'\"]", "|[Special characters:|r|n\"|\'test||split|\'\"|]")]
            public void Should_Log_Description(string description, string expectedOutput)
            {
                // Given
                var fixture = new MyGetFixture();
                var provider = fixture.CreateMyGetProvider();

                // When
                provider.BuildProblem(description);

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal($"##myget[buildProblem description='{expectedOutput}']", entry);
            }
        }

        public sealed class SetParameter
        {
            [Theory]
            [InlineData("Parameter", "Value", "Parameter", "Value")]
            [InlineData("", "", "", "")]
            [InlineData(null, null, "", "")]
            [InlineData("Special [param] \'name\'", "test\n|\r||value||", "Special |[param|] |\'name|\'", "test|n|||r||||value||||")]
            public void Should_Log_Parameter_Value(string name, string value, string expectedName, string expectedValue)
            {
                // Given
                var fixture = new MyGetFixture();
                var provider = fixture.CreateMyGetProvider();

                // When
                provider.SetParameter(name, value);

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal($"##myget[setParameter name='{expectedName}' value='{expectedValue}']", entry);
            }
        }

        public sealed class WriteStatus
        {
            [Theory]
            [InlineData("M", MyGetBuildStatus.Normal, "M", "NORMAL")]
            [InlineData(null, MyGetBuildStatus.Warning, "", "WARNING")]
            [InlineData("Message \n text", MyGetBuildStatus.Error, "Message |n text", "ERROR")]
            [InlineData("[Failure]|status", MyGetBuildStatus.Failure, "|[Failure|]||status", "FAILURE")]
            public void Should_Log_Status(string message, MyGetBuildStatus status, string expectedMessage, string expectedStatus)
            {
                // Given
                var fixture = new MyGetFixture();
                var provider = fixture.CreateMyGetProvider();

                // When
                provider.WriteStatus(message, status);

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal($"##myget[message text='{expectedMessage}' status='{expectedStatus}']", entry);
            }

            [Theory]
            [InlineData("Hello, World!", MyGetBuildStatus.Normal, "", "Hello, World!", "NORMAL", "")]
            [InlineData("My custom message", MyGetBuildStatus.Warning, "Hello, World!", "My custom message", "WARNING", "Hello, World!")]
            [InlineData("Test", MyGetBuildStatus.Error, "r = (a - b) * c + (s1 & s2)", "Test", "ERROR", "r = (a - b) * c + (s1 & s2)")]
            [InlineData("T", MyGetBuildStatus.Failure, "i = (b << 4) | c;\r\nr = a[i] / c;", "T", "FAILURE", "i = (b << 4) || c;|r|nr = a|[i|] / c;")]
            public void Should_Log_Status_With_Error_Details(string message, MyGetBuildStatus status, string errorDetails, string expectedMessage, string expectedStatus, string expectedDetails)
            {
                // Given
                var fixture = new MyGetFixture();
                var provider = fixture.CreateMyGetProvider();

                // When
                provider.WriteStatus(message, status, errorDetails);

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal($"##myget[message text='{expectedMessage}' status='{expectedStatus}' errorDetails='{expectedDetails}']", entry);
            }
        }

        public sealed class SetBuildNumber
        {
            [Theory]
            [InlineData("2.3.1", "2.3.1")]
            [InlineData("", "")]
            [InlineData(null, "")]
            [InlineData("99.4-beta", "99.4-beta")]
            [InlineData("[net5.0\r\n\"\'beta|preview\'\"]", "|[net5.0|r|n\"|\'beta||preview|\'\"|]")]
            public void Should_Log_Build_Number(string buildNumber, string expectedOutput)
            {
                // Given
                var fixture = new MyGetFixture();
                var provider = fixture.CreateMyGetProvider();

                // When
                provider.SetBuildNumber(buildNumber);

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal($"##myget[buildNumber '{expectedOutput}']", entry);
            }
        }
    }
}
