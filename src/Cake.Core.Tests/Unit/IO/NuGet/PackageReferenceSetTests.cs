using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Xunit;

namespace Cake.Core.Tests.Unit.IO.NuGet
{
    public sealed class PackageReferenceSetTests
    {
        private static readonly FrameworkName _dummyFrameworkName = new FrameworkName("DummyFx", new Version());

        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_References_Is_Null()
            {
                // Given
                IEnumerable<FilePath> references = null;

                // When
                var result = Record.Exception(() => new PackageReferenceSet(_dummyFrameworkName, references));

                // Then
                Assert.IsArgumentNullException(result, "references");
            }

            [Fact]
            public void Should_Allow_Null_Framework_Name()
            {
                // Given
                var references = Enumerable.Empty<FilePath>();
                FrameworkName frameworkName = null;

                // When
                var result = Record.Exception(() => new PackageReferenceSet(frameworkName, references));

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
                FrameworkName frameworkName = null;
                var referenceSet = new PackageReferenceSet(frameworkName, references);

                // When
                var result = referenceSet.SupportedFrameworks;

                // Then
                Assert.Empty(result);
            }

            [Fact]
            public void Should_Be_Include_Framework_Name()
            {
                // Given
                var referenceSet = new PackageReferenceSet(_dummyFrameworkName, Enumerable.Empty<FilePath>());

                // When
                var result = referenceSet.SupportedFrameworks;

                // Then
                Assert.Equal(new[] { _dummyFrameworkName }, result);
            }
        }
    }
}