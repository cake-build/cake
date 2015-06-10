using System;
using Cake.Core.Container;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="ICakeContainer"/>.
    /// </summary>
    public static class CakeContainerExtensions
    {
        /// <summary>
        /// Registers the specified type with the specified lifetime.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="lifetime">The lifetime.</param>
        public static void RegisterType<TService>(this ICakeContainer container, Lifetime lifetime = Lifetime.InstancePerDependency)
        {
            container.Register(new TypeRegistration(typeof(TService), typeof(TService), lifetime));
        }

        /// <summary>
        /// Registers the specified type <typeparamref name="TImplementation"/> 
        /// as a <typeparamref name="TService"/> with the specified lifetime.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="lifetime">The lifetime.</param>
        public static void RegisterType<TService, TImplementation>(this ICakeContainer container, Lifetime lifetime = Lifetime.InstancePerDependency)
            where TImplementation : TService
        {
            container.Register(new TypeRegistration(typeof(TService), typeof(TImplementation), lifetime));
        }

        /// <summary>
        /// Registers the specified instance with the specified lifetime.
        /// </summary>
        /// <typeparam name="TInstance">The instance type.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="instance">The instance.</param>
        public static void RegisterInstance<TInstance>(this ICakeContainer container, TInstance instance)
        {
            container.Register(new InstanceRegistration(typeof(TInstance), instance));
        }

        /// <summary>
        /// Registers the specified instance of type <typeparamref name="TInstance"/> 
        /// as a <typeparamref name="TService"/> with the specified lifetime.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <typeparam name="TInstance">The instance type.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="instance">The instance.</param>
        public static void RegisterInstance<TService, TInstance>(this ICakeContainer container, TInstance instance)
            where TInstance : TService
        {
            container.Register(new InstanceRegistration(typeof(TService), instance));
        }

        /// <summary>
        /// Registers the specified type with the specified <see cref="Func{FactoryRegistrationContext,TService}"/>.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="factory">The factory delegate.</param>
        public static void RegisterFactory<TService>(this ICakeContainer container, Func<FactoryRegistrationContext, TService> factory)
        {
            container.Register(new FactoryRegistration(typeof(TService), context => factory(context)));
        }
    }
}
