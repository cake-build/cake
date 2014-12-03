using System;
using Cake.Core.Scripting;
using Cake.Scripting;

namespace Cake.Commands
{
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

        public void Execute(CakeOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            _scriptRunner.Run(_host, options.Script, options.Arguments);
        }
    }
}
