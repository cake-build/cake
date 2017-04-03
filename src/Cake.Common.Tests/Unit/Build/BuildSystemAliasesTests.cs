// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build
{
    public sealed class BuildSystemAliasesTests
    {
        public sealed class TheBuildSystemMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => BuildSystemAliases.BuildSystem(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }
        }
        public sealed class TheAppVeyorMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => BuildSystemAliases.AppVeyor(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }
        }
        public sealed class TheTeamCityMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => BuildSystemAliases.TeamCity(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }
        }
        public sealed class TheBambooMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => BuildSystemAliases.Bamboo(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }
        }

        public sealed class TheJenkinsMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => BuildSystemAliases.Jenkins(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }
        }

        public sealed class TheBitriseMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => BuildSystemAliases.Bitrise(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }
        }

        public sealed class TheTravisCIMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => BuildSystemAliases.TravisCI(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }
        }

        public sealed class TheBitbucketPipelinesMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => BuildSystemAliases.BitbucketPipelines(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }
        }

        public sealed class TheGitLabCIMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => BuildSystemAliases.GitLabCI(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }
        }

        public sealed class TheTFBuildMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => BuildSystemAliases.TFBuild(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }
        }
    }
}