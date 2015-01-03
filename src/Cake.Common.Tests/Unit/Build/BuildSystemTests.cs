using Cake.Common.Build;
using Cake.Common.Build.AppVeyor;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build
{
    public sealed class BuildSystemTests
    {
        public sealed class TheIsRunningOnAppVeyorProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_AppVeyor()
            {
                // Given
                var appVeyor = Substitute.For<IAppVeyorProvider>();
                appVeyor.IsRunningOnAppVeyor.Returns(true);
                var buildSystem = new BuildSystem(appVeyor);

                // When
                var result = buildSystem.IsRunningOnAppVeyor;

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheIsLocalBuildProperty
        {
            [Fact]
            public void Should_Return_False_If_Running_On_AppVeyor()
            {
                // Given
                var appVeyor = Substitute.For<IAppVeyorProvider>();
                appVeyor.IsRunningOnAppVeyor.Returns(true);
                var buildSystem = new BuildSystem(appVeyor);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_True_If_Not_Running_On_AppVeyor()
            {
                // Given
                var appVeyor = Substitute.For<IAppVeyorProvider>();
                appVeyor.IsRunningOnAppVeyor.Returns(false);
                var buildSystem = new BuildSystem(appVeyor);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.True(result);
            }
        }
    }
}
