// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build;
using Cake.Common.Build.AppVeyor;
using Cake.Common.Build.Bamboo;
using Cake.Common.Build.BitbucketPipelines;
using Cake.Common.Build.Bitrise;
using Cake.Common.Build.ContinuaCI;
using Cake.Common.Build.GitLabCI;
using Cake.Common.Build.GoCD;
using Cake.Common.Build.Jenkins;
using Cake.Common.Build.MyGet;
using Cake.Common.Build.TeamCity;
using Cake.Common.Build.TFBuild;
using Cake.Common.Build.TravisCI;
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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(null, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider));

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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, null, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider));

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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, null, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider));

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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, null, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider));

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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, null, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider));

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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, null, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider));

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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, null, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider));

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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, null, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider));

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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, null, goCDProvider, gitlabCIProvider, tfBuildProvider));

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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, null, gitlabCIProvider, tfBuildProvider));

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
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, null, tfBuildProvider));

                // Then
                AssertEx.IsArgumentNullException(result, "gitlabCIProvider");
            }

            [Fact]
            public void Should_Throw_If_TFBuild_Is_Null()
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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, null));

                // Then
                AssertEx.IsArgumentNullException(result, "tfBuildProvider");
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
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                teamCityProvider.IsRunningOnTeamCity.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                myGetProvider.IsRunningOnMyGet.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                bambooProvider.IsRunningOnBamboo.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                continuaCIProvider.IsRunningOnContinuaCI.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsRunningOnContinuaCI;

                // Then
                Assert.True(result);
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
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                jenkinsProvider.IsRunningOnJenkins.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsRunningOnJenkins;

                // Then
                Assert.True(result);
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
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                bitriseProvider.IsRunningOnBitrise.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsRunningOnBitrise;

                // Then
                Assert.True(result);
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
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                travisCIProvider.IsRunningOnTravisCI.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsRunningOnTravisCI;

                // Then
                Assert.True(result);
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
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsRunningOnBitbucketPipelines;

                // Then
                Assert.True(result);
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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                goCDProvider.IsRunningOnGoCD.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsRunningOnGoCD;

                // Then
                Assert.True(result);
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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                gitlabCIProvider.IsRunningOnGitLabCI.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsRunningOnGitLabCI;

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheIsRunningOnTFSProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_TFS()
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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

#pragma warning disable 618
                tfBuildProvider.IsRunningOnTFS.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsRunningOnTFS;
#pragma warning restore 618

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheIsRunningOnVSTSProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_VSTS()
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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

#pragma warning disable 618
                tfBuildProvider.IsRunningOnVSTS.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsRunningOnVSTS;
#pragma warning restore 618

                // Then
                Assert.True(result);
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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                tfBuildProvider.IsRunningOnAzurePipelines.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsRunningOnAzurePipelines;

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheIsRunningOnAzurePipelinesHostedProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_AzurePipelinesHosted()
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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                tfBuildProvider.IsRunningOnAzurePipelinesHosted.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsRunningOnAzurePipelinesHosted;

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheProviderProperty
        {
            [Theory]
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, BuildProvider.Local)]
            [InlineData(true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, BuildProvider.AppVeyor)]
            [InlineData(false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, BuildProvider.TeamCity)]
            [InlineData(false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, BuildProvider.MyGet)]
            [InlineData(false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, BuildProvider.Bamboo)]
            [InlineData(false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, BuildProvider.ContinuaCI)]
            [InlineData(false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, BuildProvider.Jenkins)]
            [InlineData(false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, BuildProvider.Bitrise)]
            [InlineData(false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, BuildProvider.TravisCI)]
            [InlineData(false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, BuildProvider.BitbucketPipelines)]
            [InlineData(false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, BuildProvider.GoCD)]
            [InlineData(false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, BuildProvider.GitLabCI)]
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, BuildProvider.AzurePipelines)] // tfs
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, BuildProvider.AzurePipelinesHosted)] // vsts
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, BuildProvider.AzurePipelines)]
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, true, false, false, true, BuildProvider.AzurePipelinesHosted)]
            public void Should_Return_Provider_If_Running_On_Provider(bool appVeyor, bool teamCity, bool myGet, bool bamboo, bool continuaCI, bool jenkins, bool bitrise, bool travisCI, bool bitbucketPipelines, bool goCD, bool gitlabCI, bool tfs, bool vsts, bool azurePipelines, bool azurePipelinesHosted, BuildProvider provider)
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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(appVeyor);
                teamCityProvider.IsRunningOnTeamCity.Returns(teamCity);
                myGetProvider.IsRunningOnMyGet.Returns(myGet);
                bambooProvider.IsRunningOnBamboo.Returns(bamboo);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(continuaCI);
                jenkinsProvider.IsRunningOnJenkins.Returns(jenkins);
                bitriseProvider.IsRunningOnBitrise.Returns(bitrise);
                travisCIProvider.IsRunningOnTravisCI.Returns(travisCI);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(bitbucketPipelines);
                goCDProvider.IsRunningOnGoCD.Returns(goCD);
                gitlabCIProvider.IsRunningOnGitLabCI.Returns(gitlabCI);
#pragma warning disable 618
                tfBuildProvider.IsRunningOnTFS.Returns(tfs);
                tfBuildProvider.IsRunningOnVSTS.Returns(vsts);
#pragma warning restore 618
                tfBuildProvider.IsRunningOnAzurePipelines.Returns(azurePipelines);
                tfBuildProvider.IsRunningOnAzurePipelinesHosted.Returns(azurePipelinesHosted);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // Then
                Assert.Equal(provider, buildSystem.Provider);
            }
        }

        public sealed class TheIsLocalBuildProperty
        {
            [Theory]
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true)]
            [InlineData(true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
            [InlineData(false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
            [InlineData(false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false)]
            [InlineData(false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false)]
            [InlineData(false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false)]
            [InlineData(false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false)]
            [InlineData(false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false)]
            [InlineData(false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false)]
            [InlineData(false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false)]
            [InlineData(false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false)]
            [InlineData(false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false)]
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false)] // tfs
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false)] // vsts
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false)]
            [InlineData(false, false, false, false, false, false, false, false, false, false, false, true, false, false, true, false)]
            public void Should_Return_False_If_Running_On_Provider(bool appVeyor, bool teamCity, bool myGet, bool bamboo, bool continuaCI, bool jenkins, bool bitrise, bool travisCI, bool bitbucketPipelines, bool goCD, bool gitlabCI, bool tfs, bool vsts, bool azurePipelines, bool azurePipelinesHosted, bool isLocalBuild)
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
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(appVeyor);
                teamCityProvider.IsRunningOnTeamCity.Returns(teamCity);
                myGetProvider.IsRunningOnMyGet.Returns(myGet);
                bambooProvider.IsRunningOnBamboo.Returns(bamboo);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(continuaCI);
                jenkinsProvider.IsRunningOnJenkins.Returns(jenkins);
                bitriseProvider.IsRunningOnBitrise.Returns(bitrise);
                travisCIProvider.IsRunningOnTravisCI.Returns(travisCI);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(bitbucketPipelines);
                goCDProvider.IsRunningOnGoCD.Returns(goCD);
                gitlabCIProvider.IsRunningOnGitLabCI.Returns(gitlabCI);
#pragma warning disable 618
                tfBuildProvider.IsRunningOnTFS.Returns(tfs);
                tfBuildProvider.IsRunningOnVSTS.Returns(vsts);
#pragma warning restore 618
                tfBuildProvider.IsRunningOnAzurePipelines.Returns(azurePipelines);
                tfBuildProvider.IsRunningOnAzurePipelinesHosted.Returns(azurePipelinesHosted);

                // When
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // Then
                Assert.Equal(isLocalBuild, buildSystem.IsLocalBuild);
            }
        }
    }
}