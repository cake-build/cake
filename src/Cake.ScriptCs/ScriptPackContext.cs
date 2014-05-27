using Cake.Core;
using ScriptCs.Contracts;

namespace Cake.ScriptCs
{
    public sealed class ScriptPackContext : IScriptPackContext
    {
        private readonly CakeEngine _engine;

        public ScriptPackContext(CakeEngine engine)
        {
            _engine = engine;
        }

        public CakeTask Task(string name)
        {
            return _engine.Task(name);
        }

        public void Run(string target)
        {
            _engine.Run(target);
        }
    }
}
