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
                    _log.Debug("Registering task {0} with engine...", taskName);

                    // Get the task's context type.
                    if (!task.HasCompatibleContext(context))
                    {
                        const string format = "Task cannot be used since the context isn't convertable to {0}.";
                        _log.Warning(format, task.GetContextType().FullName);
                    }
                    else
                    {
                        // Register task with the Cake engine.
                        var cakeTask = engine.RegisterTask(taskName);
                        cakeTask.Does(task.Run);
                        cakeTask.WithCriteria(task.ShouldRun);

                        // Add dependencies
                        var attributes = task.GetType().GetTypeInfo().GetCustomAttributes<DependencyAttribute>();
                        foreach (var dependency in attributes)
                        {
                            var dependencyName = TaskNameHelper.GetTaskName(dependency);
                            if (!typeof(IFrostingTask).IsAssignableFrom(dependency.Task))
                            {
                                throw new FrostingException($"The dependency {dependencyName} does not implement IFrostingTask.");
                            }
                            cakeTask.IsDependentOn(dependencyName);
                        }
                    }
                }
            }

            if (lifetime != null)
            {
                _log.Debug("Registering lifetime {0} with engine...", lifetime.GetType().FullName);
                engine.RegisterSetupAction(info => lifetime.Setup(context));
                engine.RegisterTeardownAction(info => lifetime.Teardown(context, info));
            }

            if (taskLifetime != null)
            {
                _log.Debug("Registering task lifetime {0} with engine...", taskLifetime.GetType().Name);
                engine.RegisterTaskSetupAction(info => taskLifetime.Setup(context, info));
                engine.RegisterTaskTeardownAction(info => taskLifetime.Teardown(context, info));
            }
        }
    }
}
