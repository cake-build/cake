// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reflection;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Frosting.Internal
{
    internal sealed class EngineInitializer
    {
        private readonly ICakeLog _log;

        public EngineInitializer(ICakeLog log)
        {
            _log = log;
        }

        public void Initialize(ICakeEngine engine, IFrostingContext context, IEnumerable<IFrostingTask> tasks,
            IFrostingLifetime lifetime, IFrostingTaskLifetime taskLifetime)
        {
            if (tasks != null)
            {
                foreach (var task in tasks)
                {
                    var taskName = TaskNameHelper.GetTaskName(task);
                    _log.Debug("Registering task: {0}", taskName);

                    // Get the task's context type.
                    if (!task.HasCompatibleContext(context))
                    {
                        const string format = "Task cannot be used since the context isn't convertible to {0}.";
                        _log.Warning(format, task.GetContextType().FullName);
                    }
                    else
                    {
                        // Register task with the Cake engine.
                        var cakeTask = engine.RegisterTask(taskName);

                        // Is the run method overridden?
                        if (task.IsRunOverridden(context))
                        {
                            cakeTask.Does(task.RunAsync);
                        }

                        // Is the criteria method overridden?
                        if (task.IsShouldRunOverridden(context))
                        {
                            cakeTask.WithCriteria(task.ShouldRun);
                        }

                        // Continue on error?
                        if (task.IsContinueOnError())
                        {
                            cakeTask.ContinueOnError();
                        }

                        // Is the on error method overridden?
                        if (task.IsOnErrorOverridden(context))
                        {
                            cakeTask.OnError(exception => task.OnError(exception, context));
                        }

                        // Is the finally method overridden?
                        if (task.IsFinallyOverridden(context))
                        {
                            cakeTask.Finally(() => task.Finally(context));
                        }

                        // Add dependencies
                        var attributes = task.GetType().GetTypeInfo().GetCustomAttributes<DependencyAttribute>();
                        foreach (var dependency in attributes)
                        {
                            var dependencyName = TaskNameHelper.GetTaskName(dependency);
                            if (!typeof(IFrostingTask).IsAssignableFrom(dependency.Task))
                            {
                                throw new FrostingException($"The dependency '{dependencyName}' is not a valid task.");
                            }
                            cakeTask.IsDependentOn(dependencyName);
                        }
                    }
                }
            }

            if (lifetime != null)
            {
                _log.Debug("Registering lifetime: {0}", lifetime.GetType().Name);

                if (lifetime.IsSetupOverridden(context))
                {
                    engine.RegisterSetupAction(info => lifetime.Setup(context));
                }
                if (lifetime.IsTeardownOverridden(context))
                {
                    engine.RegisterTeardownAction(info => lifetime.Teardown(context, info));
                }
            }

            if (taskLifetime != null)
            {
                _log.Debug("Registering task lifetime: {0}", taskLifetime.GetType().Name);

                if (taskLifetime.IsSetupOverridden(context))
                {
                    engine.RegisterTaskSetupAction(info => taskLifetime.Setup(context, info));
                }

                if (taskLifetime.IsTeardownOverridden(context))
                {
                    engine.RegisterTaskTeardownAction(info => taskLifetime.Teardown(context, info));
                }
            }
        }
    }
}