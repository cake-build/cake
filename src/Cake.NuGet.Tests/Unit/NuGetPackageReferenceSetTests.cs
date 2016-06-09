// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Linq;
using System.Runtime.Versioning;
using Cake.Core.IO;
using Xunit;

namespace Cake.NuGet.Tests.Unit
{
    public sealed class NuGetPackageReferenceSetTests
    {
        private static readonly FrameworkName _dummyFrameworkName = new FrameworkName("DummyFx", new Version());

        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_References_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new NuGetPackageReferenceSet(_dummyFrameworkName, null));

                // Then
                Assert.IsArgumentNullException(result, "references");
            }

            [Fact]
            public void Should_Allow_Null_Framework_Name()
            {
                // Given
                var references = Enumerable.Empty<FilePath>();

                // When
                var result = Record.Exception(() => new NuGetPackageReferenceSet(null, references));

                // Then
                Assert.Null(result);
            }
        }

        public sealed class TheSupportedFrameworksProperty
        {
            [Fact]
            public void Should_Be_Empty_When_Framework_Name_Is_Null()
            {
                // Given
                var references = Enumerable.Empty<FilePath>();
                var referenceSet = new NuGetPackageReferenceSet(null, references);

                // When
                var result = referenceSet.SupportedFrameworks;

                // Then
                Assert.Empty(result);
            }

            [Fact]
            public void Should_Be_Include_Framework_Name()
            {
                // Given
                var referenceSet = new NuGetPackageReferenceSet(_dummyFrameworkName, Enumerable.Empty<FilePath>());

                // When
                var result = referenceSet.SupportedFrameworks;

                // Then
                Assert.Equal(new[] { _dummyFrameworkName }, result);
            }
        }
    }
}
