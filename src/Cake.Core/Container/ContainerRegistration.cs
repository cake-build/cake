using System;

namespace Cake.Core.Container
{
    /// <summary>
    /// Represents a container registration.
    /// </summary>
    public abstract class ContainerRegistration
    {
        private readonly Type _registrationType;
        private readonly Lifetime _lifetime;

        /// <summary>
        /// Gets the type of the registration.
        /// </summary>
        /// <value>The type of the registration.</value>
        public Type RegistrationType
        {
            get { return _registrationType; }
        }

        /// <summary>
        /// Gets the lifetime of the registration.
        /// </summary>
        /// <value>The lifetime.</value>
        public Lifetime Lifetime
        {
            get { return _lifetime; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerRegistration"/> class.
        /// </summary>
        /// <param name="registrationType">The registration type.</param>
        /// <param name="lifetime">The lifetime.</param>
        protected ContainerRegistration(Type registrationType, Lifetime lifetime)
        {
            _lifetime = lifetime;
            _registrationType = registrationType;
        }
    }
}