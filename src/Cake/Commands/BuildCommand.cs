using Cake.Scripting;
using Cake.Scripting.Hosts;

namespace Cake.Commands
{
    internal sealed class BuildCommand : ICommand
    {
        private readonly ScriptRunner.Factory _factory;
        private readonly BuildScriptHost _host;

        // Delegate factory used by Autofac.
        public delegate BuildCommand Factory();

        public BuildCommand(ScriptRunner.Factory factory, BuildScriptHost host)
        {
            _factory = factory;
            _host = host;
        }

        public void Execute(CakeOptions options)
        {
            var runner = _factory(_host);
            runner.Run(options);
        }
    }
}
