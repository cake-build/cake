// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Cake.Common.Tools.MSBuild;
using Cake.Core;
using Cake.Core.Diagnostics;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.MSBuild
{
    public sealed class MSBuildVerbosityExtensionsTests
    {
        public sealed class GetMSBuildVerbosityMethod
        {
            [Theory]
            [InlineData("quiet", Verbosity.Quiet)]
            [InlineData("minimal", Verbosity.Minimal)]
            [InlineData("normal", Verbosity.Normal)]
            [InlineData("detailed", Verbosity.Verbose)]
            [InlineData("diagnostic", Verbosity.Diagnostic)]
            public void Should_Convert_Valid_String_To_Verbosity_Enum(string verbosityName, Verbosity expectedVerbosity)
            {
                // Given

                // When
                var actualVerbosity = MSBuildVerbosityExtensions.GetMSBuildVerbosity(verbosityName);

                // Then
                Assert.Equal(expectedVerbosity, actualVerbosity);
            }

            [Theory]
            [InlineData("QUIET", Verbosity.Quiet)]
            [InlineData("MINIMAL", Verbosity.Minimal)]
            [InlineData("NORMAL", Verbosity.Normal)]
            [InlineData("DETAILED", Verbosity.Verbose)]
            [InlineData("DIAGNOSTIC", Verbosity.Diagnostic)]
            public void Should_Convert_Valid_UpperCase_String_To_Verbosity_Enum(string verbosityName, Verbosity expectedVerbosity)
            {
                // Given

                // When
                var actualVerbosity = MSBuildVerbosityExtensions.GetMSBuildVerbosity(verbosityName);

                // Then
                Assert.Equal(expectedVerbosity, actualVerbosity);
            }

            [Fact]
            public void Should_Throws_Exception_When_Parameter_Is_Null()
            {
                // Given

                // When
                var actualException = Assert.Throws<CakeException>(() => MSBuildVerbosityExtensions.GetMSBuildVerbosity(null));

                // Then
                Assert.Equal("Encountered unknown MSBuild build log verbosity ''. Valid values are 'quiet', 'minimal', 'normal', 'detailed' and 'diagnostic'.", actualException.Message);
            }

            [Theory]
            [InlineData("", "Encountered unknown MSBuild build log verbosity ''. Valid values are 'quiet', 'minimal', 'normal', 'detailed' and 'diagnostic'.")]
            [InlineData("invalid", "Encountered unknown MSBuild build log verbosity 'invalid'. Valid values are 'quiet', 'minimal', 'normal', 'detailed' and 'diagnostic'.")]
            public void Should_Throws_Exception_When_Parameter_Has_Invalid_Value(string invalidVerbosityValue, string expectedMessage)
            {
                // Given

                // When
                var actualException = Assert.Throws<CakeException>(() => MSBuildVerbosityExtensions.GetMSBuildVerbosity(invalidVerbosityValue));

                // Then
                Assert.Equal(expectedMessage, actualException.Message);
            }
        }
   }
}