// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETCORE
using System.Linq;
using Cake.NuGet.Tests.Fixtures;
using Xunit;

namespace Cake.NuGet.Tests.Unit.V3
{
    public sealed class NuGetV3ContentResolverTests
    {
        [Fact]
        public void Should_Throw_If_Path_Is_Null()
        {
            // Given
            var fixture = new NuGetV3ContentResolverFixture(".NETStandard,Version=v1.6");
            fixture.Path = null;

            // When
            var result = Record.Exception(() => fixture.GetFiles());

            // Then
            AssertEx.IsArgumentNullException(result, "path");
        }

        [Fact]
        public void Should_Return_Exact_Framework_If_Possible()
        {
            // Given
            var fixture = new NuGetV3ContentResolverFixture(".NETStandard,Version=v1.6");
            fixture.CreateCLRAssembly("/Working/lib/net45/file.dll");
            fixture.CreateCLRAssembly("/Working/lib/net451/file.dll");
            fixture.CreateCLRAssembly("/Working/lib/net452/file.dll");
            fixture.CreateCLRAssembly("/Working/lib/net461/file.dll");
            fixture.CreateCLRAssembly("/Working/lib/netstandard1.5/file.dll");
            fixture.CreateCLRAssembly("/Working/lib/netstandard1.6/file.dll");

            // When
            var result = fixture.GetFiles();

            // Then
            Assert.Equal(1, result.Count);
            Assert.Equal("/Working/lib/netstandard1.6/file.dll", result.ElementAt(0).Path.FullPath);
        }

        [Fact]
        public void Should_Return_Nearest_Compatible_Framework_If_An_Exact_Match_Is_Not_Possible()
        {
            // Given
            var fixture = new NuGetV3ContentResolverFixture(".NETStandard,Version=v1.6");
            fixture.CreateCLRAssembly("/Working/lib/net45/file.dll");
            fixture.CreateCLRAssembly("/Working/lib/net451/file.dll");
            fixture.CreateCLRAssembly("/Working/lib/net452/file.dll");
            fixture.CreateCLRAssembly("/Working/lib/net461/file.dll");
            fixture.CreateCLRAssembly("/Working/lib/netstandard1.5/file.dll");

            // When
            var result = fixture.GetFiles();

            // Then
            Assert.Equal(1, result.Count);
            Assert.Equal("/Working/lib/netstandard1.5/file.dll", result.ElementAt(0).Path.FullPath);
        }

        [Fact]
        public void Should_Return_Empty_Result_If_Any_Match_Is_Not_Possible()
        {
            // Given
            var fixture = new NuGetV3ContentResolverFixture(".NETStandard,Version=v1.6");
            fixture.CreateCLRAssembly("/Working/lib/net45/file.dll");
            fixture.CreateCLRAssembly("/Working/lib/net451/file.dll");
            fixture.CreateCLRAssembly("/Working/lib/net452/file.dll");
            fixture.CreateCLRAssembly("/Working/lib/net461/file.dll");

            // When
            var result = fixture.GetFiles();

            // Then
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void Should_Return_Only_CLR_Assemblies()
        {
            // Given
            var fixture = new NuGetV3ContentResolverFixture(".NETStandard,Version=v1.6");

            fixture.CreateCLRAssembly("/Working/lib/netstandard1.6/file.dll");
            fixture.CreateNonCLRAssembly("/Working/lib/netstandard1.6/lib/native.dll");

            // When
            var result = fixture.GetFiles();

            // Then
            Assert.Equal(1, result.Count);
            Assert.Equal("/Working/lib/netstandard1.6/file.dll", result.ElementAt(0).Path.FullPath);
        }
    }
}
#endif