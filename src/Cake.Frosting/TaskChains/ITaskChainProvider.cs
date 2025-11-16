namespace Cake.Frosting.TaskChains
{
    /// <summary>
    /// Provider the tasks chain to be applied to the build.
    /// </summary>
    public interface ITaskChainProvider
    {
        /// <summary>
        /// Gets the tasks chain.
        /// </summary>
        /// <returns>The tasks chain.</returns>
        TaskChainItem GetChain();
    }
}