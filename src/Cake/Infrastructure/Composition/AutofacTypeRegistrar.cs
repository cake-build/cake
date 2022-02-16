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
    public sealed class AutofacTypeRegistrar : ITypeRegistrar, ICakeContainerRegistrar
    {
        private readonly ContainerBuilder _builder;

        public AutofacTypeRegistrar(ContainerBuilder builder)
        {
            _builder = builder;
        }

        public void RegisterModule(ICakeModule module)
        {
            module.Register(this);
        }

        public void Register(Type service, Type implementation)
        {
            _builder.RegisterType(implementation).As(service).SingleInstance();
        }

        public ICakeRegistrationBuilder RegisterType(Type type)
        {
            var registration = _builder.RegisterType(type);
            return new ContainerRegistrationBuilder<object, ConcreteReflectionActivatorData>(registration);
        }

        public void RegisterInstance(Type service, object implementation)
        {
            _builder.RegisterInstance(implementation).As(service).SingleInstance();
        }

        public void RegisterLazy(Type service, Func<object> factory)
        {
            _builder.Register(_ => factory()).As(service);
        }

        public ICakeRegistrationBuilder RegisterInstance<TImplementation>(TImplementation instance)
            where TImplementation : class
        {
            var registration = _builder.RegisterInstance(instance);
            return new ContainerRegistrationBuilder<TImplementation, SimpleActivatorData>(registration);
        }

        public IContainer BuildContainer()
        {
            return _builder.Build();
        }

        public ITypeResolver Build()
        {
            return new AutofacTypeResolver(BuildContainer());
        }
    }
}
