// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core.Scripting;
using Cake.Scripting;

namespace Cake.Commands
{
    /// <summary>
    /// A command that builds and runs a build script.
    /// </summary>
    internal sealed class BuildCommand : ICommand
    {
        private readonly IScriptRunner _scriptRunner;
        private readonly BuildScriptHost _host;

        // Delegate factory used by Autofac.
        public delegate BuildCommand Factory();

        public BuildCommand(IScriptRunner scriptRunner, BuildScriptHost host)
        {
            _scriptRunner = scriptRunner;
            _host = host;
        }

        public bool Execute(CakeOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            _scriptRunner.Run(_host, options.Script, options.Arguments);
            return true;
        }
    }
}
