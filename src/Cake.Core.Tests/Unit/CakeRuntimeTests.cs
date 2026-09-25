// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Polyfill;
using Xunit;

namespace Cake.Core.Tests.Unit;

public sealed class CakeRuntimeTests
{
    public sealed class TheBuiltFrameworkProperty
    {
        [Fact]
        public void Should_Return_Correct_Result_For_CoreClr()
        {
            // Given
            var runtime = new CakeRuntime();
            var expect = $".NETCoreApp,Version=v{Environment.Version.Major}.{Environment.Version.Minor}";

            // When
            var framework = runtime.BuiltFramework;

            // Then
            Assert.Equal(expect, framework.FullName);
        }
    }

    public sealed class TheRuntimeProperty
    {
        [Fact]
        public void Should_Return_CoreClr()
        {
            // Given
            var runtime = new CakeRuntime();

            // When
            var runtimeKind = runtime.Runtime;

            // Then
            Assert.Equal(Runtime.CoreClr, runtimeKind);
        }
    }

    public sealed class TheIsCoreClrProperty
    {
        [Fact]
        public void Should_Return_True()
        {
            // Given
            var runtime = new CakeRuntime();

            // When
            var isCoreClr = runtime.IsCoreClr;

            // Then
            Assert.True(isCoreClr);
        }
    }
}
