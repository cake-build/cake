// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build;
using Cake.Common.Build.AppVeyor;
using Cake.Common.Build.AzurePipelines;
using Cake.Common.Build.Bamboo;
using Cake.Common.Build.BitbucketPipelines;
using Cake.Common.Build.Bitrise;
using Cake.Common.Build.ContinuaCI;
using Cake.Common.Build.GitHubActions;
using Cake.Common.Build.GitLabCI;
using Cake.Common.Build.GoCD;
using Cake.Common.Build.Jenkins;
using Cake.Common.Build.MyGet;
using Cake.Common.Build.TeamCity;
using Cake.Common.Build.TravisCI;
using Cake.Common.Tests.Fixtures.Build;
using NSubstitute;
using Xunit;

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
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(null, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider));

                // Then
                AssertEx.IsArgumentNullException(result, "appVeyorProvider");
            }

            [Fact]
            public void Should_Throw_If_TeamCity_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, null, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider));

                // Then
                AssertEx.IsArgumentNullException(result, "teamCityProvider");
            }

            [Fact]
            public void Should_Throw_If_MyGet_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, null, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider));

                // Then
                AssertEx.IsArgumentNullException(result, "myGetProvider");
            }

            [Fact]
            public void Should_Throw_If_Bamboo_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, null, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider));

                // Then
                AssertEx.IsArgumentNullException(result, "bambooProvider");
            }

            [Fact]
            public void Should_Throw_If_ContinuaCI_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, null, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider));

                // Then
                AssertEx.IsArgumentNullException(result, "continuaCIProvider");
            }

            [Fact]
            public void Should_Throw_If_Jenkins_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, null, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider));

                // Then
                AssertEx.IsArgumentNullException(result, "jenkinsProvider");
            }

            [Fact]
            public void Should_Throw_If_Bitrise_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, null, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider));

                // Then
                AssertEx.IsArgumentNullException(result, "bitriseProvider");
            }

            [Fact]
            public void Should_Throw_If_TravisCI_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, null, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider));

                // Then
                AssertEx.IsArgumentNullException(result, "travisCIProvider");
            }

            [Fact]
            public void Should_Throw_If_BitbucketPipelines_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, null, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider));

                // Then
                AssertEx.IsArgumentNullException(result, "bitbucketPipelinesProvider");
            }

            [Fact]
            public void Should_Throw_If_GoCD_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, null, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider));

                // Then
                AssertEx.IsArgumentNullException(result, "goCDProvider");
            }

            [Fact]
            public void Should_Throw_If_GitLabCI_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, null, gitHubActionsProvider, azurePipelinesProvider));

                // Then
                AssertEx.IsArgumentNullException(result, "gitLabCIProvider");
            }

            [Fact]
            public void Should_Throw_If_AzurePipelines_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, null));

                // Then
                AssertEx.IsArgumentNullException(result, "azurePipelinesProvider");
            }

            [Fact]
            public void Should_Throw_If_GitHubActions_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, null, azurePipelinesProvider));

                // Then
                AssertEx.IsArgumentNullException(result, "gitHubActionsProvider");
            }
        }

        public sealed class TheIsRunningOnAppVeyorProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_AppVeyor()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var appVeyorEnvironment = new AppVeyorInfoFixture().CreateEnvironmentInfo();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(true);
                appVeyorProvider.Environment.Returns(appVeyorEnvironment);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);

                // Then
                Assert.True(buildSystem.IsRunningOnAppVeyor);
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
                var teamCityEnvironment = new TeamCityInfoFixture().CreateEnvironmentInfo();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                teamCityProvider.IsRunningOnTeamCity.Returns(true);
                teamCityProvider.Environment.Returns(teamCityEnvironment);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);

                // Then
                Assert.True(buildSystem.IsRunningOnTeamCity);
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
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                myGetProvider.IsRunningOnMyGet.Returns(true);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);

                // Then
                Assert.True(buildSystem.IsRunningOnMyGet);
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
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                bambooProvider.IsRunningOnBamboo.Returns(true);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);

                // Then
                Assert.True(buildSystem.IsRunningOnBamboo);
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
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                continuaCIProvider.IsRunningOnContinuaCI.Returns(true);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);

                // Then
                Assert.True(buildSystem.IsRunningOnContinuaCI);
            }
        }

        public sealed class TheIsRunningOnJenkinsProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_Jenkins()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var jenkinsEnvironment = new JenkinsInfoFixture().CreateEnvironmentInfo();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                jenkinsProvider.IsRunningOnJenkins.Returns(true);
                jenkinsProvider.Environment.Returns(jenkinsEnvironment);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);

                // Then
                Assert.True(buildSystem.IsRunningOnJenkins);
            }
        }

        public sealed class TheIsRunningOnBitriseProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_Bitrise()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var bitriseEnvironment = new BitriseInfoFixture().CreateEnvironmentInfo();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                bitriseProvider.IsRunningOnBitrise.Returns(true);
                bitriseProvider.Environment.Returns(bitriseEnvironment);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);

                // Then
                Assert.True(buildSystem.IsRunningOnBitrise);
            }
        }

        public sealed class TheIsRunningOnTravisCIProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_TravisCI()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var travisCIEnvironment = new TravisCIInfoFixture().CreateEnvironmentInfo();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                travisCIProvider.IsRunningOnTravisCI.Returns(true);
                travisCIProvider.Environment.Returns(travisCIEnvironment);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);

                // Then
                Assert.True(buildSystem.IsRunningOnTravisCI);
            }
        }

        public sealed class TheIsRunningOnBitbucketPipelinesProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_BitbucketPipelines()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var bitbucketPipelinesEnvironment = new BitbucketPipelinesInfoFixture().CreateEnvironmentInfo();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(true);
                bitbucketPipelinesProvider.Environment.Returns(bitbucketPipelinesEnvironment);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);

                // Then
                Assert.True(buildSystem.IsRunningOnBitbucketPipelines);
            }
        }

        public sealed class TheIsRunningOnGoCDProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_GoCD()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                goCDProvider.IsRunningOnGoCD.Returns(true);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);

                // Then
                Assert.True(buildSystem.IsRunningOnGoCD);
            }
        }

        public sealed class TheIsRunningOnGitLabCIProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_GitLabCI()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitLabCIEnvironment = new GitLabCIInfoFixture().CreateEnvironmentInfo();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();

                gitLabCIProvider.IsRunningOnGitLabCI.Returns(true);
                gitLabCIProvider.Environment.Returns(gitLabCIEnvironment);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);

                // Then
                Assert.True(buildSystem.IsRunningOnGitLabCI);
            }
        }

        public sealed class TheIsRunningOnAzurePipelinesProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_AzurePipelines()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();
                var azurePipelinesEnvironment = new AzurePipelinesInfoFixture().CreateEnvironmentInfo();

                azurePipelinesProvider.IsRunningOnAzurePipelines.Returns(true);
                azurePipelinesProvider.Environment.Returns(azurePipelinesEnvironment);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);

                // Then
                Assert.True(buildSystem.IsRunningOnAzurePipelines);
            }
        }

        public sealed class TheIsRunningOnGitHubActionsProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_GitHubActions()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var gitHubActionsEnvironment = new GitHubActionsInfoFixture().CreateEnvironmentInfo();

                gitHubActionsProvider.IsRunningOnGitHubActions.Returns(true);
                gitHubActionsProvider.Environment.Returns(gitHubActionsEnvironment);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);

                // Then
                Assert.True(buildSystem.IsRunningOnGitHubActions);
            }
        }

        public sealed class TheProviderProperty
        {
            [Theory]
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, false, false, BuildProvider.Local)]
            [InlineData(true, false, false, false, false, false, false, false, false, false, false, false, false, BuildProvider.AppVeyor)]
            [InlineData(false, true, false, false, false, false, false, false, false, false, false, false, false, BuildProvider.TeamCity)]
            [InlineData(false, false, true, false, false, false, false, false, false, false, false, false, false, BuildProvider.MyGet)]
            [InlineData(false, false, false, true, false, false, false, false, false, false, false, false, false, BuildProvider.Bamboo)]
            [InlineData(false, false, false, false, true, false, false, false, false, false, false, false, false, BuildProvider.ContinuaCI)]
            [InlineData(false, false, false, false, false, true, false, false, false, false, false, false, false, BuildProvider.Jenkins)]
            [InlineData(false, false, false, false, false, false, true, false, false, false, false, false, false, BuildProvider.Bitrise)]
            [InlineData(false, false, false, false, false, false, false, true, false, false, false, false, false, BuildProvider.TravisCI)]
            [InlineData(false, false, false, false, false, false, false, false, true, false, false, false, false, BuildProvider.BitbucketPipelines)]
            [InlineData(false, false, false, false, false, false, false, false, false, true, false, false, false, BuildProvider.GoCD)]
            [InlineData(false, false, false, false, false, false, false, false, false, false, true, false, false, BuildProvider.GitLabCI)]
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, true, false, BuildProvider.AzurePipelines)]
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, false, true, BuildProvider.GitHubActions)]
            public void Should_Return_Provider_If_Running_On_Provider(bool appVeyor, bool teamCity, bool myGet, bool bamboo, bool continuaCI, bool jenkins, bool bitrise, bool travisCI, bool bitbucketPipelines, bool goCD, bool gitLabCI, bool azurePipelines, bool gitHubActions, BuildProvider provider)
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var appVeyorEnvironment = new AppVeyorInfoFixture().CreateEnvironmentInfo();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var teamCityEnvironment = new TeamCityInfoFixture().CreateEnvironmentInfo();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var jenkinsEnvironment = new JenkinsInfoFixture().CreateEnvironmentInfo();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var bitriseEnvironment = new BitriseInfoFixture().CreateEnvironmentInfo();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var travisCIEnvironment = new TravisCIInfoFixture().CreateEnvironmentInfo();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var bitbucketPipelinesEnvironment = new BitbucketPipelinesInfoFixture().CreateEnvironmentInfo();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitLabCIEnvironment = new GitLabCIInfoFixture().CreateEnvironmentInfo();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var gitHubActionsEnvironment = new GitHubActionsInfoFixture().CreateEnvironmentInfo();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();
                var azurePipelinesEnvironment = new AzurePipelinesInfoFixture().CreateEnvironmentInfo();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(appVeyor);
                appVeyorProvider.Environment.Returns(appVeyorEnvironment);
                teamCityProvider.IsRunningOnTeamCity.Returns(teamCity);
                teamCityProvider.Environment.Returns(teamCityEnvironment);
                myGetProvider.IsRunningOnMyGet.Returns(myGet);
                bambooProvider.IsRunningOnBamboo.Returns(bamboo);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(continuaCI);
                jenkinsProvider.IsRunningOnJenkins.Returns(jenkins);
                jenkinsProvider.Environment.Returns(jenkinsEnvironment);
                bitriseProvider.IsRunningOnBitrise.Returns(bitrise);
                bitriseProvider.Environment.Returns(bitriseEnvironment);
                travisCIProvider.IsRunningOnTravisCI.Returns(travisCI);
                travisCIProvider.Environment.Returns(travisCIEnvironment);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(bitbucketPipelines);
                bitbucketPipelinesProvider.Environment.Returns(bitbucketPipelinesEnvironment);
                goCDProvider.IsRunningOnGoCD.Returns(goCD);
                gitLabCIProvider.IsRunningOnGitLabCI.Returns(gitLabCI);
                gitLabCIProvider.Environment.Returns(gitLabCIEnvironment);
                gitHubActionsProvider.IsRunningOnGitHubActions.Returns(gitHubActions);
                gitHubActionsProvider.Environment.Returns(gitHubActionsEnvironment);
                azurePipelinesProvider.IsRunningOnAzurePipelines.Returns(azurePipelines);
                azurePipelinesProvider.Environment.Returns(azurePipelinesEnvironment);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);

                // Then
                Assert.Equal(provider, buildSystem.Provider);
            }
        }

        public sealed class TheIsLocalBuildProperty
        {
            [Theory]
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, false, false, true)]
            [InlineData(true, false, false, false, false, false, false, false, false, false, false, false, false, false)]
            [InlineData(false, true, false, false, false, false, false, false, false, false, false, false, false, false)]
            [InlineData(false, false, true, false, false, false, false, false, false, false, false, false, false, false)]
            [InlineData(false, false, false, true, false, false, false, false, false, false, false, false, false, false)]
            [InlineData(false, false, false, false, true, false, false, false, false, false, false, false, false, false)]
            [InlineData(false, false, false, false, false, true, false, false, false, false, false, false, false, false)]
            [InlineData(false, false, false, false, false, false, true, false, false, false, false, false, false, false)]
            [InlineData(false, false, false, false, false, false, false, true, false, false, false, false, false, false)]
            [InlineData(false, false, false, false, false, false, false, false, true, false, false, false, false, false)]
            [InlineData(false, false, false, false, false, false, false, false, false, true, false, false, false, false)]
            [InlineData(false, false, false, false, false, false, false, false, false, false, true, false, false, false)]
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, true, false, false)]
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, false, true, false)]
            public void Should_Return_Whether_Or_Not_Running_On_Provider(bool appVeyor, bool teamCity, bool myGet, bool bamboo, bool continuaCI, bool jenkins, bool bitrise, bool travisCI, bool bitbucketPipelines, bool goCD, bool gitLabCI, bool azurePipelines, bool gitHubActions, bool isLocalBuild)
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var appVeyorEnvironment = new AppVeyorInfoFixture().CreateEnvironmentInfo();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var teamCityEnvironment = new TeamCityInfoFixture().CreateEnvironmentInfo();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var jenkinsEnvironment = new JenkinsInfoFixture().CreateEnvironmentInfo();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var bitriseEnvironment = new BitriseInfoFixture().CreateEnvironmentInfo();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var travisCIEnvironment = new TravisCIInfoFixture().CreateEnvironmentInfo();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var bitbucketPipelinesEnvironment = new BitbucketPipelinesInfoFixture().CreateEnvironmentInfo();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitLabCIEnvironment = new GitLabCIInfoFixture().CreateEnvironmentInfo();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var gitHubActionsEnvironment = new GitHubActionsInfoFixture().CreateEnvironmentInfo();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();
                var azurePipelinesEnvironment = new AzurePipelinesInfoFixture().CreateEnvironmentInfo();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(appVeyor);
                appVeyorProvider.Environment.Returns(appVeyorEnvironment);
                teamCityProvider.IsRunningOnTeamCity.Returns(teamCity);
                teamCityProvider.Environment.Returns(teamCityEnvironment);
                myGetProvider.IsRunningOnMyGet.Returns(myGet);
                bambooProvider.IsRunningOnBamboo.Returns(bamboo);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(continuaCI);
                jenkinsProvider.IsRunningOnJenkins.Returns(jenkins);
                jenkinsProvider.Environment.Returns(jenkinsEnvironment);
                bitriseProvider.IsRunningOnBitrise.Returns(bitrise);
                bitriseProvider.Environment.Returns(bitriseEnvironment);
                travisCIProvider.IsRunningOnTravisCI.Returns(travisCI);
                travisCIProvider.Environment.Returns(travisCIEnvironment);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(bitbucketPipelines);
                bitbucketPipelinesProvider.Environment.Returns(bitbucketPipelinesEnvironment);
                goCDProvider.IsRunningOnGoCD.Returns(goCD);
                gitLabCIProvider.IsRunningOnGitLabCI.Returns(gitLabCI);
                gitLabCIProvider.Environment.Returns(gitLabCIEnvironment);
                gitHubActionsProvider.IsRunningOnGitHubActions.Returns(gitHubActions);
                gitHubActionsProvider.Environment.Returns(gitHubActionsEnvironment);
                azurePipelinesProvider.IsRunningOnAzurePipelines.Returns(azurePipelines);
                azurePipelinesProvider.Environment.Returns(azurePipelinesEnvironment);

                // When
                System.Console.WriteLine(jenkinsProvider);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);

                // Then
                Assert.Equal(isLocalBuild, buildSystem.IsLocalBuild);
            }
        }

        public sealed class TheIsPullRequestProperty
        {
            [Theory]
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, false, false, false)] // none
            [InlineData(true, false, false, false, false, false, false, false, false, false, false, false, false, true)] // appveyor
            [InlineData(false, true, false, false, false, false, false, false, false, false, false, false, false, true)] // teamcity
            [InlineData(false, false, true, false, false, false, false, false, false, false, false, false, false, false)] // myget
            [InlineData(false, false, false, true, false, false, false, false, false, false, false, false, false, false)] // bamboo
            [InlineData(false, false, false, false, true, false, false, false, false, false, false, false, false, false)] // continua
            [InlineData(false, false, false, false, false, true, false, false, false, false, false, false, false, true)] // jenkins
            [InlineData(false, false, false, false, false, false, true, false, false, false, false, false, false, true)] // bitrise
            [InlineData(false, false, false, false, false, false, false, true, false, false, false, false, false, true)] // travis
            [InlineData(false, false, false, false, false, false, false, false, true, false, false, false, false, true)] // bitbucket
            [InlineData(false, false, false, false, false, false, false, false, false, true, false, false, false, false)] // gocd
            [InlineData(false, false, false, false, false, false, false, false, false, false, true, false, false, true)] // gitlab
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, true, false, true)] // az pipelines
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, false, true, true)] // gh actions
            public void Should_Return_True_If_Running_On_Supported_Provider(bool appVeyor, bool teamCity, bool myGet, bool bamboo, bool continuaCI, bool jenkins, bool bitrise, bool travisCI, bool bitbucketPipelines, bool goCD, bool gitLabCI, bool azurePipelines, bool gitHubActions, bool isPullRequest)
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var appVeyorEnvironment = new AppVeyorInfoFixture().CreateEnvironmentInfo();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                var teamCityEnvironment = new TeamCityInfoFixture().CreateEnvironmentInfo();
                var myGetProvider = Substitute.For<IMyGetProvider>();
                var bambooProvider = Substitute.For<IBambooProvider>();
                var continuaCIProvider = Substitute.For<IContinuaCIProvider>();
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var jenkinsEnvironment = new JenkinsInfoFixture().CreateEnvironmentInfo();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var bitriseEnvironment = new BitriseInfoFixture().CreateEnvironmentInfo();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var travisCIEnvironment = new TravisCIInfoFixture().CreateEnvironmentInfo();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var bitbucketPipelinesEnvironment = new BitbucketPipelinesInfoFixture().CreateEnvironmentInfo();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitLabCIProvider = Substitute.For<IGitLabCIProvider>();
                var gitLabCIEnvironment = new GitLabCIInfoFixture().CreateEnvironmentInfo();
                var gitHubActionsProvider = Substitute.For<IGitHubActionsProvider>();
                var gitHubActionsEnvironment = new GitHubActionsInfoFixture().CreateEnvironmentInfo();
                var azurePipelinesProvider = Substitute.For<IAzurePipelinesProvider>();
                var azurePipelinesEnvironment = new AzurePipelinesInfoFixture().CreateEnvironmentInfo();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(appVeyor);
                appVeyorProvider.Environment.Returns(appVeyorEnvironment);
                teamCityProvider.IsRunningOnTeamCity.Returns(teamCity);
                teamCityProvider.Environment.Returns(teamCityEnvironment);
                myGetProvider.IsRunningOnMyGet.Returns(myGet);
                bambooProvider.IsRunningOnBamboo.Returns(bamboo);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(continuaCI);
                jenkinsProvider.IsRunningOnJenkins.Returns(jenkins);
                jenkinsProvider.Environment.Returns(jenkinsEnvironment);
                bitriseProvider.IsRunningOnBitrise.Returns(bitrise);
                bitriseProvider.Environment.Returns(bitriseEnvironment);
                travisCIProvider.IsRunningOnTravisCI.Returns(travisCI);
                travisCIProvider.Environment.Returns(travisCIEnvironment);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(bitbucketPipelines);
                bitbucketPipelinesProvider.Environment.Returns(bitbucketPipelinesEnvironment);
                goCDProvider.IsRunningOnGoCD.Returns(goCD);
                gitLabCIProvider.IsRunningOnGitLabCI.Returns(gitLabCI);
                gitLabCIProvider.Environment.Returns(gitLabCIEnvironment);
                gitHubActionsProvider.IsRunningOnGitHubActions.Returns(gitHubActions);
                gitHubActionsProvider.Environment.Returns(gitHubActionsEnvironment);
                azurePipelinesProvider.IsRunningOnAzurePipelines.Returns(azurePipelines);
                azurePipelinesProvider.Environment.Returns(azurePipelinesEnvironment);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);

                // Then
                Assert.Equal(isPullRequest, buildSystem.IsPullRequest);
            }
        }
    }
}