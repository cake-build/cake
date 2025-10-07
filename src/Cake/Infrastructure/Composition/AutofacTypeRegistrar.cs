// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Autofac;
using Autofac.Builder;
using Cake.Core.Composition;
using Spectre.Console.Cli;

namespace Cake.Infrastructure.Composition
{
    /// <summary>
    /// Represents an Autofac-based type registrar.
    /// </summary>
    public sealed class AutofacTypeRegistrar : ITypeRegistrar, ICakeContainerRegistrar
    {
        private readonly ContainerBuilder _builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacTypeRegistrar"/> class.
        /// </summary>
        /// <param name="builder">The container builder.</param>
        public AutofacTypeRegistrar(ContainerBuilder builder)
        {
            _builder = builder;
        }

        /// <summary>
        /// Registers a Cake module.
        /// </summary>
        /// <param name="module">The module to register.</param>
        public void RegisterModule(ICakeModule module)
        {
            module.Register(this);
        }

        /// <summary>
        /// Registers a service with its implementation.
        /// </summary>
        /// <param name="service">The service type.</param>
        /// <param name="implementation">The implementation type.</param>
        public void Register(Type service, Type implementation)
        {
            _builder.RegisterType(implementation).As(service).SingleInstance();
        }

        /// <summary>
        /// Registers a type for dependency injection.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <returns>A registration builder.</returns>
        public ICakeRegistrationBuilder RegisterType(Type type)
        {
            var registration = _builder.RegisterType(type);
            return new ContainerRegistrationBuilder<object, ConcreteReflectionActivatorData>(registration);
        }

        /// <summary>
        /// Registers an instance for a service.
        /// </summary>
        /// <param name="service">The service type.</param>
        /// <param name="implementation">The implementation instance.</param>
        public void RegisterInstance(Type service, object implementation)
        {
            _builder.RegisterInstance(implementation).As(service).SingleInstance();
        }

        /// <summary>
        /// Registers a lazy factory for a service.
        /// </summary>
        /// <param name="service">The service type.</param>
        /// <param name="factory">The factory function.</param>
        public void RegisterLazy(Type service, Func<object> factory)
        {
            _builder.Register(_ => factory()).As(service);
        }

        /// <summary>
        /// Registers an instance for a service.
        /// </summary>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        /// <param name="instance">The instance to register.</param>
        /// <returns>A registration builder.</returns>
        public ICakeRegistrationBuilder RegisterInstance<TImplementation>(TImplementation instance)
            where TImplementation : class
        {
            var registration = _builder.RegisterInstance(instance);
            return new ContainerRegistrationBuilder<TImplementation, SimpleActivatorData>(registration);
        }

        /// <summary>
        /// Builds the container.
        /// </summary>
        /// <returns>The built container.</returns>
        public IContainer BuildContainer()
        {
            return _builder.Build();
        }

        /// <summary>
        /// Builds the type resolver.
        /// </summary>
        /// <returns>The type resolver.</returns>
        public ITypeResolver Build()
        {
            return new AutofacTypeResolver(BuildContainer());
        }
    }
}
