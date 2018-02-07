// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Autofac;
using Autofac.Builder;
using Cake.Core.Composition;

namespace Cake.Composition
{
    internal sealed class ContainerRegistrar : ICakeContainerRegistrar
    {
        private readonly ContainerBuilder _builder;

        public ContainerRegistrar()
        {
            _builder = new ContainerBuilder();
        }

        public void RegisterModule(ICakeModule module)
        {
            module.Register(this);
        }

        public ICakeRegistrationBuilder RegisterType(Type type)
        {
            var registration = _builder.RegisterType(type);
            return new ContainerRegistrationBuilder<object, ConcreteReflectionActivatorData>(registration);
        }

        public ICakeRegistrationBuilder RegisterInstance<TImplementationType>(TImplementationType instance)
            where TImplementationType : class
        {
            var registration = _builder.RegisterInstance(instance);
            return new ContainerRegistrationBuilder<TImplementationType, SimpleActivatorData>(registration);
        }

        public IContainer Build()
        {
            return _builder.Build();
        }

        public void Update(IContainer container)
        {
#pragma warning disable CS0618
            _builder.Update(container);
#pragma warning restore CS0618
        }
    }
}