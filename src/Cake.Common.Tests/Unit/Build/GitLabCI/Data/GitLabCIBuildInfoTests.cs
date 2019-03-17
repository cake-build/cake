// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GitLabCI.Data
{
    public sealed class GitLabCIBuildInfoTests
    {
        public sealed class TheIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.Id;

                // Then
                Assert.Equal(50, result);
            }

            [Fact]
            public void Should_Return_Correct_Value_Version_Nine_Or_Newer()
            {
                // Given
                var info = new GitLabCIInfoFixture(true).CreateBuildInfo();

                // When
                var result = info.Id;

                // Then
                Assert.Equal(50, result);
            }
        }

        public sealed class TheManualProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.Manual;

                // Then
                Assert.Equal(true, result);
            }

            [Fact]
            public void Should_Return_Correct_Value_Version_Nine_Or_Newer()
            {
                // Given
                var info = new GitLabCIInfoFixture(true).CreateBuildInfo();

                // When
                var result = info.Manual;

                // Then
                Assert.Equal(true, result);
            }
        }

        public sealed class TheNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.Name;

                // Then
                Assert.Equal("spec:other", result);
            }

            [Fact]
            public void Should_Return_Correct_Value_Version_Nine_Or_Newer()
            {
                // Given
                var info = new GitLabCIInfoFixture(true).CreateBuildInfo();

                // When
                var result = info.Name;

                // Then
                Assert.Equal("spec:other", result);
            }
        }

        public sealed class ThePipelineIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.PipelineId;

                // Then
                Assert.Equal(1000, result);
            }
        }

        public sealed class ThePipelineIIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.PipelineIId;

                // Then
                Assert.Equal(100, result);
            }
        }

        public sealed class TheReferenceProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.Reference;

                // Then
                Assert.Equal("1ecfd275763eff1d6b4844ea3168962458c9f27a", result);
            }

            [Fact]
            public void Should_Return_Correct_Value_Version_Nine_Or_Newer()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.Reference;

                // Then
                Assert.Equal("1ecfd275763eff1d6b4844ea3168962458c9f27a", result);
            }
        }

        public sealed class TheRefNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.RefName;

                // Then
                Assert.Equal("master", result);
            }

            [Fact]
            public void Should_Return_Correct_Value_Version_Nine_Or_Newer()
            {
                // Given
                var info = new GitLabCIInfoFixture(true).CreateBuildInfo();

                // When
                var result = info.RefName;

                // Then
                Assert.Equal("master", result);
            }
        }

        public sealed class TheRepoUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.RepoUrl;

                // Then
                Assert.Equal("https://gitab-ci-token:abcde-1234ABCD5678ef@gitlab.com/gitlab-org/gitlab-ce.git", result);
            }

            [Fact]
            public void Should_Return_Correct_Value_Version_Nine_Or_Newer()
            {
                // Given
                var info = new GitLabCIInfoFixture(true).CreateBuildInfo();

                // When
                var result = info.RepoUrl;

                // Then
                Assert.Equal("https://gitab-ci-token:abcde-1234ABCD5678ef@gitlab.com/gitlab-org/gitlab-ce.git", result);
            }
        }

        public sealed class TheStageProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.Stage;

                // Then
                Assert.Equal("test", result);
            }

            [Fact]
            public void Should_Return_Correct_Value_Version_Nine_Or_Newer()
            {
                // Given
                var info = new GitLabCIInfoFixture(true).CreateBuildInfo();

                // When
                var result = info.Stage;

                // Then
                Assert.Equal("test", result);
            }
        }

        public sealed class TheTagProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.Tag;

                // Then
                Assert.Equal("1.0.0", result);
            }

            [Fact]
            public void Should_Return_Correct_Value_Version_Nine_Or_Newer()
            {
                // Given
                var info = new GitLabCIInfoFixture(true).CreateBuildInfo();

                // When
                var result = info.Tag;

                // Then
                Assert.Equal("1.0.0", result);
            }
        }

        public sealed class TheTokenProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.Token;

                // Then
                Assert.Equal("abcde-1234ABCD5678ef", result);
            }

            [Fact]
            public void Should_Return_Correct_Value_Version_Nine_Or_Newer()
            {
                // Given
                var info = new GitLabCIInfoFixture(true).CreateBuildInfo();

                // When
                var result = info.Token;

                // Then
                Assert.Equal("abcde-1234ABCD5678ef", result);
            }
        }

        public sealed class TheTriggeredProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.Triggered;

                // Then
                Assert.Equal(true, result);
            }

            [Fact]
            public void Should_Return_Correct_Value_Version_Nine_Or_Newer()
            {
                // Given
                var info = new GitLabCIInfoFixture(true).CreateBuildInfo();

                // When
                var result = info.Triggered;

                // Then
                Assert.Equal(true, result);
            }
        }

        public sealed class TheUserEmailProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.UserEmail;

                // Then
                Assert.Equal("anthony@warwickcontrol.com", result);
            }
        }

        public sealed class TheUserIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.UserId;

                // Then
                Assert.Equal(42, result);
            }
        }
    }
}
