// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Cake.Core.IO;
using Cake.Core.Polyfill;
using Xunit;

namespace Cake.Core.Tests.Unit.Polyfill;

public sealed class AssemblyPathResolverTests
{
    public sealed class TheResolveFilePathMethod
    {
        [Fact]
        public void Should_Prefer_Assembly_Location_When_Present()
        {
            // Given
            const string location = "/app/Cake.dll";
            const string processPath = "/app/Cake.exe";

            // When
            var result = AssemblyPathResolver.ResolveFilePath(location, processPath);

            // Then
            Assert.Equal(location, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_Use_Process_Path_When_Location_Is_Empty(string location)
        {
            // Given
            const string processPath = "/app/Cake.exe";

            // When
            var result = AssemblyPathResolver.ResolveFilePath(location, processPath);

            // Then
            Assert.Equal(processPath, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("   ", "   ")]
        public void Should_Return_Null_When_Both_Are_Empty(string location, string processPath)
        {
            // When
            var result = AssemblyPathResolver.ResolveFilePath(location, processPath);

            // Then
            Assert.Null(result);
        }
    }

    public sealed class TheResolveDirectoryMethod
    {
        [Fact]
        public void Should_Return_Directory_Of_Assembly_Location()
        {
            // Given
            var location = new FilePath("/app/Cake.dll");

            // When
            var result = AssemblyPathResolver.ResolveDirectory(location.FullPath, "/other/Cake.exe", "/fallback");

            // Then
            Assert.Equal(location.GetDirectory().FullPath, result);
        }

        [Fact]
        public void Should_Return_Directory_Of_Process_Path_When_Location_Is_Empty()
        {
            // Given
            var processPath = new FilePath("/app/Cake.exe");

            // When
            var result = AssemblyPathResolver.ResolveDirectory(string.Empty, processPath.FullPath, "/fallback");

            // Then
            Assert.Equal(processPath.GetDirectory().FullPath, result);
        }

        [Fact]
        public void Should_Return_Base_Directory_When_File_Paths_Are_Empty()
        {
            // Given
            const string baseDirectory = "/Working/bin";

            // When
            var result = AssemblyPathResolver.ResolveDirectory(string.Empty, null, baseDirectory);

            // Then
            Assert.Equal(baseDirectory, result);
        }

        [Fact]
        public void Should_Return_Null_When_All_Sources_Are_Empty()
        {
            // When
            var result = AssemblyPathResolver.ResolveDirectory(null, null, "   ");

            // Then
            Assert.Null(result);
        }
    }

    public sealed class TheGetAssemblyFilePathMethod
    {
        [Fact]
        public void Should_Return_Location_For_Loaded_Assembly()
        {
            // Given
            var assembly = typeof(AssemblyPathResolver).Assembly;

            // When
            var result = AssemblyPathResolver.GetAssemblyFilePath(assembly);

            // Then
            Assert.False(string.IsNullOrWhiteSpace(result));
            Assert.Equal(assembly.Location, result);
        }

        [Fact]
        public void Should_Fall_Back_To_Process_Path_When_Assembly_Is_Null()
        {
            // When
            var result = AssemblyPathResolver.GetAssemblyFilePath(null);

            // Then
            Assert.Equal(System.Environment.ProcessPath, result);
        }
    }

    public sealed class TheGetAssemblyDirectoryMethod
    {
        [Fact]
        public void Should_Return_Directory_For_Loaded_Assembly()
        {
            // Given
            var assembly = typeof(AssemblyPathResolver).Assembly;
            var fallback = new DirectoryPath("/Working/bin");

            // When
            var result = AssemblyPathResolver.GetAssemblyDirectory(assembly, fallback);

            // Then
            Assert.Equal(new FilePath(assembly.Location).GetDirectory().FullPath, result.FullPath);
        }
    }
}
