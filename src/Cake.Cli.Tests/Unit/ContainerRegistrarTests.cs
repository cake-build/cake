// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.Composition;
using Cake.Core.Diagnostics;
using Cake.Core.Modules;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace Cake.Cli.Tests.Unit;

public sealed class ContainerRegistrarTests
{
    public sealed class TheRegisterTypeMethod
    {
        [Fact]
        public void Should_Register_All_As_Service_Types()
        {
            // Given
            var services = new ServiceCollection();
            var registrar = new ContainerRegistrar(services);

            // When
            registrar.RegisterType<SharedService>().As<IFoo>().As<IBar>().Singleton();
            using var provider = registrar.BuildServiceProvider();

            // Then
            var foo = provider.GetRequiredService<IFoo>();
            var bar = provider.GetRequiredService<IBar>();
            Assert.IsType<SharedService>(foo);
            Assert.IsType<SharedService>(bar);
            Assert.Same(foo, bar);
        }

        [Fact]
        public void Should_Register_Implementation_When_As_Is_Not_Specified()
        {
            // Given
            var services = new ServiceCollection();
            var registrar = new ContainerRegistrar(services);

            // When
            registrar.RegisterType<SharedService>().Singleton();
            using var provider = registrar.BuildServiceProvider();

            // Then
            Assert.IsType<SharedService>(provider.GetRequiredService<SharedService>());
        }
    }

    public sealed class TheRegisterInstanceMethod
    {
        [Fact]
        public void Should_Register_Instance_For_All_As_Service_Types()
        {
            // Given
            var services = new ServiceCollection();
            var registrar = new ContainerRegistrar(services);
            var instance = new SharedService();

            // When
            registrar.RegisterInstance(instance).As<IFoo>().As<IBar>();
            using var provider = registrar.BuildServiceProvider();

            // Then
            Assert.Same(instance, provider.GetRequiredService<IFoo>());
            Assert.Same(instance, provider.GetRequiredService<IBar>());
        }
    }

    public sealed class TheUseModuleMethod
    {
        [Fact]
        public void Should_Transfer_Module_Registrations_Into_The_Service_Collection()
        {
            // Given
            var services = new ServiceCollection();

            // When
            services.UseModule<TestModule>();
            using var provider = services.BuildServiceProvider();

            // Then
            var foo = provider.GetRequiredService<IFoo>();
            var bar = provider.GetRequiredService<IBar>();
            Assert.Same(foo, bar);
        }

        [Fact]
        public void Should_Share_CakeDataService_Across_Resolver_And_Service()
        {
            // Given
            var services = new ServiceCollection();

            // When
            services.UseModule<CoreModule>();
            using var provider = services.BuildServiceProvider();

            // Then
            var resolver = provider.GetRequiredService<ICakeDataResolver>();
            var dataService = provider.GetRequiredService<ICakeDataService>();
            Assert.Same(resolver, dataService);
        }
    }

    public sealed class TheAddCakeDiagnosticsMethod
    {
        [Fact]
        public void Should_Register_Spectre_Report_Printer()
        {
            // Given
            var services = new ServiceCollection();

            // When
            services.AddCakeDiagnostics();
            using var provider = services.BuildServiceProvider();

            // Then
            Assert.NotNull(provider.GetRequiredService<IAnsiConsole>());
            Assert.IsType<CakeSpectreReportPrinter>(provider.GetRequiredService<ICakeReportPrinter>());
        }

        [Fact]
        public void Should_Register_Log_And_Console()
        {
            // Given
            var services = new ServiceCollection();
            var registrar = new ContainerRegistrar(services);

            // When
            registrar.AddCakeDiagnostics();
            registrar.Transfer();

            // Then
            Assert.Contains(services, descriptor =>
                descriptor.ServiceType == typeof(ICakeLog) &&
                descriptor.ImplementationType == typeof(CakeBuildLog));
            Assert.Contains(services, descriptor =>
                descriptor.ServiceType == typeof(IConsole) &&
                descriptor.ImplementationType == typeof(CakeConsole));
        }
    }

    public sealed class TheUseCakeDefaultModulesMethod
    {
        [Fact]
        public void Should_Share_CakeDataService_Across_Resolver_And_Service()
        {
            // Given
            var services = new ServiceCollection();

            // When
            services.UseCakeDefaultModules();
            using var provider = services.BuildServiceProvider();

            // Then
            var resolver = provider.GetRequiredService<ICakeDataResolver>();
            var dataService = provider.GetRequiredService<ICakeDataService>();
            Assert.Same(resolver, dataService);
        }

        [Fact]
        public void Should_Register_Modules_On_The_Registrar()
        {
            // Given
            var services = new ServiceCollection();
            var registrar = new ContainerRegistrar(services);

            // When
            registrar.UseCakeDefaultModules();
            using var provider = registrar.BuildServiceProvider();

            // Then
            var resolver = provider.GetRequiredService<ICakeDataResolver>();
            var dataService = provider.GetRequiredService<ICakeDataService>();
            Assert.Same(resolver, dataService);
        }
    }

    public interface IFoo
    {
    }

    public interface IBar
    {
    }

    public sealed class SharedService : IFoo, IBar
    {
    }

    public sealed class TestModule : ICakeModule
    {
        public void Register(ICakeContainerRegistrar registrar)
        {
            registrar.RegisterType<SharedService>().As<IFoo>().As<IBar>().Singleton();
        }
    }
}
