// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Autofac;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting.Internal;
using Cake.Frosting.Internal.Commands;

namespace Cake.Frosting
{
    internal sealed class CakeHost : ICakeHost
    {
        private readonly CakeHostOptions _options;
        private readonly IFrostingContext _context;
        private readonly IEnumerable<IFrostingTask> _tasks;
        private readonly IFrostingLifetime _lifetime;
        private readonly IFrostingTaskLifetime _taskLifetime;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeEngine _engine;
        private readonly ICakeLog _log;
        private readonly CommandFactory _commandFactory;
        private readonly EngineInitializer _engineInitializer;

        // ReSharper disable once NotAccessedField.Local
        private readonly ILifetimeScope _scope;

        public CakeHost(IFrostingContext context, ICakeEnvironment environment, ICakeEngine engine, ICakeLog log,
            ILifetimeScope scope, CakeHostOptions options, EngineInitializer engineInitializer, CommandFactory commandFactory,
            IEnumerable<IFrostingTask> tasks = null, IFrostingLifetime lifetime = null, IFrostingTaskLifetime taskLifetime = null)
        {
            Guard.ArgumentNotNull(context, nameof(context));
            Guard.ArgumentNotNull(environment, nameof(environment));
            Guard.ArgumentNotNull(engine, nameof(engine));
            Guard.ArgumentNotNull(log, nameof(log));
            Guard.ArgumentNotNull(scope, nameof(scope));
            Guard.ArgumentNotNull(options, nameof(options));
            Guard.ArgumentNotNull(engineInitializer, nameof(engineInitializer));
            Guard.ArgumentNotNull(commandFactory, nameof(commandFactory));

            _options = options;
            _engineInitializer = engineInitializer;
            _commandFactory = commandFactory;
            _scope = scope; // Keep the scope alive.
            _context = context;
            _environment = environment;
            _engine = engine;
            _log = log;
            _tasks = tasks;
            _lifetime = lifetime;
            _taskLifetime = taskLifetime;
        }

        public int Run()
        {
            try
            {
                // Update the log verbosity and working directory.
                _log.Verbosity = _options.Verbosity;
                _environment.WorkingDirectory = _options.WorkingDirectory.MakeAbsolute(_environment);

                // Initialize the engine and register everything.
                _engineInitializer.Initialize(_engine, _context, _tasks, _lifetime, _taskLifetime);

                // Get the command and execute.
                var command = _commandFactory.GetCommand(_options);
                var result = command.Execute(_engine, _options);

                // Return success.
                return result ? 0 : 1;
            }
            catch (Exception exception)
            {
                ErrorHandler.OutputError(_log, exception);
                return ErrorHandler.GetExitCode(exception);
            }
        }
    }
}