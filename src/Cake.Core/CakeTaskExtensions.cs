using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Cake.Core
{
    internal static class CakeTaskExtensions
    {
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

        public static void AddCriteria(this CakeTask task, Func<ICakeContext, bool> criteria, string message = null)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }
            task.Criterias.Add(new CakeTaskCriteria(criteria, message));
        }

        public static void SetErrorHandler(this CakeTask task, Action<Exception> errorHandler)
        {
            if (task.ErrorHandler != null)
            {
                throw new CakeException("There can only be one error handler per task.");
            }
            task.ErrorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        }

        public static void SetErrorReporter(this CakeTask task, Action<Exception> errorReporter)
        {
            if (task.ErrorReporter != null)
            {
                throw new CakeException("There can only be one error reporter per task.");
            }
            task.ErrorReporter = errorReporter ?? throw new ArgumentNullException(nameof(errorReporter));
        }

        public static void SetFinallyHandler(this CakeTask task, Action finallyHandler)
        {
            if (task.FinallyHandler != null)
            {
                throw new CakeException("There can only be one finally handler per task.");
            }
            task.FinallyHandler = finallyHandler ?? throw new ArgumentNullException(nameof(finallyHandler));
        }

        public static void AddAction(this CakeTask task, Func<ICakeContext, Task> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            task.Actions.Add(action);
        }

        public static void AddDelayedAction(this CakeTask task, Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            task.DelayedActions.Enqueue(action);
        }

        public static void SetDeferExceptions(this CakeTask task, bool value)
        {
            task.DeferExceptions = value;
        }
    }
}