// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Threading;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;
using Cake.Diagnostics;
using Cake.Scripting;

namespace Cake.Commands
{
    /// <summary>
    /// A command that builds and debugs a build script.
    /// </summary>
    internal sealed class DebugCommand : ICommand
    {
        private readonly IScriptRunner _scriptRunner;
        private readonly IDebugger _debugger;
        private readonly ICakeLog _log;
        private readonly BuildScriptHost _host;

        // Delegate factory used by Autofac.
        public delegate DebugCommand Factory();

        public DebugCommand(IScriptRunner scriptRunner, IDebugger debugger, BuildScriptHost host)
        {
            _scriptRunner = scriptRunner;
            _debugger = debugger;
            _host = host;
            _log = _host.Context.Log;
        }

        public bool Execute(CakeOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            var message = "Attach debugger to process {0} to continue";
            var pid = _debugger.GetProcessId();

            _log.Debug("Performing debug...");
            _log.Information(Verbosity.Quiet, message, pid);

            _debugger.WaitForAttach(Timeout.InfiniteTimeSpan);

            _log.Debug("Debugger attached");

            _scriptRunner.Run(_host, options.Script, options.Arguments);
            return true;
        }
    }
}
