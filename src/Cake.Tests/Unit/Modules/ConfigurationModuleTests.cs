// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Composition;
using Cake.Core.Configuration;
using Cake.Core.IO;
using Cake.Modules;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit.Modules
{
    public sealed class ConfigurationModuleTests
    {
        public sealed class TheRegisterMethod
        {
            [Fact]
            public void Should_Use_The_Script_Directory_As_Root_For_Configuration_File()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var provider = new CakeConfigurationProvider(fileSystem, environment);
                var registry = new ContainerRegistry();
                var options = new CakeOptions { Script = "./foo/bar/build.cake" };
                var module = new ConfigurationModule(provider, options);

                // When
                module.Register(registry);

                // Then
                fileSystem.Received(1).Exist(Arg.Is<FilePath>(f => f.FullPath == "/Working/foo/bar/cake.config"));
            }
        }
    }
}