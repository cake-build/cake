// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Linq;
using System.Runtime.Versioning;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.NuGet.Tests.Unit
{
    public sealed class NuGetPackageReferenceBundlerTests
    {
        public sealed class TheParseMethod
        {
            [Fact]
            public void Should_Throw_If_Package_Assembly_Files_Is_Null()
            {
                // Given
                var parser = new NuGetPackageReferenceBundler(Substitute.For<IAssemblyFrameworkNameParser>());

                // When
                var result = Record.Exception(() => parser.BundleByTargetFramework(null));

                // Then
                Assert.IsArgumentNullException(result, "packageAssemblyFiles");
            }

            [Fact]
            public void Should_Return_Empty_If_Package_Assembly_Files_Is_Empty()
            {
                // Given
                var packageAssemblyFiles = Enumerable.Empty<FilePath>();

                var parser = new NuGetPackageReferenceBundler(Substitute.For<IAssemblyFrameworkNameParser>());

                // When
                var result = parser.BundleByTargetFramework(packageAssemblyFiles);

                // Then
                Assert.Empty(result);
            }

            [Fact]
            public void Should_Group_Assembly_Files_Into_Package_Reference_Sets_By_Target_Framework()
            {
                // Given
                var packageAssemblyFiles = new FilePath[]
                {
                    "framework1/assemblyA.dll", "framework1/assemblyB.dll", "assemblyC.dll", "framework2/assemblyA.dll",
                    "framework2/assemblyB.dll"
                };

                var pathParser = Substitute.For<IAssemblyFrameworkNameParser>();

                pathParser.Parse(Arg.Any<FilePath>()).Returns(ci =>
                {
                    var directoryName = ci.Arg<FilePath>().GetDirectory().FullPath;

                    if (string.IsNullOrEmpty(directoryName))
                    {
                        return null;
                    }

                    return new FrameworkName(directoryName, new Version());
                });

                var parser = new NuGetPackageReferenceBundler(pathParser);

                // When
                var result = parser.BundleByTargetFramework(packageAssemblyFiles).ToArray();

                // Then
                Assert.Equal(3, result.Length);

                var framework1Set =
                    result.Single(p => p.TargetFramework == new FrameworkName("framework1", new Version()));
                Assert.Equal(2, framework1Set.References.Count);
                Assert.ContainsFilePath(framework1Set.References, packageAssemblyFiles[0]);
                Assert.ContainsFilePath(framework1Set.References, packageAssemblyFiles[1]);

                var framework2Set =
                    result.Single(p => p.TargetFramework == new FrameworkName("framework2", new Version()));
                Assert.Equal(2, framework2Set.References.Count);
                Assert.ContainsFilePath(framework2Set.References, packageAssemblyFiles[3]);
                Assert.ContainsFilePath(framework2Set.References, packageAssemblyFiles[4]);

                var noFrameworkSet =
                    result.Single(p => p.TargetFramework == null);
                Assert.Equal(1, noFrameworkSet.References.Count);
                Assert.ContainsFilePath(noFrameworkSet.References, packageAssemblyFiles[2]);
            }
        }
    }
}
