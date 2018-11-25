using System.Collections.Generic;

namespace Cake.Core
{
    /// <summary>
    /// Acts as a context providing info about the overall build before its started.
    /// </summary>
    public interface ISetupContext : ICakeContext
    {
        /// <summary>
        /// Gets target (initiating) task.
        /// </summary>
        ICakeTaskInfo TargetTask { get; }

        /// <summary>
        /// Gets all registered tasks that are going to be executed.
        /// </summary>
        IReadOnlyCollection<ICakeTaskInfo> TasksToExecute { get; }
    }
}