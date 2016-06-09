// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.ContinuaCI.Data
{
    public sealed class ContinuaCIEnvironmentInfoTests
    {
        public sealed class TheVersionProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Version;

                // Then
                Assert.Equal("v1.6.6.6", result);
            }
        }

        public sealed class TheVariableProperty
        {
            [Fact]
            public void Should_Return_Correct_Values()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Variable;

                // Then
                Assert.Equal(3, result.Count);
                Assert.Equal("gorgonzola", result["TestVar1"]);
                Assert.Equal("is", result["TestVar2"]);
                Assert.Equal("tasty", result["TestVarX"]);
            }
        }

        public sealed class TheVAgentPropertyProperty
        {
            [Fact]
            public void Should_Return_Correct_Values()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.AgentProperty;

                // Then
                Assert.Equal(3, result.Count);
                Assert.Equal(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319", result["DotNet.4.0.FrameworkPathX64"]);
                Assert.Equal(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319", result["MSBuild.4.0.PathX86"]);
                Assert.Equal("True", result["ServerFileTransport.UNCAvailable"]);
            }
        }
    }
}
