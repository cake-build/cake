// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.Configuration;
using Cake.Testing;
using Xunit;

namespace Cake.NuGet.Tests.Unit
{
    public sealed class NuGetConfigPathResolverTests
    {
        [Fact]
        public void Should_Use_Working_Directory_When_Config_File_Not_Specified()
        {
            // Given
            var environment = FakeEnvironment.CreateUnixEnvironment();
            environment.WorkingDirectory = "/project/src";
            var fileSystem = new FakeFileSystem(environment);
            var config = new CakeConfiguration(new Dictionary<string, string>());

            // When
            var result = NuGetConfigPathResolver.GetPath(environment, config, fileSystem);

            // Then
            Assert.Equal("/project/src", result.Item1.FullPath);
            Assert.Null(result.Item2);
        }

        [Fact]
        public void Should_Use_Explicit_Config_File_When_Specified()
        {
            // Given
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            var configPath = environment.WorkingDirectory.CombineWithFilePath("NuGet.config");
            fileSystem.CreateFile(configPath).Dispose();
            var config = new CakeConfiguration(new Dictionary<string, string>
            {
                [Constants.NuGet.ConfigFile] = "./NuGet.config",
            });

            // When
            var result = NuGetConfigPathResolver.GetPath(environment, config, fileSystem);

            // Then
            Assert.Equal("/Working", result.Item1.FullPath);
            Assert.Equal("NuGet.config", result.Item2.FullPath);
        }
    }
}
