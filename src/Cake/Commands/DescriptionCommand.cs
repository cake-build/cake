using Cake.Scripting;
using Cake.Scripting.Hosts;

namespace Cake.Commands
{
    internal sealed class DescriptionCommand : ICommand
    {
        private readonly ScriptRunner.Factory _factory;
        private readonly DescriptionScriptHost _host;

        // Delegate factory used by Autofac.
        public delegate DescriptionCommand Factory();

        public DescriptionCommand(ScriptRunner.Factory factory, DescriptionScriptHost host)
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
