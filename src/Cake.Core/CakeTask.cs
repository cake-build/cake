﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Cake.Core
{
    /// <summary>
    /// A <see cref="CakeTask"/> represents a unit of work.
    /// </summary>
    public abstract class CakeTask
    {
        private readonly string _name;
        private readonly List<string> _dependencies;
        private readonly List<Func<bool>> _criterias;

        /// <summary>
        /// Gets the name of the task.
        /// </summary>
        /// <value>The name of the task.</value>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets or sets the description of the task.
        /// </summary>
        /// <value>The description of the task.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets the task's dependencies.
        /// </summary>
        /// <value>The task's dependencies.</value>
        public IReadOnlyList<string> Dependencies
        {
            get { return _dependencies; }
        }

        /// <summary>
        /// Gets the task's criterias.
        /// </summary>
        /// <value>The task's criterias.</value>
        public IReadOnlyList<Func<bool>> Criterias
        {
            get { return _criterias; }
        }

        /// <summary>
        /// Gets the error handler.
        /// </summary>
        /// <value>The error handler.</value>
        public Action<Exception> ErrorHandler { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeTask"/> class.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        protected CakeTask(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Task name cannot be empty.");
            }
            _name = name;
            _dependencies = new List<string>();
            _criterias = new List<Func<bool>>();
        }


        /// <summary>
        /// Adds a dependency to the task.
        /// </summary>
        /// <param name="name">The name of the dependency.</param>
        public void AddDependency(string name)
        {
            if (_dependencies.Any(x => x == name))
            {
                const string format = "The task '{0}' already have a dependency on '{1}'.";
                var message = string.Format(CultureInfo.InvariantCulture, format, _name, name);
                throw new CakeException(message);
            }
            _dependencies.Add(name);
        }

        /// <summary>
        /// Adds a criteria to the task that is invoked when the task is invoked.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void AddCriteria(Func<bool> criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }
            _criterias.Add(criteria);
        }

        /// <summary>
        /// Sets the error handler for the task.
        /// The error handler is invoked when an exception is thrown from the task.
        /// </summary>
        /// <param name="errorHandler">The error handler.</param>
        public void SetErrorHandler(Action<Exception> errorHandler)
        {
            if (errorHandler == null)
            {
                throw new ArgumentNullException("errorHandler");
            }
            if (ErrorHandler != null)
            {
                throw new CakeException("There can only be one error handler per task.");
            }
            ErrorHandler = errorHandler;
        }

        /// <summary>
        /// Executes the task using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public abstract void Execute(ICakeContext context);
    }
}
