// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;
using Cake.Core.Tests.Fixtures;
using Xunit;

namespace Cake.Core.Tests.Unit.Tooling
{
    public sealed class ToolTests
    {
        public sealed class TheRunProcessMethod
        {
            [Fact]
            public void Should_Use_Arguments_Provided_In_Tool_Settings()
            {
                // Given
                var fixture = new DummyToolFixture();
                fixture.Settings.ArgumentCustomization = builder => builder.Append("--bar");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--foo --bar", result.Args);
            }

            [Fact]
            public void Should_Replace_Arguments_Provided_In_Tool_Settings_If_Returning_A_New_Builder()
            {
                // Given
                var fixture = new DummyToolFixture();
                fixture.Settings.ArgumentCustomization = builder =>
                {
                    var newBuilder = new ProcessArgumentBuilder();
                    newBuilder.Append("--bar");
                    return newBuilder;
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--bar", result.Args);
            }

            [Fact]
            public void Should_Replace_Arguments_Provided_In_Tool_Settings_If_Returning_A_String()
            {
                // Given
                var fixture = new DummyToolFixture();
                fixture.Settings.ArgumentCustomization = builder => "--bar";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--bar", result.Args);
            }
        }
    }
}
