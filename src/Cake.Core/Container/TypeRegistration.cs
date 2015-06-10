using System;

namespace Cake.Core.Container
{
    /// <summary>
    /// Represents a type registration.
    /// </summary>
    public sealed class TypeRegistration : ContainerRegistration
    {
        private readonly Type _implementationType;

        /// <summary>
        /// Gets the implementing type.
        /// </summary>
        /// <value>The type of the implementation.</value>
        public Type ImplementationType
        {
            get { return _implementationType; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRegistration"/> class.
        /// </summary>
        /// <param name="registrationType">The registration type.</param>
        /// <param name="lifetime">The lifetime.</param>
        public TypeRegistration(Type registrationType, Lifetime lifetime)
            : this(registrationType, registrationType, lifetime)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRegistration"/> class.
        /// </summary>
        /// <param name="registrationType">The registration type.</param>
        /// <param name="implementationType">The implementation type.</param>
        /// <param name="lifetime">The lifetime.</param>
        public TypeRegistration(Type registrationType, Type implementationType, Lifetime lifetime)
            : base(registrationType, lifetime)
        {
            if (!registrationType.IsAssignableFrom(implementationType))
            {
                const string format = "The type '{0}' is not assignable from '{1}'.";
                throw new InvalidOperationException(string.Format(format, implementationType.FullName, registrationType.FullName));
            }
            _implementationType = implementationType;
        }
    }
}