// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools
{
    public sealed class DotNetBuildSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_Default_Verbosity_To_Normal()
            {
                // Given, When
                var settings = new DotNetBuildSettings(new FilePath("./Test.sln"));

                // Then
                Assert.Equal(Verbosity.Normal, settings.Verbosity);
            }

            [Fact]
            public void Should_Throw_If_Solution_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new DotNetBuildSettings(null));

                // Then
                AssertEx.IsArgumentNullException(result, "solution");
            }
        }

        public sealed class TheTargetsProperty
        {
            [Fact]
            public void Should_Return_A_Set_That_Is_Case_Insensitive()
            {
                // Given
                var settings = new DotNetBuildSettings(new FilePath("./Test.sln"));

                // When
                settings.Targets.Add("TARGET");

                // Then
                Assert.True(settings.Targets.Contains("target"));
            }
        }

        public sealed class ThePropertiesProperty
        {
            [Fact]
            public void Should_Return_A_Dictionary_That_Is_Case_Insensitive()
            {
                // Given
                var settings = new DotNetBuildSettings(new FilePath("./Test.sln"));

                // When
                settings.Properties.Add("THEKEY", new[] { "THEVALUE" });

                // Then
                Assert.True(settings.Properties.ContainsKey("thekey"));
            }
        }

        public sealed class TheConfigurationProperty
        {
            [Fact]
            public void Should_Be_Empty_By_Default()
            {
                // Given, When
                var settings = new DotNetBuildSettings(new FilePath("./Test.sln"));

                // Then
                Assert.Equal(string.Empty, settings.Configuration);
            }
        }

        public sealed class TheSolutionProperty
        {
            [Fact]
            public void Should_Return_Passed_Constructor_Argument()
            {
                // Given
                var solution = new FilePath("./Test.sln");

                // When
                var settings = new DotNetBuildSettings(solution);

                // Then
                Assert.Equal(solution, settings.Solution);
            }
        }
    }
}