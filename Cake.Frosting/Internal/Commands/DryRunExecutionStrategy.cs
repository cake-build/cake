// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Frosting.Internal.Commands
{
    internal sealed class DryRunExecutionStrategy : IExecutionStrategy
    {
        private readonly ICakeLog _log;
        private int _counter;

        public DryRunExecutionStrategy(ICakeLog log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _counter = 1;
        }

        public void PerformSetup(Action<ISetupContext> action, ISetupContext context)
        {
        }

        public void PerformTeardown(Action<ITeardownContext> action, ITeardownContext teardownContext)
        {
        }

        public Task ExecuteAsync(CakeTask task, ICakeContext context)
        {
            if (task != null)
            {
                _log.Information("{0}. {1}", _counter, task.Name);
                _counter++;
            }

            return Task.CompletedTask;
        }

        public void Skip(CakeTask task, CakeTaskCriteria criteria)
        {
        }

        public void ReportErrors(Action<Exception> action, Exception exception)
        {
        }

        public void HandleErrors(Action<Exception, ICakeContext> action, Exception exception, ICakeContext context)
        {
        }

        public void InvokeFinally(Action action)
        {
        }

        public void PerformTaskSetup(Action<ITaskSetupContext> action, ITaskSetupContext taskSetupContext)
        {
        }

        public void PerformTaskTeardown(Action<ITaskTeardownContext> action, ITaskTeardownContext taskTeardownContext)
        {
        }

        public Task ReportErrorsAsync(Func<Exception, Task> action, Exception exception)
        {
            return Task.CompletedTask;
        }

        public Task HandleErrorsAsync(Func<Exception, ICakeContext, Task> action, Exception exception, ICakeContext context)
        {
            return Task.CompletedTask;
        }

        public Task InvokeFinallyAsync(Func<Task> action)
        {
            return Task.CompletedTask;
        }
    }
}
