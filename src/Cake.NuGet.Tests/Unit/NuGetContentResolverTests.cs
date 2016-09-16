// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Cake.NuGet.Tests.Fixtures;
using Cake.Testing;
using Xunit;

namespace Cake.NuGet.Tests.Unit
{
    public sealed class NuGetContentResolverTests
    {
        public sealed class Tools
        {
            [Fact]
            public void Should_Return_Exe_Files_By_Default()
            {
                // Given
                var fixture = new NuGetToolContentResolverFixture("nuget:?package=Foo");
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Foo.exe");
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Foo.pdb");
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Foo.XML");

                // When
                var files = fixture.GetFiles();

                // Then
                Assert.Equal(1, files.Count);
                Assert.Equal("/Working/tools/Foo/Foo.exe", files.First().Path.FullPath);
            }

            [Fact]
            public void Should_Return_Dll_Files_By_Default()
            {
                // Given
                var fixture = new NuGetToolContentResolverFixture("nuget:?package=Foo");
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Foo.dll");
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Foo.pdb");
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Foo.XML");

                // When
                var files = fixture.GetFiles();

                // Then
                Assert.Equal(1, files.Count);
                Assert.Equal("/Working/tools/Foo/Foo.dll", files.First().Path.FullPath);
            }

            [Theory]
            [InlineData("nuget:?package=Foo&include=/**/*.XML")]
            [InlineData("nuget:?package=Foo&include=**/*.XML")]
            [InlineData("nuget:?package=Foo&include=./**/*.XML")]
            public void Should_Return_Included_Files_If_Specified(string package)
            {
                // Given
                var fixture = new NuGetToolContentResolverFixture(package);
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Foo.dll");
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Foo.pdb");
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Foo.XML");

                // When
                var files = fixture.GetFiles();

                // Then
                Assert.Equal(2, files.Count);
                Assert.Equal("/Working/tools/Foo/Foo.dll", files.ElementAt(0).Path.FullPath);
                Assert.Equal("/Working/tools/Foo/Foo.XML", files.ElementAt(1).Path.FullPath);
            }

            [Theory]
            [InlineData("nuget:?package=Foo&exclude=/**/*.dll")]
            [InlineData("nuget:?package=Foo&exclude=**/*.dll")]
            [InlineData("nuget:?package=Foo&exclude=./**/*.dll")]
            public void Should_Excluded_Files_If_Specified(string package)
            {
                // Given
                var fixture = new NuGetToolContentResolverFixture(package);
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Foo.dll");
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Foo.pdb");
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Foo.XML");

                // When
                var files = fixture.GetFiles();

                // Then
                Assert.Equal(0, files.Count);
            }

            [Fact]
            public void Should_Include_And_Exclude_Files_If_Specified()
            {
                // Given
                var fixture = new NuGetToolContentResolverFixture("nuget:?package=Foo&exclude=./**/*.dll&include=./**/*.XML&include=Bar/Qux.pdb");
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Foo.exe");
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Foo.dll");
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Foo.pdb");
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Bar/Qux.pdb");
                fixture.FileSystem.CreateFile("/Working/tools/Foo/Foo.XML");

                // When
                var files = fixture.GetFiles();

                // Then
                Assert.Equal(3, files.Count);
                Assert.True(files.Any(p => p.Path.FullPath == "/Working/tools/Foo/Foo.exe"));
                Assert.True(files.Any(p => p.Path.FullPath == "/Working/tools/Foo/Bar/Qux.pdb"));
                Assert.True(files.Any(p => p.Path.FullPath == "/Working/tools/Foo/Foo.XML"));
            }
        }
    }
}