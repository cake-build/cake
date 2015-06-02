using System;
using Autofac;
using Cake.Core;
using Cake.Core.Container;

namespace Cake.Autofac
{
    internal sealed class AutofacCakeContainer : ICakeContainer
    {
        private readonly ContainerBuilder _builder;

        public AutofacCakeContainer()
        {
            _builder = new ContainerBuilder();
        }

        public void Register(ContainerRegistration registration)
        {
            var typeRegistration = registration as TypeRegistration;
            if (typeRegistration != null)
            {
                RegisterType(typeRegistration);
            }
            else
            {
                var instanceRegistration = registration as InstanceRegistration;
                if (instanceRegistration != null)
                {
                    RegisterInstance(instanceRegistration);
                }
                else
                {
                    var factoryRegistration = registration as FactoryRegistration;
                    if (factoryRegistration != null)
                    {
                        RegisterFactory(factoryRegistration);
                    }
                    else
                    {
                        throw new NotSupportedException("Registration is not supported.");
                    }
                }
            }
        }

        public void Update(IContainer container)
        {
            _builder.Update(container);
        }

        private void RegisterType(TypeRegistration registration)
        {
            var autofacRegistration = _builder.RegisterType(registration.ImplementationType).As(registration.RegistrationType);
            if (registration.Lifetime == Lifetime.Singleton)
            {
                autofacRegistration.SingleInstance();
            }
        }

        private void RegisterInstance(InstanceRegistration registration)
        {
            _builder.RegisterInstance(registration.Instance).As(registration.RegistrationType).AsSelf();
        }

        private void RegisterFactory(FactoryRegistration registration)
        {
            _builder.Register(c => registration.Factory(new FactoryRegistrationContext(c.Resolve)))
                .As(registration.RegistrationType);
        }
    }
}
