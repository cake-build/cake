// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Autofac;
using Autofac.Builder;
using Cake.Core.Composition;

namespace Cake.Frosting.Internal.Composition
{
    internal sealed class ContainerRegistrar : ICakeServices
    {
        public ContainerBuilder Builder { get; }

        public ContainerRegistrar()
            : this(null)
        {
        }

        public ContainerRegistrar(ContainerBuilder builder)
        {
            Builder = builder ?? new ContainerBuilder();
        }

        public void RegisterModule(ICakeModule module)
        {
            module.Register(this);
        }

        public ICakeRegistrationBuilder RegisterType(Type type)
        {
            var registration = Builder.RegisterType(type);
            return new ContainerRegistration<object, ConcreteReflectionActivatorData>(registration);
        }

        public ICakeRegistrationBuilder RegisterInstance<T>(T instance) where T : class
        {
            var registration = Builder.RegisterInstance(instance);
            return new ContainerRegistration<T, SimpleActivatorData>(registration);
        }

        public IContainer Build()
        {
            return Builder.Build();
        }
    }
}
