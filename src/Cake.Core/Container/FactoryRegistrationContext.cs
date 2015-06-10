using System;

namespace Cake.Core.Container
{
    /// <summary>
    /// The factory registration context.
    /// </summary>
    public sealed class FactoryRegistrationContext
    {
        private readonly Func<Type, object> _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryRegistrationContext"/> class.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        public FactoryRegistrationContext(Func<Type, object> resolver)
        {
            _resolver = resolver;
        }

        /// <summary>
        /// Resolves an instance from the container.
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <returns>The resolved instance.</returns>
        public T Resolve<T>()
        {
            return (T)_resolver(typeof(T));
        }
    }
}