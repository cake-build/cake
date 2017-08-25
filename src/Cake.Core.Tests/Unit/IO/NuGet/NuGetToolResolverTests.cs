// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Tests.Fixtures;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Core.Tests.Unit.IO.NuGet
{
    public sealed class NuGetToolResolverTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new NuGetToolResolverFixture();
                fixture.FileSystem = null;

                // When
                var result = Record.Exception(() => fixture.Resolve());

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new NuGetToolResolverFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.Resolve());

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheResolveToolPathMethod
        {
            [Fact]
            public void Should_Throw_If_NuGet_Exe_Could_Not_Be_Found()
            {
                // Given
                var fixture = new NuGetToolResolverFixture();

                // When
                var result = Record.Exception(() => fixture.Resolve());

                // Assert
                AssertEx.IsCakeException(result, "Could not locate nuget.exe.");
            }

            [Fact]
            public void Should_Be_Able_To_Resolve_Path_From_The_Tools_Directory()
            {
                // Given
                var fixture = new NuGetToolResolverFixture();
                fixture.FileSystem.CreateFile("/Working/tools/nuget.exe");

                // When
                var result = fixture.Resolve();

                // Then
                Assert.Equal("/Working/tools/nuget.exe", result.FullPath);
            }

            [Fact]
            public void Should_Be_Able_To_Resolve_Path_Via_Environment_Path_Variable_On_Unix()
            {
                // Given
                var fixture = new NuGetToolResolverFixture();
                fixture.Environment.SetEnvironmentVariable("PATH", "/temp:/stuff/programs:/programs");
                fixture.FileSystem.CreateFile("/stuff/programs/nuget.exe");

                // When
                var result = fixture.Resolve();

                // Then
                Assert.Equal("/stuff/programs/nuget.exe", result.FullPath);
            }

            [WindowsFact]
            public void Should_Be_Able_To_Resolve_Path_Via_Environment_Path_Variable_On_Windows()
            {
                // Given
                var fixture = new NuGetToolResolverFixture(FakeEnvironment.CreateWindowsEnvironment());
                fixture.Environment.SetEnvironmentVariable("PATH", "/temp;/stuff/programs;/programs");
                fixture.FileSystem.CreateFile("/stuff/programs/nuget.exe");

                // When
                var result = fixture.Resolve();

                // Then
                Assert.Equal("/stuff/programs/nuget.exe", result.FullPath);
            }

            [Fact]
            public void Should_Be_Able_To_Resolve_Path_Via_NuGet_Environment_Variable()
            {
                // Given
                var fixture = new NuGetToolResolverFixture();
                fixture.Environment.SetEnvironmentVariable("NUGET_EXE", "/programs/nuget.exe");
                fixture.FileSystem.CreateFile("/programs/nuget.exe");

                // When
                var result = fixture.Resolve();

                // Then
                Assert.Equal("/programs/nuget.exe", result.FullPath);
            }

            [Theory]
            [InlineData("/Library/Frameworks/Mono.framework/Versions/Current/Commands/nuget")]
            [InlineData("/usr/local/bin/nuget")]
            [InlineData("/usr/bin/nuget")]
            public void Should_Be_Able_To_Resolve_Path_Via_Special_Unix_Paths(string path)
            {
                // Given
                var fixture = new NuGetToolResolverFixture();
                fixture.FileSystem.CreateFile(path);

                // When
                var result = fixture.Resolve();

                // Then
                Assert.Equal(path, result.FullPath);
            }
        }
    }
}