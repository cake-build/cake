// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Scripting
{
    internal sealed class DryRunExecutionStrategy : IExecutionStrategy
    {
        private readonly ICakeLog _log;
        private int _counter;

        public DryRunExecutionStrategy(ICakeLog log)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            _log = log;
            _counter = 1;
        }

        public void PerformSetup(Action<ICakeContext> action, ICakeContext context)
        {
        }

        public void PerformTeardown(Action<ICakeContext> action, ICakeContext context)
        {
        }

        public void Execute(CakeTask task, ICakeContext context)
        {
            if (task != null)
            {
                _log.Information("{0}. {1}", _counter, task.Name);
                _counter++;
            }
        }

        public void Skip(CakeTask task)
        {
        }

        public void ReportErrors(Action<Exception> action, Exception exception)
        {
        }

        public void HandleErrors(Action<Exception> action, Exception exception)
        {
        }

        public void InvokeFinally(Action action)
        {
        }

        public void PerformTaskSetup(Action<ICakeContext, ITaskSetupContext> action, ICakeContext context, ITaskSetupContext setupContext)
        {
        }

        public void PerformTaskTeardown(Action<ICakeContext, ITaskTeardownContext> action, ICakeContext context, ITaskTeardownContext teardownContext)
        {
        }
    }
}
