// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.NuGet.Tests.Unit
{
    public class NuGetAssemblyCompatibilityFilterTests
    {
        public sealed class TheFilterCompatibleAssembliesMethod
        {
            private static readonly FrameworkName _dummyFrameworkName = new FrameworkName(".NETFramework,Version=v4.5.1");

            [Fact]
            public void Should_Throw_If_Target_Framework_Is_Null()
            {
                // Given

                var filter = new NuGetAssemblyCompatibilityFilter(Substitute.For<INuGetFrameworkCompatibilityFilter>(),
                    Substitute.For<INuGetPackageReferenceBundler>());

                // When
                // ReSharper disable once ExpressionIsAlwaysNull
                var result = Record.Exception(() => filter.FilterCompatibleAssemblies(null, new FilePath[0]));

                // Then
                Assert.IsArgumentNullException(result, "targetFramework");
            }

            [Fact]
            public void Should_Throw_If_Assembly_Paths_Is_Null()
            {
                // Given
                var filter = new NuGetAssemblyCompatibilityFilter(Substitute.For<INuGetFrameworkCompatibilityFilter>(),
                    Substitute.For<INuGetPackageReferenceBundler>());

                // When
                var result = Record.Exception(() => filter.FilterCompatibleAssemblies(_dummyFrameworkName, null));

                // Then
                Assert.IsArgumentNullException(result, "assemblyPaths");
            }

            [Fact]
            public void Should_Return_Empty_When_No_References_Are_Compatible()
            {
                // Given
                var targetFramework = _dummyFrameworkName;

                var compatibilityFilter = Substitute.For<INuGetFrameworkCompatibilityFilter>();

                compatibilityFilter.GetCompatibleItems(targetFramework, Arg.Any<IEnumerable<NuGetPackageReferenceSet>>())
                    .Returns(Enumerable.Empty<NuGetPackageReferenceSet>());

                var referenceSetFactory = Substitute.For<INuGetPackageReferenceBundler>();

                var assemblyFiles = new FilePath[] { "dummy.dll", "dummy2.dll" };
                var filter = new NuGetAssemblyCompatibilityFilter(compatibilityFilter,
                    referenceSetFactory);

                // When
                var result = filter.FilterCompatibleAssemblies(targetFramework, assemblyFiles);

                // Then
                Assert.Empty(result);
            }

            [Fact]
            public void Should_Throw_If_Any_Assembly_Path_Is_Not_Relative()
            {
                // Given
                var compatibilityFilter = Substitute.For<INuGetFrameworkCompatibilityFilter>();
                var referenceSetFactory = Substitute.For<INuGetPackageReferenceBundler>();

                var assemblyFiles = new FilePath[] { "/dir/dummy.dll", "dummy2.dll" };
                var filter = new NuGetAssemblyCompatibilityFilter(compatibilityFilter,
                    referenceSetFactory);

                // When
                var result = Record.Exception(() => filter.FilterCompatibleAssemblies(_dummyFrameworkName, assemblyFiles));

                // Then
                Assert.IsCakeException(result, "All assemblyPaths must be relative to the package directory.");
            }

            [Fact]
            public void Should_Return_Compatible_References()
            {
                // TODO: this is a pretty bad test -- some refactoring of the SUT implementation should simplify this

                // Given
                var targetFramework = new FrameworkName(".NETFramework,Version=v4.0");

                var compatibleAssemblies = new FilePath[]
                { "lib/net40/compatible1.dll", "lib/net40/compatible2.dll", "compatible3.dll" };
                var allAssemblies =
                    compatibleAssemblies.Concat(new FilePath[]
                    { "lib/net452/incompatible1.dll", "lib/net452/incompatible2.dll" }).ToArray();

                var compatibilityFilter = Substitute.For<INuGetFrameworkCompatibilityFilter>();

                compatibilityFilter.GetCompatibleItems(targetFramework, Arg.Any<IEnumerable<NuGetPackageReferenceSet>>())
                    .Returns(args => new[]
                    {
                        new NuGetPackageReferenceSet(args.Arg<FrameworkName>(),
                            compatibleAssemblies)
                    });

                var referenceSetFactory = Substitute.For<INuGetPackageReferenceBundler>();

                var filter = new NuGetAssemblyCompatibilityFilter(compatibilityFilter,
                    referenceSetFactory);

                // When
                var result = filter.FilterCompatibleAssemblies(targetFramework, allAssemblies);

                // Then
                Assert.Equal(compatibleAssemblies.Select(ca => ca.FullPath).ToArray(),
                    result.Select(ca => ca.FullPath).ToArray());
            }
        }
    }
}
