// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using System;
using System.Globalization;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.TeamCity.Data
{
    public sealed class TeamCityBuildInfoTests
    {
        public sealed class TheBuildConfNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TeamCityInfoFixture().CreateBuildInfo();

                // When
                var result = info.BuildConfName;

                // Then
                Assert.Equal("Cake Build", result);
            }
        }

        public sealed class TheNumberProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TeamCityInfoFixture().CreateBuildInfo();

                // When
                var result = info.Number;

                // Then
                Assert.Equal("10-Foo", result);
            }
        }

        public sealed class TheStartDateTimeProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TeamCityInfoFixture().CreateBuildInfo();

                // When
                var result = info.StartDateTime;

                // Then
                var expected = new DateTimeOffset?(new DateTime(2020, 08, 22, 12, 34, 56, DateTimeKind.Local));
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Use_Build_Server_Local_Time()
            {
                // Given
                var now = DateTime.Now;
                var startDate = now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                var startTime = now.ToString("HHmmss", CultureInfo.InvariantCulture);

                var fixture = new TeamCityInfoFixture();
                fixture.SetBuildStartDate(startDate);
                fixture.SetBuildStartTime(startTime);

                var info = fixture.CreateBuildInfo();

                // When
                var result = info.StartDateTime;

                // Then
                var expected = new DateTimeOffset?(new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Local));
                Assert.Equal(expected, result);
            }

            [Theory]
            [InlineData(null, null)]
            [InlineData("Cake", null)]
            [InlineData(null, "Build")]
            [InlineData("Cake", "Build")]
            [InlineData("Cake", "123456")]
            [InlineData("20200822", "Build")]
            public void Should_Return_Null_If_Cannot_Parse_Values(string startDate, string startTime)
            {
                // Given
                var fixture = new TeamCityInfoFixture();
                fixture.SetBuildStartDate(startDate);
                fixture.SetBuildStartTime(startTime);

                var info = fixture.CreateBuildInfo();

                // When
                var result = info.StartDateTime;

                // Then
                Assert.Equal(null, result);
            }
        }

        public sealed class TheBranchProperty
        {
            [Fact]
            public void Should_Return_Empty_When_No_Properties()
            {
                // Given
                var info = new TeamCityInfoFixture().CreateBuildInfo();

                // When
                var result = info.BranchName;

                // Then
                Assert.Equal(string.Empty, result);
            }

            [Fact]
            public void Should_Return_Value_From_Properties()
            {
                // Given
                var fixture = new TeamCityInfoFixture();
                fixture.SetPropertiesFileContent(Properties.Resources.TeamCity_Properties_Xml);
                fixture.SetBuildConfigurationFileContent(Properties.Resources.TeamCity_Build_Configuration_Xml);
                var info = fixture.CreateBuildInfo();

                // When
                var result = info.BranchName;

                // Then
                Assert.Equal("branchName", result);
            }
        }

        public sealed class ThePropertiesProperty
        {
            [Fact]
            public void Should_Return_Empty_When_File_Not_Created()
            {
                // Given
                var fixture = new TeamCityInfoFixture();
                var info = fixture.CreateBuildInfo();

                // When
                var result = info.Properties;

                // Then
                Assert.Empty(result);
            }

            [Fact]
            public void Should_Return_Empty_When_Build_Properties_File_Not_Created()
            {
                // Given
                var fixture = new TeamCityInfoFixture();
                fixture.SetPropertiesFileContent(Properties.Resources.TeamCity_Properties_Xml);
                var info = fixture.CreateBuildInfo();

                // When
                var result = info.Properties;

                // Then
                Assert.Empty(result);
            }

            [Fact]
            public void Should_Return_Values_When_Files_Exist()
            {
                // Given
                var fixture = new TeamCityInfoFixture();
                fixture.SetPropertiesFileContent(Properties.Resources.TeamCity_Properties_Xml);
                fixture.SetBuildConfigurationFileContent(Properties.Resources.TeamCity_Build_Configuration_Xml);
                var info = fixture.CreateBuildInfo();

                // When
                var result = info.Properties;

                // Then
                Assert.NotEmpty(result);
                Assert.Equal(4, result.Count);
                Assert.Equal("3246", result["build.number"]);
            }
        }
    }
}