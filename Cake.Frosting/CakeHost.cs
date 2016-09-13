// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting.Internal;
using Cake.Frosting.Internal.Commands;
using Cake.Frosting.Internal.Composition;

namespace Cake.Frosting
{
    internal sealed class CakeHost : ICakeHost
    {
        private readonly CakeHostOptions _options;
        private readonly IFileSystem _fileSystem;
        private readonly IFrostingContext _context;
        private readonly IEnumerable<IFrostingTask> _tasks;
        private readonly IFrostingLifetime _lifetime;
        private readonly IFrostingTaskLifetime _taskLifetime;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeEngine _engine;
        private readonly ICakeLog _log;
        private readonly CommandFactory _commandFactory;
        private readonly WorkingDirectory _workingDirectory;
        private readonly EngineInitializer _engineInitializer;

        // ReSharper disable once NotAccessedField.Local
        private readonly Container _container;

        public CakeHost(IFrostingContext context, Container container, CakeHostOptions options,
            IFileSystem fileSystem, ICakeEnvironment environment, ICakeEngine engine, ICakeLog log,
            EngineInitializer engineInitializer, CommandFactory commandFactory,
            WorkingDirectory workingDirectory = null, IEnumerable<IFrostingTask> tasks = null,
            IFrostingLifetime lifetime = null, IFrostingTaskLifetime taskLifetime = null)
        {
            Guard.ArgumentNotNull(context, nameof(context));
            Guard.ArgumentNotNull(container, nameof(container));
            Guard.ArgumentNotNull(options, nameof(options));
            Guard.ArgumentNotNull(fileSystem, nameof(fileSystem));
            Guard.ArgumentNotNull(environment, nameof(environment));
            Guard.ArgumentNotNull(engine, nameof(engine));
            Guard.ArgumentNotNull(log, nameof(log));
            Guard.ArgumentNotNull(engineInitializer, nameof(engineInitializer));
            Guard.ArgumentNotNull(commandFactory, nameof(commandFactory));

            // Mandatory arguments.
            _context = context;
            _container = container;
            _options = options;
            _fileSystem = fileSystem;
            _environment = environment;
            _engine = engine;
            _log = log;
            _engineInitializer = engineInitializer;
            _commandFactory = commandFactory;

            // Optional arguments.
            _workingDirectory = workingDirectory;
            _tasks = tasks;
            _lifetime = lifetime;
            _taskLifetime = taskLifetime;
        }

        public int Run()
        {
            try
            {
                // Update the log verbosity.
                _log.Verbosity = _options.Verbosity;

                // Set the working directory.
                _environment.WorkingDirectory = GetWorkingDirectory();
                _log.Debug("Working directory: {0}", _environment.WorkingDirectory.FullPath);

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

        private DirectoryPath GetWorkingDirectory()
        {
            var workingDirectory = _options.WorkingDirectory ?? _workingDirectory?.Path ?? ".";
            workingDirectory = workingDirectory.MakeAbsolute(_environment);

            if (!_fileSystem.Exist(workingDirectory))
            {
                throw new FrostingException($"The working directory '{workingDirectory.FullPath}' does not exist.");
            }

            return workingDirectory;
        }
    }
}