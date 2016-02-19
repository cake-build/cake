﻿using Cake.Common.Build;
using Cake.Common.Build.AppVeyor;
using Cake.Common.Build.MyGet;
using Cake.Common.Build.TeamCity;
using Cake.Common.Build.Bamboo;
using NSubstitute;
using Xunit;
using Cake.Common.Build.ContinuaCI;

namespace Cake.Common.Tests.Unit.Build
{
    public sealed class BuildSystemTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_AppVeyor_Is_Null()
            {
                // Given
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(null, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider));

                // Then
                Assert.IsArgumentNullException(result, "appVeyorProvider");
            }

            [Fact]
            public void Should_Throw_If_TeamCity_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, null, myGetProvider, bambooProvider, continuaCIProvider));

                // Then
                Assert.IsArgumentNullException(result, "teamCityProvider");
            }

            [Fact]
            public void Should_Throw_If_MyGet_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, null, bambooProvider, continuaCIProvider));

                // Then
                Assert.IsArgumentNullException(result, "myGetProvider");
            }

            [Fact]
            public void Should_Throw_If_Bamboo_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, null, continuaCIProvider));

                // Then
                Assert.IsArgumentNullException(result, "bambooProvider");

            }

            [Fact]
            public void Should_Throw_If_ContinuaCI_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, null));

                // Then
                Assert.IsArgumentNullException(result, "continuaCIProvider");

            }
        }

        public sealed class TheIsRunningOnAppVeyorProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_AppVeyor()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider);

                // When
                var result = buildSystem.IsRunningOnAppVeyor;

                // Then
                Assert.True(result);
            }
        }
        public sealed class TheIsRunningOnTeamCityProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_TeamCity()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();

                teamCityProvider.IsRunningOnTeamCity.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider);

                // When
                var result = buildSystem.IsRunningOnTeamCity;

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheIsRunningOnMyGetProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_MyGet()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();

                myGetProvider.IsRunningOnMyGet.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider);

                // When
                var result = buildSystem.IsRunningOnMyGet;

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheIsRunningOnBambooProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_Bamboo()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();

                bambooProvider.IsRunningOnBamboo.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider);

                // When
                var result = buildSystem.IsRunningOnBamboo;

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheIsRunningOnContinuaCIProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_ContinuaCI()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();

                continuaCIProvider.IsRunningOnContinuaCI.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider);

                // When
                var result = buildSystem.IsRunningOnContinuaCI;

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
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(true);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_If_Running_On_TeamCity()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(true);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_If_Running_On_MyGet()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(true);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.False(result);
            }
            [Fact]
            public void Should_Return_False_If_Running_On_Bamboo()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(true);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_If_Running_On_ContinuaCI()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.False(result);
            }


            [Fact]
            public void Should_Return_True_If_Not_Running_On_Any()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.True(result);
            }
        }
    }
}