// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core.Scripting;
using Cake.Scripting;

namespace Cake.Commands
{
    /// <summary>
    /// A command that displays information about script tasks.
    /// </summary>
    internal sealed class DescriptionCommand : ICommand
    {
        private readonly IScriptRunner _scriptRunner;
        private readonly DescriptionScriptHost _host;

        // Delegate factory used by Autofac.
        public delegate DescriptionCommand Factory();

        public DescriptionCommand(IScriptRunner scriptRunner, DescriptionScriptHost host)
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
