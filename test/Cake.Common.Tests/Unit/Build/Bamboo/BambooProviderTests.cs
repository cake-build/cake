using Cake.Common.Build.Bamboo;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Bamboo
{
    public sealed class BambooProviderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new BambooProvider(null));

                // Then
                Assert.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheIsRunningOnBambooProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_Bamboo()
            {
                // Given
                var fixture = new BambooFixture();
                fixture.IsRunningOnBamboo();
                var Bamboo = fixture.CreateBambooService();

                // When
                var result = Bamboo.IsRunningOnBamboo;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_Bamboo()
            {
                // Given
                var fixture = new BambooFixture();
                var Bamboo = fixture.CreateBambooService();

                // When
                var result = Bamboo.IsRunningOnBamboo;

                // Then
                Assert.False(result);
            }
        }

        public sealed class TheEnvironmentProperty
        {
            [Fact]
            public void Should_Return_Non_Null_Reference()
            {
                // Given
                var fixture = new BambooFixture();
                var Bamboo = fixture.CreateBambooService();

                // When
                var result = Bamboo.Environment;

                // Then
                Assert.NotNull(result);
            }
        }
    }
}