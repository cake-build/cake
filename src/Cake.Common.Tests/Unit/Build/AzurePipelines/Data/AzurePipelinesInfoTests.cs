// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.AzurePipelines.Data
{
    public sealed class AzurePipelinesInfoTests
    {
        public sealed class TheIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.Id;

                // Then
                Assert.Equal(100234, result);
            }
        }

        public sealed class TheNumberProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.Number;

                // Then
                Assert.Equal("Build-20160927.1", result);
            }
        }

        public sealed class TheUriProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.Uri;

                // Then
                var uri = new Uri("vstfs:///Build/Build/1430");
                Assert.Equal(uri, result);
            }
        }

        public sealed class TheQueuedByProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.QueuedBy;

                // Then
                Assert.Equal(@"[DefaultCollection]\Project Collection Service Accounts", result);
            }
        }

        public sealed class TheRequestedForProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.RequestedFor;

                // Then
                Assert.Equal("Alistair Chapman", result);
            }
        }

        public sealed class TheRequestedForEmailProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.RequestedForEmail;

                // Then
                Assert.Equal("author@mail.com", result);
            }
        }

        public sealed class TheAccessTokenProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.AccessToken;

                // Then
                Assert.Equal("f662dbe218144c86bdecb1e9b2eb336c", result);
            }
        }

        public sealed class TheDebugProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.Debug;

                // Then
                Assert.Equal(true, result);
            }
        }

        public sealed class TheArtifactStagingDirectoryProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.ArtifactStagingDirectory;

                // Then
                Assert.Equal(@"c:/agent/_work/1/a", result.FullPath);
            }
        }

        public sealed class TheBinariesDirectoryProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.BinariesDirectory;

                // Then
                Assert.Equal(@"c:/agent/_work/1/b", result.FullPath);
            }
        }

        public sealed class TheSourcesDirectoryProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.SourcesDirectory;

                // Then
                Assert.Equal(@"c:/agent/_work/1/s", result.FullPath);
            }
        }

        public sealed class TheStagingDirectoryProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.StagingDirectory;

                // Then
                Assert.Equal(@"c:/agent/_work/1/a", result.FullPath);
            }
        }

        public sealed class TheTestResultsDirectoryProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.TestResultsDirectory;

                // Then
                Assert.Equal(@"c:/agent/_work/1/TestResults", result.FullPath);
            }
        }

        public sealed class TheReasonProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.Reason;

                // Then
                Assert.Equal(@"PullRequest", result);
            }
        }

        public sealed class TheTriggeredByBuildIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.TriggeredBy.BuildId;

                // Then
                Assert.Equal(1, result);
            }
        }

        public sealed class TheTriggeredByDefinitionIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.TriggeredBy.DefinitionId;

                // Then
                Assert.Equal(1, result);
            }
        }

        public sealed class TheTriggeredByDefinitionNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.TriggeredBy.DefinitionName;

                // Then
                Assert.Equal(@"Build", result);
            }
        }

        public sealed class TheTriggeredByBuildNumberProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.TriggeredBy.BuildNumber;

                // Then
                Assert.Equal(@"123", result);
            }
        }

        public sealed class TheTriggeredByProjectIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateBuildInfo();

                // When
                var result = info.TriggeredBy.ProjectId;

                // Then
                Assert.Equal(@"456", result);
            }
        }
    }
}
