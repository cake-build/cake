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
