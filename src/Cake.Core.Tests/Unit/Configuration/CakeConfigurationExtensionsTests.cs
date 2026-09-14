// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.Tests.Fixtures;
using Xunit;

namespace Cake.Core.Tests.Unit.Configuration
{
    public sealed class CakeConfigurationExtensionsTests
    {
        public sealed class TheGetVerbosityMethod
        {
            [Fact]
            public void Should_Return_Default_When_Configuration_Is_Null()
            {
                // Given
                ICakeConfiguration configuration = null;

                // When
                var result = configuration.GetVerbosity(null, Verbosity.Minimal);

                // Then
                Assert.Equal(Verbosity.Minimal, result);
            }

            [Fact]
            public void Should_Return_Default_When_Value_Is_Missing()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();

                // When
                var result = fixture.Create().GetVerbosity(null);

                // Then
                Assert.Equal(Verbosity.Normal, result);
            }

            [Theory]
            [InlineData("invalid")]
            [InlineData(" ")]
            public void Should_Return_Default_When_Value_Is_Invalid(string value)
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.Environment.SetEnvironmentVariable("CAKE_SETTINGS_VERBOSITY", value);

                // When
                var result = fixture.Create().GetVerbosity(null);

                // Then
                Assert.Equal(Verbosity.Normal, result);
            }

            [Theory]
            [InlineData("Diagnostic", Verbosity.Diagnostic)]
            [InlineData("quiet", Verbosity.Quiet)]
            [InlineData("q", Verbosity.Quiet)]
            public void Should_Parse_Configured_Verbosity(string value, Verbosity expected)
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.Environment.SetEnvironmentVariable("CAKE_SETTINGS_VERBOSITY", value);

                // When
                var result = fixture.Create().GetVerbosity(null);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Prefer_Command_Line_Over_Configuration()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.Environment.SetEnvironmentVariable("CAKE_SETTINGS_VERBOSITY", "Diagnostic");
                var configuration = fixture.Create();

                // When
                var result = configuration.GetVerbosity(Verbosity.Normal);

                // Then
                Assert.Equal(Verbosity.Normal, result);
            }

            [Fact]
            public void Should_Use_Configuration_When_Command_Line_Is_Not_Specified()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.Environment.SetEnvironmentVariable("CAKE_SETTINGS_VERBOSITY", "Diagnostic");
                var configuration = fixture.Create();

                // When
                var result = configuration.GetVerbosity(null);

                // Then
                Assert.Equal(Verbosity.Diagnostic, result);
            }
        }
    }
}
