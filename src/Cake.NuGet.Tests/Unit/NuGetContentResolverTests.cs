// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Core.Polyfill;
using Cake.NuGet.Tests.Fixtures;
using Cake.Testing;
using Cake.Testing.Xunit;
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
                Assert.Contains(files, p => p.Path.FullPath == "/Working/tools/Foo/Foo.exe");
                Assert.Contains(files, p => p.Path.FullPath == "/Working/tools/Foo/Bar/Qux.pdb");
                Assert.Contains(files, p => p.Path.FullPath == "/Working/tools/Foo/Foo.XML");
            }
        }

        public sealed class Addins
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var fixture = new NuGetAddinContentResolverFixture(".NETStandard,Version=v1.6", Runtime.CoreClr);
                fixture.Path = null;

                // When
                var result = Record.Exception(() => fixture.GetFiles());

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Theory]
            [InlineData(".NETStandard,Version=v1.6", Runtime.CoreClr, "netstandard1.6")]
            [InlineData(".NETFramework,Version=v4.6.1", Runtime.Clr, "net461")]
            public void Should_Return_Exact_Framework_If_Possible(string framework, Runtime runtime, string expected)
            {
                // Given
                var fixture = new NuGetAddinContentResolverFixture(framework, runtime);
                fixture.CreateCLRAssembly("/Working/lib/net45/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/net451/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/net452/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/net46/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/net461/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/net462/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/netstandard1.5/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/netstandard1.6/file.dll");

                // When
                var result = fixture.GetFiles();

                // Then
                Assert.Equal(1, result.Count);
                Assert.Equal($"/Working/lib/{expected}/file.dll", result.ElementAt(0).Path.FullPath);
            }

            [Theory]
            [InlineData(".NETStandard,Version=v2.0", Runtime.CoreClr, "netstandard1.5")]
            [InlineData(".NETFramework,Version=v4.6.1", Runtime.Clr, "net452")]
            public void Should_Return_Nearest_Compatible_Framework_If_An_Exact_Match_Is_Not_Possible(string framework, Runtime runtime, string expected)
            {
                // Given
                var fixture = new NuGetAddinContentResolverFixture(framework, runtime);
                fixture.CreateCLRAssembly("/Working/lib/net45/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/net451/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/net452/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/net462/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/netstandard1.5/file.dll");

                // When
                var result = fixture.GetFiles();

                // Then
                Assert.Equal(1, result.Count);
                Assert.Equal($"/Working/lib/{expected}/file.dll", result.ElementAt(0).Path.FullPath);
            }

            [Theory]
            [InlineData(".NETStandard,Version=v2.0", Runtime.CoreClr)]
            [InlineData(".NETFramework,Version=v4.6.1", Runtime.Clr)]
            public void Should_Return_Empty_Result_If_Any_Match_Is_Not_Possible(string framework, Runtime runtime)
            {
                // Given
                var fixture = new NuGetAddinContentResolverFixture(framework, runtime);

                fixture.CreateCLRAssembly("/Working/lib/net462/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/netstandard2.2/file.dll");

                // When
                var result = fixture.GetFiles();

                // Then
                Assert.Equal(0, result.Count);
            }

            [Fact]
            public void Should_Return_Compatible_Netstandard_If_An_Exact_Match_Is_Not_Possible()
            {
                // Given
                var fixture = new NuGetAddinContentResolverFixture(".NETFramework,Version=v4.6.1", Runtime.Clr);

                fixture.CreateCLRAssembly("/Working/lib/netstandard1.0/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/netstandard1.3/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/netstandard1.6/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/netstandard2.1/file.dll");

                // When
                var result = fixture.GetFiles();

                // Then
                Assert.Equal(1, result.Count);
                Assert.Equal($"/Working/lib/netstandard1.6/file.dll", result.ElementAt(0).Path.FullPath);
            }

            [Fact]
            public void Should_Return_Only_CLR_Assemblies()
            {
                // Given
                var fixture = new NuGetAddinContentResolverFixture(".NETStandard,Version=v1.6", Runtime.CoreClr);

                fixture.CreateCLRAssembly("/Working/lib/netstandard1.6/file.dll");
                fixture.CreateNonCLRAssembly("/Working/lib/netstandard1.6/lib/native.dll");

                // When
                var result = fixture.GetFiles();

                // Then
                Assert.Equal(1, result.Count);
                Assert.Equal("/Working/lib/netstandard1.6/file.dll", result.ElementAt(0).Path.FullPath);
            }

            [Theory(Skip = "Not possible to return files from root anymore.")]
            [InlineData(".NETStandard,Version=v1.6", Runtime.CoreClr)]
            [InlineData(".NETFramework,Version=v4.6.1", Runtime.Clr)]
            public void Should_Return_Files_When_Located_In_Root(string framework, Runtime runtime)
            {
                // Given
                var fixture = new NuGetAddinContentResolverFixture(framework, runtime);

                fixture.CreateCLRAssembly("/Working/file1.dll");
                fixture.CreateCLRAssembly("/Working/file2.dll");
                fixture.CreateCLRAssembly("/Working/lib/file3.dll");

                // When
                var result = fixture.GetFiles();

                // Then
                Assert.Equal(3, result.Count);
                Assert.Equal("/Working/file1.dll", result.ElementAt(0).Path.FullPath);
                Assert.Equal("/Working/file2.dll", result.ElementAt(1).Path.FullPath);
                Assert.Equal("/Working/lib/file3.dll", result.ElementAt(2).Path.FullPath);
            }

            [Theory]
            [InlineData(".NETStandard,Version=v1.6", Runtime.CoreClr, "netstandard1.6")]
            [InlineData(".NETFramework,Version=v4.6.1", Runtime.Clr, "net461")]
            public void Should_Return_Exact_Framework_Even_Though_Files_Located_In_Root(string framework, Runtime runtime, string expected)
            {
                // Given
                var fixture = new NuGetAddinContentResolverFixture(framework, runtime);

                fixture.CreateCLRAssembly("/Working/lib/net461/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/netstandard1.6/file.dll");
                fixture.CreateCLRAssembly("/Working/lib/file.dll");
                fixture.CreateCLRAssembly("/Working/file.dll");

                // When
                var result = fixture.GetFiles();

                // Then
                Assert.Equal(1, result.Count);
                Assert.Equal($"/Working/lib/{expected}/file.dll", result.ElementAt(0).Path.FullPath);
            }

            [Theory(Skip = "Not possible to return files from root anymore.")]
            [InlineData(".NETStandard,Version=v1.6", Runtime.CoreClr)]
            [InlineData(".NETFramework,Version=v4.6", Runtime.Clr)]
            public void Should_Return_From_Root_If_No_Compatible_Framework_Found(string framework, Runtime runtime)
            {
                // Given
                var fixture = new NuGetAddinContentResolverFixture(framework, runtime);

                fixture.CreateCLRAssembly("/Working/lib/net462/file.dll");
                fixture.CreateCLRAssembly("/Working/file.dll");

                // When
                var result = fixture.GetFiles();

                // Then
                Assert.Equal(1, result.Count);
                Assert.Equal($"/Working/file.dll", result.ElementAt(0).Path.FullPath);
            }

            [Theory(Skip = "Not possible to return files from root anymore.")]
            [InlineData(".NETStandard,Version=v1.6", Runtime.CoreClr)]
            [InlineData(".NETFramework,Version=v4.6.1", Runtime.Clr)]
            public void Should_Log_Warning_For_Files_Located_In_Root(string framework, Runtime runtime)
            {
                // Given
                var fixture = new NuGetAddinContentResolverFixture(framework, runtime);

                fixture.CreateCLRAssembly("/Working/file.dll");
                fixture.CreateCLRAssembly("/Working/file2.dll");
                fixture.CreateCLRAssembly("/Working/file3.dll");

                // When
                fixture.GetFiles();

                // Then
                var entries = fixture.Log.Entries.Where(x => x.Level == LogLevel.Warning &&
                    x.Message.Equals($"Could not find any assemblies compatible with {framework} in NuGet package {fixture.Package.Package}. " +
                                     "Falling back to using root folder of NuGet package."))
                    .ToList();

                Assert.Single(entries);
            }

            [Theory]
            [InlineData(".NETStandard,Version=v1.6", Runtime.CoreClr)]
            [InlineData(".NETFramework,Version=v4.6.1", Runtime.Clr)]
            public void Should_Not_Return_Ref_Assemblies(string framework, Runtime runtime)
            {
                // Given
                var fixture = new NuGetAddinContentResolverFixture(framework, runtime);

                fixture.CreateCLRAssembly("/Working/ref/netstandard1.6/file.dll");
                fixture.CreateCLRAssembly("/Working/ref/net46/file.dll");
                fixture.FileSystem.CreateFile("/Working/lib/netstandard1.6/_._");
                fixture.FileSystem.CreateFile("/Working/lib/net46/_._");

                // When
                var result = fixture.GetFiles();

                // Then
                Assert.Equal(0, result.Count);
            }

            public void Should_Return_Runtimes_Assemblies_If_CoreCLR()
            {
                // Given
                var framework = ".NETCoreApp,Version=v2.0";
                var runtime = Runtime.CoreClr;
                var fixture = new NuGetAddinContentResolverFixture(framework, runtime);

                fixture.CreateCLRAssembly("/Working/runtimes/win/lib/netstandard1.6/file.dll");
                fixture.CreateCLRAssembly("/Working/runtimes/unix/lib/netstandard1.6/file.dll");
                fixture.FileSystem.CreateFile("/Working/lib/netstandard1.6/_._");
                fixture.FileSystem.CreateFile("/Working/lib/net46/_._");

                // When
                var result = fixture.GetFiles();

                // Then
                Assert.Equal(1, result.Count);
            }

            public void Should_Return_Native_Runtimes_Assemblies_If_CoreCLR()
            {
                // Given
                var framework = ".NETCoreApp,Version=v2.0";
                var runtime = Runtime.CoreClr;
                var fixture = new NuGetAddinContentResolverFixture(framework, runtime);

                fixture.CreateCLRAssembly("/Working/runtimes/win/native/file.dll");
                fixture.CreateCLRAssembly("/Working/runtimes/linux/native/file.so");
                fixture.CreateCLRAssembly("/Working/runtimes/osx/native/file.dylib");

                // When
                var result = fixture.GetFiles();

                // Then
                Assert.Equal(1, result.Count);
            }
        }
    }
}