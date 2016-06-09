// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools
{
    public sealed class DotNetBuildSettingsExtensionsTests
    {
        public sealed class TheWithTargetMethod
        {
            [Fact]
            public void Should_Add_Target_To_Configuration()
            {
                // Given
                var settings = new DotNetBuildSettings(new FilePath("./Test.sln"));

                // When
                settings.WithTarget("Target");

                // Then
                Assert.True(settings.Targets.Contains("Target"));
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetBuildSettings(new FilePath("./Test.sln"));

                // When
                var result = settings.WithTarget("Target");

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheWithPropertyMethod
        {
            [Fact]
            public void Should_Add_Property_To_Configuration()
            {
                // Given
                var settings = new DotNetBuildSettings(new FilePath("./Test.sln"));

                // When
                settings.WithProperty("PropertyName", "Value");

                // Then
                Assert.True(settings.Properties.ContainsKey("PropertyName"));
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetBuildSettings(new FilePath("./Test.sln"));

                // When
                var result = settings.WithProperty("PropertyName", "Value");

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetConfigurationMethod
        {
            [Fact]
            public void Should_Set_Configuration()
            {
                // Given
                var settings = new DotNetBuildSettings(new FilePath("./Test.sln"));

                // When
                settings.SetConfiguration("TheConfiguration");

                // Then
                Assert.Equal("TheConfiguration", settings.Configuration);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetBuildSettings(new FilePath("./Test.sln"));

                // When
                var result = settings.SetConfiguration("TheConfiguration");

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetVerbosityMethod
        {
            [Theory]
            [InlineData(Verbosity.Quiet)]
            [InlineData(Verbosity.Minimal)]
            [InlineData(Verbosity.Normal)]
            [InlineData(Verbosity.Verbose)]
            [InlineData(Verbosity.Diagnostic)]
            public void Should_Set_Verbosity(Verbosity verbosity)
            {
                // Given
                var settings = new DotNetBuildSettings(new FilePath("./Test.sln"));

                // When
                settings.SetVerbosity(verbosity);

                // Then
                Assert.Equal(verbosity, settings.Verbosity);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetBuildSettings(new FilePath("./Test.sln"));

                // When
                var result = settings.SetVerbosity(Verbosity.Normal);

                // Then
                Assert.Equal(settings, result);
            }
        }
    }
}
