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
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, null, myGetProvider, bambooProvider, continuaCIProvider,  jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider));

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
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, null, bambooProvider, continuaCIProvider,  jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider));

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
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, null, continuaCIProvider,  jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider));

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
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, null,  jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider));

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
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider,  jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider,  jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider,  jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider,  jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider,  jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider,  jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider,  jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider,  jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider,  jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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

                tfBuildProvider.IsRunningOnVSTS.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsRunningOnVSTS;

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

                tfBuildProvider.IsRunningOnTFS.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsRunningOnTFS;

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
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(true);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                jenkinsProvider.IsRunningOnJenkins.Returns(false);
                bitriseProvider.IsRunningOnBitrise.Returns(false);
                travisCIProvider.IsRunningOnTravisCI.Returns(false);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(false);
                gitlabCIProvider.IsRunningOnGitLabCI.Returns(false);
                tfBuildProvider.IsRunningOnVSTS.Returns(false);
                tfBuildProvider.IsRunningOnTFS.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider,  jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(true);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                jenkinsProvider.IsRunningOnJenkins.Returns(false);
                bitriseProvider.IsRunningOnBitrise.Returns(false);
                travisCIProvider.IsRunningOnTravisCI.Returns(false);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(false);
                gitlabCIProvider.IsRunningOnGitLabCI.Returns(false);
                tfBuildProvider.IsRunningOnVSTS.Returns(false);
                tfBuildProvider.IsRunningOnTFS.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(true);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                jenkinsProvider.IsRunningOnJenkins.Returns(false);
                bitriseProvider.IsRunningOnBitrise.Returns(false);
                travisCIProvider.IsRunningOnTravisCI.Returns(false);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(false);
                gitlabCIProvider.IsRunningOnGitLabCI.Returns(false);
                tfBuildProvider.IsRunningOnVSTS.Returns(false);
                tfBuildProvider.IsRunningOnTFS.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(true);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                jenkinsProvider.IsRunningOnJenkins.Returns(false);
                bitriseProvider.IsRunningOnBitrise.Returns(false);
                travisCIProvider.IsRunningOnTravisCI.Returns(false);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(false);
                gitlabCIProvider.IsRunningOnGitLabCI.Returns(false);
                tfBuildProvider.IsRunningOnVSTS.Returns(false);
                tfBuildProvider.IsRunningOnTFS.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(true);
                jenkinsProvider.IsRunningOnJenkins.Returns(false);
                bitriseProvider.IsRunningOnBitrise.Returns(false);
                travisCIProvider.IsRunningOnTravisCI.Returns(false);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(false);
                gitlabCIProvider.IsRunningOnGitLabCI.Returns(false);
                tfBuildProvider.IsRunningOnVSTS.Returns(false);
                tfBuildProvider.IsRunningOnTFS.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_If_Running_On_Jenkins()
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

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                jenkinsProvider.IsRunningOnJenkins.Returns(true);
                bitriseProvider.IsRunningOnBitrise.Returns(false);
                travisCIProvider.IsRunningOnTravisCI.Returns(false);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(false);
                gitlabCIProvider.IsRunningOnGitLabCI.Returns(false);
                tfBuildProvider.IsRunningOnVSTS.Returns(false);
                tfBuildProvider.IsRunningOnTFS.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_If_Running_On_Bitrise()
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

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                jenkinsProvider.IsRunningOnJenkins.Returns(false);
                bitriseProvider.IsRunningOnBitrise.Returns(true);
                travisCIProvider.IsRunningOnTravisCI.Returns(false);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(false);
                gitlabCIProvider.IsRunningOnGitLabCI.Returns(false);
                tfBuildProvider.IsRunningOnVSTS.Returns(false);
                tfBuildProvider.IsRunningOnTFS.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_If_Running_On_TravisCI()
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

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                jenkinsProvider.IsRunningOnJenkins.Returns(false);
                bitriseProvider.IsRunningOnBitrise.Returns(false);
                travisCIProvider.IsRunningOnTravisCI.Returns(true);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(false);
                gitlabCIProvider.IsRunningOnGitLabCI.Returns(false);
                tfBuildProvider.IsRunningOnVSTS.Returns(false);
                tfBuildProvider.IsRunningOnTFS.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_If_Running_On_BitbucketPipelines()
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

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                jenkinsProvider.IsRunningOnJenkins.Returns(false);
                bitriseProvider.IsRunningOnBitrise.Returns(false);
                travisCIProvider.IsRunningOnTravisCI.Returns(false);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(true);
                gitlabCIProvider.IsRunningOnGitLabCI.Returns(false);
                tfBuildProvider.IsRunningOnVSTS.Returns(false);
                tfBuildProvider.IsRunningOnTFS.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_If_Running_On_GoCD()
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

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                jenkinsProvider.IsRunningOnJenkins.Returns(false);
                bitriseProvider.IsRunningOnBitrise.Returns(false);
                travisCIProvider.IsRunningOnTravisCI.Returns(false);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(false);
                goCDProvider.IsRunningOnGoCD.Returns(true);
                gitlabCIProvider.IsRunningOnGitLabCI.Returns(false);
                tfBuildProvider.IsRunningOnVSTS.Returns(false);
                tfBuildProvider.IsRunningOnTFS.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_If_Running_On_GitLabCI()
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

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                jenkinsProvider.IsRunningOnJenkins.Returns(false);
                bitriseProvider.IsRunningOnBitrise.Returns(false);
                travisCIProvider.IsRunningOnTravisCI.Returns(false);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(false);
                goCDProvider.IsRunningOnGoCD.Returns(false);
                gitlabCIProvider.IsRunningOnGitLabCI.Returns(true);
                tfBuildProvider.IsRunningOnVSTS.Returns(false);
                tfBuildProvider.IsRunningOnTFS.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_If_Running_On_VSTS()
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

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                jenkinsProvider.IsRunningOnJenkins.Returns(false);
                bitriseProvider.IsRunningOnBitrise.Returns(false);
                travisCIProvider.IsRunningOnTravisCI.Returns(false);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(false);
                goCDProvider.IsRunningOnGoCD.Returns(false);
                gitlabCIProvider.IsRunningOnGitLabCI.Returns(false);
                tfBuildProvider.IsRunningOnVSTS.Returns(true);
                tfBuildProvider.IsRunningOnTFS.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_If_Running_On_TFS()
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

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                jenkinsProvider.IsRunningOnJenkins.Returns(false);
                bitriseProvider.IsRunningOnBitrise.Returns(false);
                travisCIProvider.IsRunningOnTravisCI.Returns(false);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(false);
                goCDProvider.IsRunningOnGoCD.Returns(false);
                gitlabCIProvider.IsRunningOnGitLabCI.Returns(false);
                tfBuildProvider.IsRunningOnVSTS.Returns(false);
                tfBuildProvider.IsRunningOnTFS.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

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
                var jenkinsProvider = Substitute.For<IJenkinsProvider>();
                var bitriseProvider = Substitute.For<IBitriseProvider>();
                var travisCIProvider = Substitute.For<ITravisCIProvider>();
                var bitbucketPipelinesProvider = Substitute.For<IBitbucketPipelinesProvider>();
                var goCDProvider = Substitute.For<IGoCDProvider>();
                var gitlabCIProvider = Substitute.For<IGitLabCIProvider>();
                var tfBuildProvider = Substitute.For<ITFBuildProvider>();

                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                myGetProvider.IsRunningOnMyGet.Returns(false);
                bambooProvider.IsRunningOnBamboo.Returns(false);
                continuaCIProvider.IsRunningOnContinuaCI.Returns(false);
                bitriseProvider.IsRunningOnBitrise.Returns(false);
                travisCIProvider.IsRunningOnTravisCI.Returns(false);
                bitbucketPipelinesProvider.IsRunningOnBitbucketPipelines.Returns(false);
                goCDProvider.IsRunningOnGoCD.Returns(false);
                gitlabCIProvider.IsRunningOnGitLabCI.Returns(false);
                tfBuildProvider.IsRunningOnVSTS.Returns(false);
                tfBuildProvider.IsRunningOnTFS.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider,  jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitlabCIProvider, tfBuildProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.True(result);
            }
        }
    }
}