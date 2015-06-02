using System;

namespace Cake.Core.Container
{
    /// <summary>
    /// Represents an instance registration.
    /// </summary>
    public sealed class InstanceRegistration : ContainerRegistration
    {
        private readonly object _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public object Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceRegistration"/> class.
        /// </summary>
        /// <param name="registrationType">The registration type.</param>
        /// <param name="instance">The instance to register.</param>
        public InstanceRegistration(Type registrationType, object instance)
            : base(registrationType, Lifetime.InstancePerDependency)
        {
            _instance = instance;
        }
    }
}