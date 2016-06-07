namespace Cake.Core.Composition
{
    /// <summary>
    /// Represents a module responsible for
    /// registering types and instances.
    /// </summary>
    public interface ICakeModule
    {
        /// <summary>
        /// Performs custom registrations in the provided registry.
        /// </summary>
        /// <param name="registry">The container registry.</param>
        void Register(ICakeContainerRegistry registry);
    }
}
