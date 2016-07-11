// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;

namespace Cake.Core
{
    /// <summary>
    /// A task that executes a specified delegate.
    /// </summary>
    public sealed class ActionTask : CakeTask
    {
        private readonly List<Action<ICakeContext>> _actions;

        /// <summary>
        /// Gets the task's actions.
        /// </summary>
        /// <value>The task's actions.</value>
        public List<Action<ICakeContext>> Actions
        {
            get { return _actions; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionTask"/> class.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        public ActionTask(string name)
            : base(name)
        {
            _actions = new List<Action<ICakeContext>>();
        }

        /// <summary>
        /// Adds an action to the task.
        /// </summary>
        /// <param name="action">The action.</param>
        public void AddAction(Action<ICakeContext> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            _actions.Add(action);
        }

        /// <summary>
        /// Executes the task using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Execute(ICakeContext context)
        {
            foreach (var action in _actions)
            {
                action(context);
            }
        }
    }
}
