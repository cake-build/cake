// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.WoodpeckerCI.Data;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.WoodpeckerCI.Data
{
    public sealed class WoodpeckerCIForgeTypeTests
    {
        public sealed class TheParseForgeTypeMethod
        {
            [Theory]
            [InlineData(null, WoodpeckerCIForgeType.Unknown)]
            [InlineData("", WoodpeckerCIForgeType.Unknown)]
            [InlineData("   ", WoodpeckerCIForgeType.Unknown)]
            [InlineData("unknown", WoodpeckerCIForgeType.Unknown)]
            public void Should_Return_Unknown_For_Invalid_Values(string value, WoodpeckerCIForgeType expected)
            {
                // When
                var result = WoodpeckerCIForgeTypeExtensions.ParseForgeType(value);

                // Then
                Assert.Equal(expected, result);
            }

            [Theory]
            [InlineData("bitbucket", WoodpeckerCIForgeType.Bitbucket)]
            [InlineData("BITBUCKET", WoodpeckerCIForgeType.Bitbucket)]
            [InlineData("Bitbucket", WoodpeckerCIForgeType.Bitbucket)]
            [InlineData("bitbucket_dc", WoodpeckerCIForgeType.BitbucketDC)]
            [InlineData("BITBUCKET_DC", WoodpeckerCIForgeType.BitbucketDC)]
            [InlineData("Bitbucket_DC", WoodpeckerCIForgeType.BitbucketDC)]
            [InlineData("forgejo", WoodpeckerCIForgeType.Forgejo)]
            [InlineData("FORGEJO", WoodpeckerCIForgeType.Forgejo)]
            [InlineData("Forgejo", WoodpeckerCIForgeType.Forgejo)]
            [InlineData("gitea", WoodpeckerCIForgeType.Gitea)]
            [InlineData("GITEA", WoodpeckerCIForgeType.Gitea)]
            [InlineData("Gitea", WoodpeckerCIForgeType.Gitea)]
            [InlineData("github", WoodpeckerCIForgeType.GitHub)]
            [InlineData("GITHUB", WoodpeckerCIForgeType.GitHub)]
            [InlineData("GitHub", WoodpeckerCIForgeType.GitHub)]
            [InlineData("gitlab", WoodpeckerCIForgeType.GitLab)]
            [InlineData("GITLAB", WoodpeckerCIForgeType.GitLab)]
            [InlineData("GitLab", WoodpeckerCIForgeType.GitLab)]
            public void Should_Parse_Valid_Forge_Types(string value, WoodpeckerCIForgeType expected)
            {
                // When
                var result = WoodpeckerCIForgeTypeExtensions.ParseForgeType(value);

                // Then
                Assert.Equal(expected, result);
            }
        }
    }
}
