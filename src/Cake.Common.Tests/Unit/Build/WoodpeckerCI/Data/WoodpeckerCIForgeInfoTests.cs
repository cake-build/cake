// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.WoodpeckerCI.Data;
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.WoodpeckerCI.Data
{
    public sealed class WoodpeckerCIForgeInfoTests
    {
        public sealed class TheTypeProperty
        {
            [Fact]
            public void Should_Return_Correct_Forge_Type()
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                fixture.SetEnvironmentVariable("CI_FORGE_TYPE", "github");
                var forgeInfo = fixture.CreateForgeInfo();

                // When
                var result = forgeInfo.Type;

                // Then
                Assert.Equal(WoodpeckerCIForgeType.GitHub, result);
            }

            [Fact]
            public void Should_Return_Unknown_For_Invalid_Forge_Type()
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                fixture.SetEnvironmentVariable("CI_FORGE_TYPE", "invalid");
                var forgeInfo = fixture.CreateForgeInfo();

                // When
                var result = forgeInfo.Type;

                // Then
                Assert.Equal(WoodpeckerCIForgeType.Unknown, result);
            }

            [Fact]
            public void Should_Return_Unknown_For_Empty_Forge_Type()
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                fixture.SetEnvironmentVariable("CI_FORGE_TYPE", "");
                var forgeInfo = fixture.CreateForgeInfo();

                // When
                var result = forgeInfo.Type;

                // Then
                Assert.Equal(WoodpeckerCIForgeType.Unknown, result);
            }

            [Theory]
            [InlineData("bitbucket", WoodpeckerCIForgeType.Bitbucket)]
            [InlineData("bitbucket_dc", WoodpeckerCIForgeType.BitbucketDC)]
            [InlineData("forgejo", WoodpeckerCIForgeType.Forgejo)]
            [InlineData("gitea", WoodpeckerCIForgeType.Gitea)]
            [InlineData("github", WoodpeckerCIForgeType.GitHub)]
            [InlineData("gitlab", WoodpeckerCIForgeType.GitLab)]
            public void Should_Parse_All_Valid_Forge_Types(string forgeType, WoodpeckerCIForgeType expected)
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                fixture.SetEnvironmentVariable("CI_FORGE_TYPE", forgeType);
                var forgeInfo = fixture.CreateForgeInfo();

                // When
                var result = forgeInfo.Type;

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class TheUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Uri_For_Valid_Url()
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                fixture.SetEnvironmentVariable("CI_FORGE_URL", "https://github.com");
                var forgeInfo = fixture.CreateForgeInfo();

                // When
                var result = forgeInfo.Url;

                // Then
                Assert.NotNull(result);
                Assert.Equal("https://github.com/", result.ToString());
            }

            [Fact]
            public void Should_Return_Null_For_Invalid_Url()
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                fixture.SetEnvironmentVariable("CI_FORGE_URL", "not-a-valid-url");
                var forgeInfo = fixture.CreateForgeInfo();

                // When
                var result = forgeInfo.Url;

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Return_Null_For_Empty_Url()
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                fixture.SetEnvironmentVariable("CI_FORGE_URL", "");
                var forgeInfo = fixture.CreateForgeInfo();

                // When
                var result = forgeInfo.Url;

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Return_Null_For_Missing_Url()
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                fixture.SetEnvironmentVariable("CI_FORGE_URL", null);
                var forgeInfo = fixture.CreateForgeInfo();

                // When
                var result = forgeInfo.Url;

                // Then
                Assert.Null(result);
            }
        }
    }
}
