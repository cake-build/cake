// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq;
using Cake.Core.Polyfill;
using Xunit;

namespace Cake.Core.Tests.Unit.Polyfill;

public sealed class ProcessHelperTests
{
    public sealed class ProcessStartInfoEnvironmentCasing
    {
        [Fact]
        public void Should_Be_Case_Insensitive_On_Windows_And_Case_Sensitive_On_Unix()
        {
            // Given
            var info = new ProcessStartInfo();
            const string originalKey = "CAKE_PROCESSHELPER_CASE";
            const string mixedKey = "cake_processhelper_case";
            info.Environment[originalKey] = "first";
            var countAfterFirst = info.Environment.Count;

            // When
            info.Environment[mixedKey] = "second";

            // Then
            var matchingKeys = info.Environment.Keys.Count(key =>
                string.Equals(key, originalKey, StringComparison.OrdinalIgnoreCase));

            if (OperatingSystem.IsWindows())
            {
                Assert.Equal(countAfterFirst, info.Environment.Count);
                Assert.Equal(1, matchingKeys);
                Assert.Equal("second", info.Environment[originalKey]);
            }
            else
            {
                Assert.Equal(countAfterFirst + 1, info.Environment.Count);
                Assert.Equal(2, matchingKeys);
                Assert.Equal("first", info.Environment[originalKey]);
                Assert.Equal("second", info.Environment[mixedKey]);
            }
        }
    }

    public sealed class TheSetEnvironmentVariableMethod
    {
        [Fact]
        public void Should_Reuse_Existing_Environment_Key_Ignoring_Case()
        {
            // Given
            var info = new ProcessStartInfo();
            const string existingKey = "cake_processhelper_reuse";
            const string requestedKey = "CAKE_PROCESSHELPER_REUSE";
            info.Environment[existingKey] = "first";
            var countAfterSeed = info.Environment.Count;

            // When
            ProcessHelper.SetEnvironmentVariable(info, requestedKey, "True");

            // Then
            var storedKey = info.Environment.Keys.Single(key =>
                string.Equals(key, requestedKey, StringComparison.OrdinalIgnoreCase));

            Assert.Equal(countAfterSeed, info.Environment.Count);
            Assert.Equal(existingKey, storedKey);
            Assert.Equal("True", info.Environment[storedKey]);
        }
    }
}
