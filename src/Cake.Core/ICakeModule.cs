namespace Cake.Core
{
    /// <summary>
    /// Represents a Cake module.
    /// </summary>
    public interface ICakeModule
    {
        /// <summary>
        /// Allows registration of services with Cake's container.
        /// </summary>
        /// <param name="container">The container.</param>
        void Register(ICakeContainer container);
    }
}
