using Cake.Core;

namespace Cake.Frosting.TaskChains
{
    /// <summary>
    /// Configures the build tasks chain.
    /// </summary>
    public interface ITaskConfigurator
    {
        /// <summary>
        /// Configures the specific task after it was added to the execution engine.
        /// </summary>
        /// <param name="task">The task class instance.</param>
        /// <param name="cakeTask">The task configuration in Cake engine.</param>
        void Configure(IFrostingTask task, CakeTaskBuilder cakeTask);
    }
}