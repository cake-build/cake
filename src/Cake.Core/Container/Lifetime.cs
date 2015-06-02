namespace Cake.Core.Container
{
    /// <summary>
    /// Represents a lifetime.
    /// </summary>
    public enum Lifetime
    {
        /// <summary>
        /// Instance per dependency lifetime.
        /// </summary>
        InstancePerDependency,

        /// <summary>
        /// Singleton lifetime.
        /// </summary>
        Singleton
    }
}