using Cake.Core;
using Cake.Frosting.Internal;

namespace Cake.Frosting.TaskChains
{
    /// <summary>
    /// Configures the build task's default behavior.
    /// </summary>
    /// <seealso cref="Cake.Frosting.TaskChains.ITaskConfigurator" />
    public class DefaultTaskConfigurator : ITaskConfigurator
    {
        private readonly IFrostingContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTaskConfigurator"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public DefaultTaskConfigurator(IFrostingContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Configures the specific task after it was added to the execution engine.
        /// </summary>
        /// <param name="task">The task class instance.</param>
        /// <param name="cakeTask">The task configuration in Cake engine.</param>
        public virtual void Configure(IFrostingTask task, CakeTaskBuilder cakeTask)
        {
            var description = task.GetTaskDescription();
            if (!string.IsNullOrWhiteSpace(description))
            {
                cakeTask.Description(description);
            }

            // Is the run method overridden?
            if (task.IsRunOverridden(_context))
            {
                cakeTask.Does(task.RunAsync);
            }

            // Is the criteria method overridden?
            if (task.IsShouldRunOverridden(_context))
            {
                cakeTask.WithCriteria(task.ShouldRun, task.SkippedMessage);
            }

            // Continue on error?
            if (task.IsContinueOnError())
            {
                cakeTask.ContinueOnError();
            }

            // Is the on error method overridden?
            if (task.IsOnErrorOverridden(_context))
            {
                cakeTask.OnError(exception => task.OnError(exception, _context));
            }

            // Is the finally method overridden?
            if (task.IsFinallyOverridden(_context))
            {
                cakeTask.Finally(() => task.Finally(_context));
            }

            // Add dependencies (if not already added by the task execution chain)
            foreach (var dependency in task.GetDependencies())
            {
                var dependencyName = dependency.GetTaskName();
                if (!typeof(IFrostingTask).IsAssignableFrom(dependency.Task))
                {
                    throw new FrostingException($"The dependency '{dependencyName}' is not a valid task.");
                }

                cakeTask.IsDependentOn(dependencyName);
            }

            // Add reverse dependencies (if not already added by the task execution chain)
            foreach (var dependee in task.GetReverseDependencies())
            {
                var dependeeName = dependee.GetTaskName();
                if (!typeof(IFrostingTask).IsAssignableFrom(dependee.Task))
                {
                    throw new FrostingException($"The reverse dependency '{dependeeName}' is not a valid task.");
                }

                cakeTask.IsDependeeOf(dependeeName);
            }
        }
    }
}