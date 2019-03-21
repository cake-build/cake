using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="CakeTask"/>.
    /// </summary>
    public static class CakeTaskExtensions
    {
        /// <summary>
        /// Adds the dependency to the task's dependencies.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="name">The name of the dependency .</param>
        /// <param name="required">Whether or not the dependency is required.</param>
        /// <exception cref="CakeException">The task already has the dependency.</exception>
        public static void AddDependency(this CakeTask task, string name, bool required = true)
        {
            if (task.Dependencies.Any(x => x.Name == name))
            {
                const string format = "The task '{0}' already have a dependency on '{1}'.";
                var message = string.Format(CultureInfo.InvariantCulture, format, task.Name, name);
                throw new CakeException(message);
            }
            task.Dependencies.Add(new CakeTaskDependency(name, required));
        }

        /// <summary>
        /// Adds the dependee to the task's dependees.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="name">The name of the dependee.</param>
        /// <param name="required">Whether or not the dependee is required.</param>
        /// <exception cref="CakeException">The task already is a dependee.</exception>
        public static void AddDependee(this CakeTask task, string name, bool required = true)
        {
            if (task.Dependees.Any(x => x.Name == name))
            {
                const string format = "The task '{0}' already is a dependee of '{1}'.";
                var message = string.Format(CultureInfo.InvariantCulture, format, task.Name, name);
                throw new CakeException(message);
            }
            task.Dependees.Add(new CakeTaskDependency(name, required));
        }

        /// <summary>
        /// Adds to the task's criteria.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="predicate">The criteria predicate.</param>
        /// <param name="message">The criteria message if skipped.</param>
        public static void AddCriteria(this CakeTask task, Func<ICakeContext, bool> predicate, string message = null)
        {
            task.Criterias.Add(new CakeTaskCriteria(predicate, message));
        }

        /// <summary>
        /// Sets the task's error handler.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="errorHandler">The error handler.</param>
        /// <exception cref="CakeException">There can only be one error handler per task.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="errorHandler"/> is null.</exception>
        public static void SetErrorHandler(this CakeTask task, Action<Exception> errorHandler)
        {
            if (task.ErrorHandler != null)
            {
                throw new CakeException("There can only be one error handler per task.");
            }
            task.ErrorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        }

        /// <summary>
        /// Sets the task's error reporter.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="errorReporter">The error reporter.</param>
        /// <exception cref="CakeException">There can only be one error reporter per task.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="errorReporter"/> is null.</exception>
        public static void SetErrorReporter(this CakeTask task, Action<Exception> errorReporter)
        {
            if (task.ErrorReporter != null)
            {
                throw new CakeException("There can only be one error reporter per task.");
            }
            task.ErrorReporter = errorReporter ?? throw new ArgumentNullException(nameof(errorReporter));
        }

        /// <summary>
        /// Sets the task's finally handler.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="finallyHandler">The finally handler.</param>
        /// <exception cref="CakeException">There can only be one finally handler per task.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="finallyHandler"/> is null.</exception>
        public static void SetFinallyHandler(this CakeTask task, Action finallyHandler)
        {
            if (task.FinallyHandler != null)
            {
                throw new CakeException("There can only be one finally handler per task.");
            }
            task.FinallyHandler = finallyHandler ?? throw new ArgumentNullException(nameof(finallyHandler));
        }

        /// <summary>
        /// Adds the action to the task's actions.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static void AddAction(this CakeTask task, Func<ICakeContext, Task> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            task.Actions.Add(action);
        }

        /// <summary>
        /// Adds the action to the task's delayed actions.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static void AddDelayedAction(this CakeTask task, Action<ICakeContext> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            task.DelayedActions.Enqueue(action);
        }

        /// <summary>
        /// Sets the task's defer exceptions state.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="value">The defer exceptions state.</param>
        public static void SetDeferExceptions(this CakeTask task, bool value)
        {
            task.DeferExceptions = value;
        }
    }
}