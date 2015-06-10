using Cake.Core.Container;

namespace Cake.Core
{
    /// <summary>
    /// Represents a module bootstrapper.
    /// </summary>
    public interface ICakeContainer
    {
        /// <summary>
        /// Registers the specified registration.
        /// </summary>
        /// <param name="registration">The registration.</param>
        void Register(ContainerRegistration registration);
    }
}