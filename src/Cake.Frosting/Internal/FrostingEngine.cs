// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;
using Cake.Frosting.TaskChains;

namespace Cake.Frosting.Internal
{
    internal interface IFrostingEngine
    {
        ExecutionSettings Settings { get; }
        CakeReport Run(IEnumerable<string> targets);
    }

    internal abstract class FrostingEngine<THost> : IFrostingEngine
        where THost : IScriptHost
    {
        private readonly List<IFrostingTask> _tasks;
        private readonly IFrostingContext _context;
        private readonly ICakeLog _log;
        private readonly IFrostingSetup _setup;
        private readonly IFrostingTeardown _teardown;
        private readonly IFrostingTaskSetup _taskSetup;
        private readonly IFrostingTaskTeardown _taskTeardown;
        private readonly THost _host;
        private readonly ICakeEngine _engine;
        private readonly ITaskConfigurator _taskConfigurator;

        public ExecutionSettings Settings => _host.Settings;

        protected FrostingEngine(
            THost host,
            ICakeEngine engine, IFrostingContext context, ICakeLog log,
            IEnumerable<IFrostingTask> tasks,
            ITaskConfigurator taskConfigurator,
            IFrostingSetup setup = null,
            IFrostingTeardown teardown = null,
            IFrostingTaskSetup taskSetup = null,
            IFrostingTaskTeardown taskTeardown = null)
        {
            _host = host;
            _engine = engine;
            _context = context;
            _log = log;
            _setup = setup;
            _teardown = teardown;
            _taskSetup = taskSetup;
            _taskTeardown = taskTeardown;
            _tasks = new List<IFrostingTask>(tasks ?? Array.Empty<IFrostingTask>());
            _taskConfigurator = taskConfigurator;
        }

        public CakeReport Run(IEnumerable<string> targets)
        {
            ConfigureTasks();
            ConfigureLifetime();
            ConfigureTaskLifetime();

            return _host.RunTargets(targets);
        }

        private void ConfigureTaskLifetime()
        {
            if (_taskSetup != null)
            {
                _log.Debug("Registering task setup: {0}", _taskSetup.GetType().Name);
                _engine.RegisterTaskSetupAction(info => _taskSetup.Setup(_context, info));
            }

            if (_taskTeardown != null)
            {
                _log.Debug("Registering task setup: {0}", _taskTeardown.GetType().Name);
                _engine.RegisterTaskTeardownAction(info => _taskTeardown.Teardown(_context, info));
            }
        }

        private void ConfigureLifetime()
        {
            if (_setup != null)
            {
                _log.Debug("Registering setup: {0}", _setup.GetType().Name);
                _engine.RegisterSetupAction(info => _setup.Setup(_context, info));
            }

            if (_teardown != null)
            {
                _log.Debug("Registering teardown: {0}", _teardown.GetType().Name);
                _engine.RegisterTeardownAction(info => _teardown.Teardown(_context, info));
            }
        }

        private void ConfigureTasks()
        {
            if (_tasks != null)
            {
                foreach (var task in _tasks)
                {
                    var name = task.GetTaskName();
                    _log.Debug("Registering task: {0}", name);

                    // Get the task's context type.
                    if (!task.HasCompatibleContext(_context))
                    {
                        const string format = "Task cannot be used since the context isn't convertible to {0}.";
                        _log.Warning(format, task.GetContextType().FullName);
                        continue;
                    }

                    // Register task with the Cake engine.
                    var cakeTask = _engine.RegisterTask(name);

                    // Configure the task.
                    _taskConfigurator.Configure(task, cakeTask);
                }
            }

            _taskConfigurator.OnConfiguredAll();
        }
    }
}
