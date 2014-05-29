using Cake.Core;
using ScriptCs.Contracts;

namespace Cake.ScriptCs
{
    public sealed class CakeScript : IScriptPackContext, ICakeEngine
    {
        private readonly CakeEngine _engine;

        public CakeScript(CakeEngine engine)
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
