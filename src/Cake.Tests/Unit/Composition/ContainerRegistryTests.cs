// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Composition;
using Cake.Core.Composition;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit.Composition
{
    public sealed class ContainerRegistryTests
    {
        public sealed class TheRegisterModuleMethod
        {
            [Fact]
            public void Should_Call_The_Modules_Register_Method()
            {
                // Given
                var module = Substitute.For<ICakeModule>();
                var registry = new ContainerRegistry();

                // When
                registry.RegisterModule(module);

                // Then
                module.Received(1).Register(
                    Arg.Is<ICakeContainerRegistry>(registry));
            }
        }

        public sealed class TheRegisterTypeMethod
        {
            [Fact]
            public void Should_Create_Type_Registration_As_Singleton_By_Default()
            {
                // Given
                var registry = new ContainerRegistry();

                // When
                registry.RegisterType<CakeConsole>();

                // Then
                Assert.Equal(1, registry.Registrations.Count);
                Assert.Equal(typeof(CakeConsole), registry.Registrations[0].ImplementationType);
                Assert.Null(registry.Registrations[0].Instance);
                Assert.True(registry.Registrations[0].IsSingleton);
            }
        }

        public sealed class TheRegisterInstanceMethod
        {
            [Fact]
            public void Should_Create_Instance_Registration_As_Singleton_By_Default()
            {
                // Given
                var registry = new ContainerRegistry();
                var instance = new CakeConsole();

                // When
                registry.RegisterInstance(instance);

                // Then
                Assert.Equal(1, registry.Registrations.Count);
                Assert.Equal(typeof(CakeConsole), registry.Registrations[0].ImplementationType);
                Assert.Equal(instance, registry.Registrations[0].Instance);
                Assert.True(registry.Registrations[0].IsSingleton);
            }
        }
    }
}
