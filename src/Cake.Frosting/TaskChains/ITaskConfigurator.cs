using Cake.Core;

namespace Cake.Frosting.TaskChains
{
    public interface ITaskConfigurator
    {
        /// <summary>
        /// Configures the specific task after it was added to the execution engine.
        /// </summary>
        /// <param name="task">The task class instance.</param>
        /// <param name="cakeTask">The task configuration in Cake engine.</param>
        void Configure(IFrostingTask task, CakeTaskBuilder cakeTask);

        /// <summary>
        /// Called when all build tasks have been configured but not executed.
        /// </summary>
        void OnConfiguredAll();
    }
}