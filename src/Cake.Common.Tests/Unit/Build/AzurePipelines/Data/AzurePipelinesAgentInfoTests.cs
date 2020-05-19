// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.AzurePipelines.Data
{
    public sealed class AzurePipelinesAgentInfoTests
    {
            public sealed class TheBuildDirectoryProperty
            {
                [Fact]
                public void Should_Return_Correct_Value()
                {
                    // Given
                    var info = new AzurePipelinesInfoFixture().CreateAgentInfo();

                    // When
                    var result = info.BuildDirectory;

                    // Then
                    Assert.Equal("c:/agent/_work/1", result.FullPath);
                }
            }

            public sealed class TheHomeDirectoryProperty
            {
                [Fact]
                public void Should_Return_Correct_Value()
                {
                    // Given
                    var info = new AzurePipelinesInfoFixture().CreateAgentInfo();

                    // When
                    var result = info.HomeDirectory;

                    // Then
                    Assert.Equal("c:/agent", result.FullPath);
                }
            }

            public sealed class TheWorkingDirectoryProperty
            {
                [Fact]
                public void Should_Return_Correct_Value()
                {
                    // Given
                    var info = new AzurePipelinesInfoFixture().CreateAgentInfo();

                    // When
                    var result = info.WorkingDirectory;

                    // Then
                    Assert.Equal("c:/agent/_work", result.FullPath);
                }
            }

            public sealed class TheIdProperty
            {
                [Fact]
                public void Should_Return_Correct_Value()
                {
                    // Given
                    var info = new AzurePipelinesInfoFixture().CreateAgentInfo();

                    // When
                    var result = info.Id;

                    // Then
                    Assert.Equal(71, result);
                }
            }

            public sealed class TheNameProperty
            {
                [Fact]
                public void Should_Return_Correct_Value()
                {
                    // Given
                    var info = new AzurePipelinesInfoFixture().CreateAgentInfo();

                    // When
                    var result = info.Name;

                    // Then
                    Assert.Equal("Agent-1", result);
                }
            }

            public sealed class TheMachineNameProperty
            {
                [Fact]
                public void Should_Return_Correct_Value()
                {
                    // Given
                    var info = new AzurePipelinesInfoFixture().CreateAgentInfo();

                    // When
                    var result = info.MachineName;

                    // Then
                    Assert.Equal("BuildServer", result);
                }
            }

            public sealed class TheToolsDirectoryProperty
            {
                [Fact]
                public void Should_Return_Correct_Value()
                {
                    // Given
                    var info = new AzurePipelinesInfoFixture().CreateAgentInfo();

                    // When
                    var result = info.ToolsDirectory;

                    // Then
                    Assert.Equal("C:/hostedtoolcache/windows", result.FullPath);
                }
            }

            public sealed class TheJobNameProperty
            {
                [Fact]
                public void Should_Return_Correct_Value()
                {
                    // Given
                    var info = new AzurePipelinesInfoFixture().CreateAgentInfo();

                    // When
                    var result = info.JobName;

                    // Then
                    Assert.Equal("Job", result);
                }
            }

            public sealed class TheJobStatusProperty
            {
                [Fact]
                public void Should_Return_Correct_Value()
                {
                    // Given
                    var info = new AzurePipelinesInfoFixture().CreateAgentInfo();

                    // When
                    var result = info.JobStatus;

                    // Then
                    Assert.Equal("SucceededWithIssues", result);
                }
            }

            public sealed class TheIsHostedProperty
            {
                [Fact]
                public void Should_Return_True_On_Hosted_Agent()
                {
                    // Given
                    var info = new AzurePipelinesInfoFixture().CreateHostedAgentInfo();

                    // When
                    var result = info.IsHosted;

                    // Then
                    Assert.True(result);
                }

                [Fact]
                public void Should_Return_False_On_Other_Agent()
                {
                    // Given
                    var info = new AzurePipelinesInfoFixture().CreateAgentInfo();

                    // When
                    var result = info.IsHosted;

                    // Then
                    Assert.False(result);
                }
            }
    }
}
