// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Cake.Core.Packaging;
using Xunit;

namespace Cake.Core.Tests.Unit.Packaging
{
    public sealed class PackageReferenceTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Package_Not_Provided_In_Uri()
            {
                // Given, When
                var result = Record.Exception(() => new PackageReference("nuget:?version=1.2.3"));

                // Then
                AssertEx.IsArgumentException(result, "uri", "Query string parameter 'package' is missing in package reference.");
            }
        }

        public sealed class TheSchemeProperty
        {
            [Theory]
            [InlineData("nuget:https://nuget.org/?package=Cake.Foo&version=1.2.3", "nuget")]
            [InlineData("nuget:?package=Cake.Foo&version=1.2.3", "nuget")]
            [InlineData("https://cake.com/?package=Cake.Foo&version=1.2.3", "https")]
            public void Should_Return_Correct_Scheme(string uri, string expected)
            {
                // Given
                var package = new PackageReference(uri);

                // When
                var result = package.Scheme;

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class TheAddressProperty
        {
            [Fact]
            public void Should_Return_Null_Address_If_None_Is_Specified()
            {
                // Given
                var package = new PackageReference("nuget:?package=Cake.Foo&version=1.2.3");

                // When
                var result = package.Address;

                // Then
                Assert.Null(result);
            }

            [Theory]
            [InlineData("nuget:https://nuget.org/?package=Cake.Foo&version=1.2.3", "https://nuget.org/")]
            [InlineData("nuget:https://user:pass@myget.org/f/Cake?package=Cake.Foo&version=1.2.3", "https://user:pass@myget.org/f/Cake")]
            public void Should_Return_Correct_Address(string uri, string expected)
            {
                // Given
                var package = new PackageReference(uri);

                // When
                var result = package.Address;

                // Then
                Assert.Equal(expected, result.AbsoluteUri);
            }
        }

        public sealed class TheParametersProperty
        {
            [Theory]
            [InlineData("nuget:?package=Cake.Foo&version=1.2.3")]
            [InlineData("nuget:https://nuget.org/?package=Cake.Foo&version=1.2.3")]
            [InlineData("nuget:https://user:pass@myget.org/f/Cake?package=Cake.Foo&version=1.2.3")]
            public void Should_Return_Correct_Address(string uri)
            {
                // Given
                var package = new PackageReference(uri);

                // When
                var result = package.Parameters;

                // Then
                Assert.Equal(2, result.Count);
                Assert.Equal("Cake.Foo", result["package"].First());
                Assert.Equal("1.2.3", result["version"].First());
            }
        }

        public sealed class ThePackageProperty
        {
            [Theory]
            [InlineData("nuget:?package=Cake.Foo")]
            [InlineData("nuget:https://foo.com/?package=Cake.Foo")]
            public void Should_Return_Package_From_Query_String(string uri)
            {
                // Given
                var package = new PackageReference(uri);

                // When
                var result = package.Package;

                // Then
                Assert.Equal("Cake.Foo", result);
            }
        }

        public sealed class TheOriginalStringProperty
        {
            [Theory]
            [InlineData("nuget:?package=Cake.Foo")]
            [InlineData("nuget:https://foo.com/?package=Cake.Foo")]
            public void Should_Return_Uri_Provided_To_Constructor(string uri)
            {
                // Given
                var package = new PackageReference(uri);

                // When
                var result = package.OriginalString;

                // Then
                Assert.Equal(uri, result);
            }
        }
    }
}