// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Xunit;

namespace Cake.Frosting.Tests.Unit
{
    public sealed class CakeOptionsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_Arguments_To_Empty_Dictionary()
            {
                // Given
                var options = new CakeHostOptions();

                // When
                var result = options.Arguments;

                // Then
                Assert.NotNull(result);
            }

            [Fact]
            public void Should_Set_Working_Directory_To_Default_Value()
            {
                // Given
                var options = new CakeHostOptions();

                // When
                var result = options.WorkingDirectory;

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Set_Target_To_Default_Value()
            {
                // Given
                var options = new CakeHostOptions();

                // When
                var result = options.Target;

                // Then
                Assert.Equal("Default", result);
            }

            [Fact]
            public void Should_Set_Verbosity_To_Default_Value()
            {
                // Given
                var options = new CakeHostOptions();

                // When
                var result = options.Verbosity;

                // Then
                Assert.Equal(Verbosity.Normal, result);
            }

            [Fact]
            public void Should_Set_Command_To_Default_Value()
            {
                // Given
                var options = new CakeHostOptions();

                // When
                var result = options.Command;

                // Then
                Assert.Equal(CakeHostCommand.Run, result);
            }
        }
    }
}
