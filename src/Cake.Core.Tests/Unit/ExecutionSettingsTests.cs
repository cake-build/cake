// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class ExecutionSettingsTests
    {
        [Fact]
        public void Should_Have_Zero_Targets_Initially()
        {
            // Given
            var settings = new ExecutionSettings();

            // When

            // Then
            Assert.Equal(Array.Empty<string>(), settings.Targets);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void Should_Not_Allow_Empty_Targets(string actual)
        {
            // Given
            var settings = new ExecutionSettings().SetTarget(actual);

            // When

            // Then
            Assert.Equal(Array.Empty<string>(), settings.Targets);
        }

        [Fact]
        public void Should_Not_Allow_Empty_Targets_2()
        {
            // Given
            var settings = new ExecutionSettings().SetTargets(new string[] { "" });

            // When

            // Then
            Assert.Equal(Array.Empty<string>(), settings.Targets);
        }

        [Fact]
        public void Should_Not_Allow_Empty_Targets_3()
        {
            // Given
            var settings = new ExecutionSettings().SetTargets(new string[] { " " });

            // When

            // Then
            Assert.Equal(Array.Empty<string>(), settings.Targets);
        }

        [Fact]
        public void Should_Not_Allow_Empty_Targets_4()
        {
            // Given
            var settings = new ExecutionSettings().SetTargets(new string[] { " ", " " });

            // When

            // Then
            Assert.Equal(Array.Empty<string>(), settings.Targets);
        }

        [Fact]
        public void Should_Not_Allow_Empty_Targets_5()
        {
            // Given
            var settings = new ExecutionSettings().SetTargets(new string[] { " ", " ", "A" });

            // When

            // Then
            Assert.Equal(new string[] { "A" }, settings.Targets);
        }

        [Fact]
        public void Should_Clear_Existing_Targets_When_Setting_Targets_1()
        {
            // Given
            var settings = new ExecutionSettings().SetTargets(new string[] { "B", "C" });

            // When
            settings.SetTarget("A");

            // Then
            Assert.Equal(new string[] { "A" }, settings.Targets);
        }

        [Fact]
        public void Should_Clear_Existing_Targets_When_Setting_Targets_2()
        {
            // Given
            var settings = new ExecutionSettings().SetTargets(new string[] { "B", "C" });

            // When
            settings.SetTarget("");

            // Then
            Assert.Equal(Array.Empty<string>(), settings.Targets);
        }

        [Fact]
        public void Should_Clear_Existing_Targets_When_Setting_Targets_3()
        {
            // Given
            var settings = new ExecutionSettings().SetTarget("A");

            // When
            settings.SetTargets(new string[] { "B", "C" });

            // Then
            Assert.Equal(new string[] { "B", "C" }, settings.Targets);
        }

        [Fact]
        public void Should_Clear_Existing_Targets_When_Setting_Targets_4()
        {
            // Given
            var settings = new ExecutionSettings().SetTarget("A");

            // When
            settings.SetTarget("");

            // Then
            Assert.Equal(Array.Empty<string>(), settings.Targets);
        }
    }
}
